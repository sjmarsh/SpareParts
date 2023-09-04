using Newtonsoft.Json;
using SpareParts.Shared.Models;
using SpareParts.Test.Helpers;
using System.Net.Http.Headers;
using System.Text;

namespace SpareParts.API.Int.Tests
{
    [Collection("Spare Parts Tests")]
    public class GraphQLQueryTests
    {
        private readonly SparePartsTestFixture _testFixture;
        private readonly DataHelper _dataHelper;

        public GraphQLQueryTests(SparePartsTestFixture testFixture)
        {
            _testFixture = testFixture;
            _dataHelper = new DataHelper(_testFixture.DbContext);

            // clear parts table between tests
            if (_testFixture.DbContext.Parts.Any())
            {
                _testFixture.DbContext.InventoryItems.RemoveRange(_testFixture.DbContext.InventoryItems);
                _testFixture.DbContext.PartAttribute.RemoveRange(_testFixture.DbContext.PartAttribute);
                _testFixture.DbContext.Parts.RemoveRange(_testFixture.DbContext.Parts);
                _testFixture.DbContext.SaveChanges();
            }
            _testFixture.DbContext.ChangeTracker.Clear();

            // ensure client has Auth Header set
            _testFixture.AddAuthHeaderToClient();
        }

        [Fact]
        public async Task Post_Should_ReturnUnauthorizedWhenNoTokenHeader()
        {
            _testFixture.HttpClient.DefaultRequestHeaders.Remove("Authorization");

            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var request = new GraphQLRequest { query = "{\r\n  parts {\r\n   items {\r\n    name\r\n    description\r\n  }\r\n}\r\n}" };

            _testFixture.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var serialized = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _testFixture.HttpClient.PostAsync("/graphql", serialized);           
            
            response.Should().NotBeNull();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("not authorized");
        }

        [Fact]
        public async Task Post_Should_ReturnParts()
        {
            const int NumberOfParts = 3;
            var parts = await _dataHelper.CreatePartListInDatabase(NumberOfParts);
            
            var request = new GraphQLRequest { query = 
@"{
    parts {
        items { 
            name
            description
            price
            weight
            startDate
            endDate
            attributes {
                name
                description
                value
            }
        }
    }
}" 
    };

            var result = await _testFixture.PostRequest<GraphQLRequest, PartGraphQLResponsePaged>("/graphql", request);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            var resultItems = result.Data!.Parts!.Items;
            resultItems.Should().HaveCount(NumberOfParts);
            resultItems.Should().BeEquivalentTo(parts, opts =>
            {
                opts.Excluding(ctx => ctx.ID);
                opts.For(ctx => ctx.Attributes).Exclude(a => a.ID);
                opts.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(5))).WhenTypeIs<DateTime>();
                return opts;
            });
        }

        [Fact]
        public async Task Post_Should_ReturnFilteredParts()
        {
            const int NumberOfParts = 3;
            var parts = await _dataHelper.CreatePartListInDatabase(NumberOfParts);
            var part1 = parts.First();

            var request = new GraphQLRequest
            {
                query =
$@"{{
    parts (where: {{ name: {{eq: ""{part1.Name}""}}}}) {{
        items {{ 
            name
            description
            price
            weight
            startDate
            endDate
            attributes {{
                name
                description
                value
            }}
        }}
    }}
}}"
            };

            var result = await _testFixture.PostRequest<GraphQLRequest, PartGraphQLResponsePaged>("/graphql", request);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            var resultItems = result.Data!.Parts!.Items;
            resultItems.Should().NotBeNull();
            resultItems.Should().HaveCount(1);
            resultItems![0].Should().BeEquivalentTo(part1, options =>
            {
                options.Excluding(ctx => ctx.ID);
                options.For(ctx => ctx.Attributes).Exclude(a => a.ID);
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(5))).WhenTypeIs<DateTime>();
                return options;
            });
        }

        [Fact]
        public async Task Post_Should_OmitFieldIfNotIncludedInQuery()
        {
            const int NumberOfParts = 3;
            var parts = await _dataHelper.CreatePartListInDatabase(NumberOfParts);
            var part1 = parts.First();

            // query excludes the "description" field
            var request = new GraphQLRequest
            {
                query =
$@"{{
    parts (where: {{ name: {{eq: ""{part1.Name}""}}}}) {{
        items {{ 
            name
            price
            weight
            startDate
            endDate
            attributes {{
                name
                description
                value
            }}        
        }}
    }}
}}"
            };

            var result = await _testFixture.PostRequest<GraphQLRequest, PartGraphQLResponsePaged>("/graphql", request);

            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            var resultItems = result.Data!.Parts!.Items;
            resultItems.Should().NotBeNull();
            resultItems.Should().HaveCount(1);

            resultItems![0].Should().BeEquivalentTo(part1, options =>
            {
                options.Excluding(ctx => ctx.ID);
                options.For(ctx => ctx.Attributes).Exclude(a => a.ID);
                options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(5))).WhenTypeIs<DateTime>();
                options.Excluding(p => p.Description);  // exclude "description" from overall object equality
                return options;
            });

            // check "description" is actually null
            resultItems![0].Description.Should().BeNull();
        }

        [Fact]
        public async Task Should_ReturnNullData_WhenInvalidFields()
        {
            const int NumberOfParts = 3;
            await _dataHelper.CreatePartListInDatabase(NumberOfParts);

            var request = new GraphQLRequest
            {
                query =
@"{
    parts {
        items {
            notAField
            alsoNotAThing
        }
    }
}"
            };

            var result = await _testFixture.PostRequest<GraphQLRequest, PartGraphQLResponsePaged>("/graphql", request);

            result.Data.Should().BeNull();
        }
    }
}
