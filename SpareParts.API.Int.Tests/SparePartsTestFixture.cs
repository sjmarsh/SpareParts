using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using SpareParts.API.Data;
using SpareParts.Shared.Models;

namespace SpareParts.API.Int.Tests
{
    //ref:
    //https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/
    //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0

    [CollectionDefinition("Spare Parts Tests")]
    public class SparePartsTestCollection : ICollectionFixture<SparePartsTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class SparePartsTestFixture : IAsyncLifetime, IDisposable
    {
        public SparePartsTestFixture()
        {
            DbContext = SetupDbContext();
            HttpClient = SetupTestClient();             
        }

        public SparePartsDbContext DbContext { get; private set; }
        public HttpClient HttpClient { get; private set; }

        private string? _authToken { get; set; }

        public async Task InitializeAsync()
        {
            _authToken = await GetAuthToken();
        }

        public async Task DisposeAsync()
        {
            _authToken = null;
        }

        private async Task<string> GetAuthToken()
        {
            var authRequest = new AuthenticationRequest("admin", "password");
            var authenticationResponse = await PostRequest<AuthenticationRequest, AuthenticationResponse>("/api/user/authenticate", authRequest);
            authenticationResponse.Should().NotBeNull();
            authenticationResponse.Token.Should().NotBeNullOrEmpty();
            return authenticationResponse.Token!;
        }

        public void AddAuthHeaderToClient()
        {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
        }

        public async Task<HttpResponseMessage> Get(string uri)
        {
            using HttpResponseMessage response = await HttpClient.GetAsync(uri);
            return response;
        }

        public async Task<TOut> GetRequest<TOut>(string uri) where TOut : new()
        {
            try
            {
                using HttpResponseMessage response = await HttpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TOut>(responseBody);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            return await Task.FromResult(new TOut());
        }

        public async Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content) where TOut : new()
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await HttpClient.PostAsync(uri, serialized);
                
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TOut>(responseBody);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            return await Task.FromResult(new TOut());
        }

        public async Task<TOut> PutRequest<TIn, TOut>(string uri, TIn content) where TOut : new()
        {
            try
            {
                HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await HttpClient.PutAsync(uri, serialized);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TOut>(responseBody);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            return await Task.FromResult(new TOut());
        }

        public async Task<TOut> DeleteRequest<TOut>(string uri) where TOut : new()
        {
            try
            {
                using HttpResponseMessage response = await HttpClient.DeleteAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TOut>(responseBody);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }

            return await Task.FromResult(new TOut());
        }

        private SparePartsDbContext SetupDbContext()
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
            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();
            return dbContext;
        }

        private HttpClient SetupTestClient()
        {
            var application = new CustomWebApplicationFactory<Program>()
                                    .WithWebHostBuilder(builder =>
                                    {
                                        // ... Configure test services
                                        builder.ConfigureServices(services =>
                                        {
                                            
                                        });

                                    });

            return application.CreateClient();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Database.CloseConnection();
            DbContext.Dispose();
            HttpClient.Dispose();
        }
    }
        
    // ref: Issue with not picking up custom app settings for tests
    // https://github.com/dotnet/aspnetcore/issues/38435
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("IntegrationTest");

            return base.CreateHost(builder);
        }
    }

}
