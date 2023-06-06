using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SpareParts.API.Data;
using SpareParts.API.Entities;
using SpareParts.API.Extensions;
using SpareParts.API.GraphQL;
using SpareParts.API.Infrastructure;
using SpareParts.API.Services;
using SpareParts.API.Services.PdfService;
using SpareParts.Shared.Validators;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
   
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json");

    // This is needed to address issue with Custom Environments https://github.com/dotnet/aspnetcore/issues/38212
    if(builder.Environment.IsIntegrationTest())
    {
        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);
    }

    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()
    ));

    builder.Services.AddHttpContextAccessor();

    // Entity Framework
    string connectionString = "name=ConnectionStrings:SparePartsDbConnection";
    if (builder.Environment.IsDockerDev())
    {
        var dockerSqlConnString = Environment.GetEnvironmentVariable("DOCKER_SQL_CONN_STRING");
        if(dockerSqlConnString == null)
        {
            throw new ConfigurationException("The connection string for the Docker SQL instance has not been configured correctly.");
        }
        else
        {
            connectionString = dockerSqlConnString;
        }
    }
    
    builder.Services.AddDbContext<SparePartsDbContext>(options => options.UseSqlServer(connectionString));

    // Identity / Auth
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<SparePartsDbContext>();

    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["ValidIssuer"],
            ValidAudience = jwtSettings["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SigninKey"]))
        };
    });

    // Validation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<PartValidator>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Register MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

    // AutoMapper
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // Add required services for Blazor Client (hosted by this WebAPI)
    builder.Services.AddControllersWithViews();
    builder.Services.AddRazorPages();

    // Other Services - DI Registration
    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddTransient<IDataService, DataService>();
    if (builder.Environment.IsDockerDev())
    {
        builder.Services.AddTransient<IPdfService, LinuxPdfService>();
    }
    else
    {
        builder.Services.AddTransient<IPdfService, WindowsPdfService>();
    }
    builder.Services.AddTransient<IReportService, ReportService>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Default",
            builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

        options.AddPolicy("React",
            builder => builder
                .WithOrigins("https://localhost:3000")
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
    });

    builder.Services
        .AddGraphQLServer()
        .AddAuthorization()
        .AddFiltering()
        .RegisterDbContext<SparePartsDbContext>()
        .AddQueryType<Query>();

    var app = builder.Build();

    app.UseHttpsRedirection();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsIntegrationTest())
    {
        app.UseCors("React");

        app.UseSwagger();
        app.UseSwaggerUI();

        // Required for Blazor WASM
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. Change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.MapGraphQL();
        
    // For Blazor
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.MapRazorPages();
    app.MapFallbackToFile("index.html");
        
    app.UseContentSecurityPolicy();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    

    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-AU");
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-AU");

    if (app.Environment.IsDevelopment() || app.Environment.IsDockerDev())  
    {
        // Note: Production refers to app running in docker. Setting up database here for convenience in dev environment. TODO: use migration script if working in real prod.
        using var scope = app.Services.CreateScope();
        Log.Information("Setup database");
        var context = scope.ServiceProvider.GetRequiredService<SparePartsDbContext>();
        context.Database.Migrate();
    }

    if (!app.Environment.IsDevelopment() && !app.Environment.IsIntegrationTest() && !app.Environment.IsDockerDev())
    {
        Log.Information("Starting default browser.");
        Process.Start(new ProcessStartInfo("http://localhost:5000") { UseShellExecute = true });
    }

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception occurred during application start up.");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

public partial class Program { }