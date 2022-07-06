using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SpareParts.API.Data;
using SpareParts.API.Services;
using SpareParts.Shared.Validators;
using System.Reflection;
using System.Text.Json.Serialization;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

    // Add services to the container.

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()
        )
    );

    // Validation
    builder.Services.AddMvc()
      .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PartValidator>());

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Register MediatR
    builder.Services.AddMediatR(typeof(Program));

    // Entity Framework
    builder.Services.AddDbContext<SparePartsDbContext>(options => options.UseSqlServer("name=ConnectionStrings:SparePartsDbConnection"));

    // AutoMapper
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    // Other Services - DI Registration
    builder.Services.AddTransient<IDataService, DataService>();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

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