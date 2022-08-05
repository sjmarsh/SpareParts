using SpareParts.Shared.Models;

namespace SpareParts.API.Int.Tests
{
    [Collection("Spare Parts Tests")]
    public class PartTests 
    {
        private readonly SparePartsTestFixture _testFixture;

        public PartTests(SparePartsTestFixture testFixture)
        {
            _testFixture = testFixture;

            // clear parts table between tests
            _testFixture.DbContext.Parts.RemoveRange(_testFixture.DbContext.Parts);
            _testFixture.DbContext.SaveChanges();
        }

        [Fact]
        public async void Get_Should_ReturnRequiredPartRecord()
        {
            var savedPart = await CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.GetRequest<PartResponse>($"/api/part/?id={savedPart.ID}");
            
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(savedPart);
        }

        [Fact]
        public async void GetIndex_Should_ReturnListOfParts()
        {
            var parts = await CreateListInDatabase(5);
                        
            var result = await _testFixture.GetRequest<PartListResponse>($"/api/part/index");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(5);
            result.Items.Should().BeEquivalentTo(parts);
        }

        [Fact]
        public async void GetIndex_Should_ReturnListOfPartsWithRequiredPaging()
        {
            await CreateListInDatabase(5);

            var result = await _testFixture.GetRequest<PartListResponse>($"/api/part/index?skip=2&take=2");

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().HaveCount(2);
        }

        [Fact]
        public async void GetIndex_Should_ReturnListOfCurrentParts()
        {
            var parts = GetPartsFakerConfig().Generate(3);
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
        public async void Post_Should_CreatePartRecord()
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
        public async void Put_Should_UpdatePartRecord()
        {
            var savedPart = await CreatePartInDatabase();
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
        public async void Delete_Should_DeletePartRecord()
        {
            var savedPart = await CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.DeleteRequest<PartResponse>($"/api/part/?id={savedPart.ID}");

            result.Value.Should().BeNull();
            result.HasError.Should().BeFalse();
            var deletedPart = await _testFixture.DbContext.Parts.FindAsync(savedPart.ID);
            deletedPart.Should().BeNull();
        }

        private async Task<Entities.Part> CreatePartInDatabase()
        {
            var partEntity = GetPartsFakerConfig().Generate(1).First();
            _testFixture.DbContext.Parts.Add(partEntity);
            await _testFixture.DbContext.SaveChangesAsync();
            var savedPart = await _testFixture.DbContext.Parts.FindAsync(partEntity.ID);
            return savedPart;
        }

        private async Task<List<Entities.Part>> CreateListInDatabase(int howMany)
        {
            var parts = GetPartsFakerConfig().Generate(howMany);
            foreach (var part in parts)
            {
                _testFixture.DbContext.Parts.Add(part);
            }
            await _testFixture.DbContext.SaveChangesAsync();

            return parts;
        }

        private static Faker<Entities.Part> GetPartsFakerConfig()
        {
            return new Faker<Entities.Part>()
                        .RuleFor(p => p.ID, 0)
                        .RuleFor(p => p.Weight, f => f.Random.Number(0, 20))
                        .RuleFor(p => p.Price, f => f.Random.Number(0, 20))
                        .RuleFor(p => p.StartDate, f => f.Date.Between(new DateTime(2000, 1, 1), DateTime.Today));
        }
    }
}