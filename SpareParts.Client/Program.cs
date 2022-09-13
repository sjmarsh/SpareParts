using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SpareParts.Client;
using SpareParts.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddRefitClientFor<IPartService>(builder.HostEnvironment.BaseAddress);
builder.Services.AddRefitClientFor<IInventoryService>(builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();
