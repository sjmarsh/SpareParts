using SpareParts.Shared.Models;
using SpareParts.Test.Helpers;

namespace SpareParts.API.Int.Tests
{
    [Collection("Spare Parts Tests")]
    public class PartTests
    {
        private readonly SparePartsTestFixture _testFixture;
        private readonly DataHelper _dataHelper;
        
        public PartTests(SparePartsTestFixture testFixture)
        {
            _testFixture = testFixture;
            _dataHelper = new DataHelper(_testFixture.DbContext);

            // clear parts table between tests
            _testFixture.DbContext.Parts.RemoveRange(_testFixture.DbContext.Parts);
            _testFixture.DbContext.SaveChanges();
            _testFixture.DbContext.ChangeTracker.Clear();

            // ensure client has Auth Header set
            _testFixture.AddAuthHeaderToClient();
        }

        [Fact]
        public async Task Get_Should_ReturnUnauthorizedWhenNoTokenHeader()
        {
            _testFixture.HttpClient.DefaultRequestHeaders.Remove("Authorization");

            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.Get($"/api/part/?id={savedPart.ID}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Should_ReturnRequiredPartRecord()
        {
            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.GetRequest<PartResponse>($"/api/part/?id={savedPart.ID}");
            
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(savedPart);
        }

        [Fact]
        public async Task GetIndex_Should_ReturnListOfParts()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(5);
                        
            var result = await _testFixture.GetRequest<PartListResponse>($"/api/part/index");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.Items.Should().BeEquivalentTo(parts);
        }

        [Fact]
        public async Task GetIndex_Should_ReturnListOfPartsWithRequiredPaging()
        {
            await _dataHelper.CreatePartListInDatabase(5);

            var result = await _testFixture.GetRequest<PartListResponse>($"/api/part/index?skip=2&take=2");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetIndex_Should_ReturnListOfCurrentParts()
        {
            var parts = _dataHelper.GetPartsFakerConfig().Generate(3);
            parts[0].StartDate = DateTime.Today.AddYears(-1); // current
            parts[0].EndDate = null;
            parts[1].StartDate = DateTime.Today.AddYears(-1); // not current
            parts[1].EndDate = DateTime.Today.AddDays(-2);
            parts[2].StartDate = DateTime.Today.AddYears(1); // not current
            parts[2].EndDate = DateTime.Today.AddDays(2);

            foreach (var part in parts)
            {
                _testFixture.DbContext.Parts.Add(part);
            }
            await _testFixture.DbContext.SaveChangesAsync();

            var result = await _testFixture.GetRequest<PartListResponse>($"/api/part/index?isCurrentOnly=true");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(1);
            result.Items[0].Should().BeEquivalentTo(parts[0]);
        }


        [Fact]
        public async Task Post_Should_CreatePartRecord()
        {
            var part = new Part { Name = "Part 1", Description = "One", Weight = 1.1, Price = 2.2, StartDate = DateTime.Today.AddYears(-1) };
                        
            var result = await _testFixture.PostRequest<Part, PartResponse>("/api/part", part);
            
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            
            var partId = result?.Value?.ID;
            Assert.True(partId > 0);
            var savedPart = await _testFixture.DbContext.Parts.FindAsync(partId);
            
            savedPart.Should().NotBeNull();
            savedPart.Should().BeEquivalentTo(part, opt => opt.Excluding(p => p.ID));
        }

        [Fact]
        public async Task Put_Should_UpdatePartRecord()
        {
            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);
            var partModel = new Part { ID = savedPart.ID, Name = "Other Name", Description = "Other Descritpion", Weight = savedPart.Weight, Price = savedPart.Price.Value, StartDate = savedPart.StartDate };
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.PutRequest<Part, PartResponse>("/api/part", partModel);

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();

            var partId = result?.Value?.ID;
            Assert.True(partId > 0);
            var updatedPart = await _testFixture.DbContext.Parts.FindAsync(partId);

            updatedPart.Should().NotBeNull();
            updatedPart.Should().BeEquivalentTo(partModel, opt => opt.Excluding(p => p.ID));
        }

        [Fact]
        public async Task Delete_Should_DeletePartRecord()
        {
            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.DeleteRequest<PartResponse>($"/api/part/?id={savedPart.ID}");

            result.Value.Should().BeNull();
            result.HasError.Should().BeFalse();
            var deletedPart = await _testFixture.DbContext.Parts.FindAsync(savedPart.ID);
            deletedPart.Should().BeNull();
        }        
    }
}