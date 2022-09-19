using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpareParts.Client;
using SpareParts.Client.Services;
using SpareParts.Client.Services.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<AuthHeaderHandler>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRefitClientFor<IUserService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<IPartService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<IInventoryService>(builder.HostEnvironment.BaseAddress);



await builder.Build().RunAsync();
