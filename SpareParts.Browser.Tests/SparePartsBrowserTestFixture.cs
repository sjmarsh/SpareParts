using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;
using System.Net;
using System.Net.Sockets;

namespace SpareParts.Browser.Tests
{
    [CollectionDefinition("Browser Tests")]
    public class SparePartsBrowserTestCollection : ICollectionFixture<SparePartsBrowserTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class SparePartsBrowserTestFixture : IAsyncLifetime, IDisposable
    {
        
        private WebApplicationFactoryFixture<Program> _factory;
                
        private IPlaywright Playwright { get; set; }
        public IBrowser Browser { get; private set; }
        public string BaseUrl { get; } = $"https://localhost:{GetRandomUnusedPort()}";
        public SparePartsDbContext DbContext { get; private set; }

        public PageModels Pages { get; private set; }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public SparePartsBrowserTestFixture()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _factory = SetupTestClient();
        }

        public async Task InitializeAsync()
        {
            DbContext = await SetupDbContextAsync();
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            var opts = new BrowserTypeLaunchOptions { Headless = false };
            Browser = await Playwright.Chromium.LaunchAsync(opts);

            var page = await Browser.NewPageAsync();
            await page.GotoAsync(BaseUrl);
            Pages = new PageModels(page);
        }              

        private async Task<SparePartsDbContext> SetupDbContextAsync()
        {
            // Use appsettings from the API project
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.IntegrationTest.json")
                .Build();

            var options = new DbContextOptionsBuilder<SparePartsDbContext>()
                                .UseSqlServer(config["ConnectionStrings:SparePartsDbConnection"])
                                .Options;
            var dbContext = new SparePartsDbContext(options);
            await dbContext.Database.MigrateAsync();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        private WebApplicationFactoryFixture<Program> SetupTestClient()
        {
            var factory = new WebApplicationFactoryFixture<Program>
            {
                HostUrl = BaseUrl
            };
            factory.CreateDefaultClient();
            return factory;
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public void Dispose()
        {
            _factory.Dispose();

            Playwright?.Dispose();
        }

        public async Task DisposeAsync()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.CloseConnection();
            DbContext.Dispose();
            Playwright?.Dispose();
        }
    }

    //ref: https://stackoverflow.com/questions/71541576/using-webapplicationfactory-to-do-e2e-testing-with-net-6-minimal-api
    public class WebApplicationFactoryFixture<TEntryPoint> : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
    {
        public string HostUrl { get; set; } = "https://localhost:5001"; // we can use any free port

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //builder.UseEnvironment("IntegrationTest");
            builder.UseUrls(HostUrl);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var dummyHost = builder.Build();
            
            //builder.UseEnvironment("IntegrationTest");
            builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

            var host = builder.Build();
            host.Start();

            return dummyHost;
        }
    }

}
