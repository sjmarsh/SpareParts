using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SpareParts.API.Data;
using SpareParts.Shared.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SpareParts.API.Int.Tests
{
    public class PartTests : IDisposable
    {
        //ref:
        //https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/
        //https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0


        const string _databaseName = "SpareParts_Test";
        SparePartsDbContext _dbContext;
        HttpClient _client;

        public PartTests()
        {
            var options = new DbContextOptionsBuilder<SparePartsDbContext>()
                                .UseSqlServer($"Server=SJM-ULTRABOOK;Database={_databaseName};Trusted_Connection=True")
                                .Options;
            _dbContext = new SparePartsDbContext(options);
            _dbContext.Database.Migrate();

            var application = new WebApplicationFactory<Program>()
                                    .WithWebHostBuilder(builder =>
                                    {
                                        // ... Configure test services
                                        builder.ConfigureServices(services =>
                                        {
                                            services.AddDbContext<SparePartsDbContext>(options => options.UseSqlServer($"Server=SJM-ULTRABOOK;Database={_databaseName};Trusted_Connection=True"));
                                        });                                        
                                    });

            _client = application.CreateClient();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.CloseConnection();
        }

        [Fact]
        public async void Post_Should_CreatePartRecord()
        {
            var part = new Part { Name = "Part 1", Description = "One", Weight = 1.1, Price = 2.2, StartDate = DateTime.Today.AddYears(-1) };
            var result = await PostRequest<Part, PartResponse>("/part", part);
            Assert.NotNull(result);
            Assert.NotNull(result.Value);

        }

        private async Task<TOut> PostRequest<TIn, TOut>(string uri, TIn content) where TOut : new()
        {
            try
            {
                //using (var client = new HttpClient())
                //{
                    _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialized = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await _client.PostAsync(uri, serialized))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();

                        return JsonConvert.DeserializeObject<TOut>(responseBody);
                    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return await Task.FromResult(new TOut());
            
        }

    }
}