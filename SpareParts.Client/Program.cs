using FluentValidation;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpareParts.Client;
using SpareParts.Client.Services;
using SpareParts.Client.Services.Authentication;
using SpareParts.Client.Shared.Components.Filter;
using SpareParts.Client.Shared.Components.Toast;
using SpareParts.Shared.Models;
using SpareParts.Shared.Validators;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddServiceWrapper();
builder.Services.AddLoadingIndicatorService();
builder.Services.AddMessageBoxService();
builder.Services.AddToastService();
builder.Services.AddAuthorizationCore();
builder.Services.AddInMemoryAuthTokenStore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddScoped<IGraphQLRequestBuilder, GraphQLRequestBuilder>();

builder.Services.AddSingleton<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();
builder.Services.AddSingleton<IValidator<Part>, PartValidator>();
builder.Services.AddSingleton<IValidator<PartAttribute>, PartAttributeValidator>();
builder.Services.AddSingleton<IValidator<InventoryItem>, InventoryItemValidator>();
builder.Services.AddSingleton<IValidator<InventoryItemDetail>, InventoryItemDetailValidator>();
builder.Services.AddSingleton<IValidator<List<InventoryItem>>, InventoryItemListValidator>();
builder.Services.AddSingleton<IValidator<StocktakeModel>, StocktakeValidator>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRefitClientFor<IUserService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<IPartService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<IInventoryService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<ISearchService>(builder.HostEnvironment.BaseAddress);

builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly));

await builder.Build().RunAsync();
