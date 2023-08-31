using Microsoft.EntityFrameworkCore;
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

            // clear parts tables between tests
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
        public async Task Get_Should_ReturnUnauthorizedWhenNoTokenHeader()
        {
            _testFixture.HttpClient.DefaultRequestHeaders.Remove("Authorization");

            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.Get($"/api/part/?id={savedPart?.ID}");

            result.Should().NotBeNull();
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Should_ReturnRequiredPartRecord()
        {
            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);

            var result = await _testFixture.GetRequest<PartResponse>($"/api/part/?id={savedPart!.ID}");
            
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
            result.Items.Should().BeEquivalentTo(parts, opt => opt.Excluding(p => p.Attributes));
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
            result?.Items?[0].Should().BeEquivalentTo(parts[0], opt => opt.Excluding(p => p.Attributes));
        }


        [Fact]
        public async Task Post_Should_CreatePartRecord()
        {
            var part = new Part { Name = "Part 1", Description = "One", Weight = 1.1, Price = 2.2, StartDate = DateTime.Today.AddYears(-1), 
                Attributes = new List<PartAttribute> { new PartAttribute { Name = "Colour", Description = "The colour of the part", Value = "Green" }}
            };
                        
            var result = await _testFixture.PostRequest<Part, PartResponse>("/api/part", part);
            
            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();
            
            var partId = result?.Value?.ID;
            Assert.True(partId > 0);
            var savedPart = await _testFixture.DbContext.Parts.Include(p => p.Attributes).FirstOrDefaultAsync(p => p.ID == partId);
            
            savedPart.Should().NotBeNull();
            savedPart.Should().BeEquivalentTo(part, opt => opt.Excluding(p => p.ID).Excluding(p => p.Attributes));
            savedPart!.Attributes.Should().BeEquivalentTo(part.Attributes, opt => opt.Excluding(a => a.ID));
        }

        [Fact]
        public async Task Put_Should_UpdatePartRecord()
        {
            const int attributeCount = 2;
            var savedPart = await _dataHelper.CreatePartInDatabase(attributeCount);
            savedPart!.ID.Should().BeGreaterThan(0);
            savedPart!.Attributes.Should().HaveCount(attributeCount);
            var firstAttribute = savedPart!.Attributes![0];
            var secondAttribute = savedPart!.Attributes![1];
            var partModel = new Part { ID = savedPart!.ID, Name = "Other Name", Description = "Other Descritpion", Weight = savedPart.Weight, Price = savedPart!.Price!.Value, StartDate = savedPart.StartDate,
                Attributes = new List<PartAttribute> { 
                    new PartAttribute { ID = firstAttribute.ID, Name = "Other attribute name", Description = firstAttribute.Description, Value = firstAttribute.Value },
                    // don't include existing 2nd attribute with the udpate to ensure it is deleted
                    // add a new attribute to ensure it gets created
                    new PartAttribute { ID = 0, Name = "A new attribute with this update", Value = "NewValue" }
                }
            };
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.PutRequest<Part, PartResponse>("/api/part", partModel);

            result.Should().NotBeNull();
            result.Value.Should().NotBeNull();

            var partId = result?.Value?.ID;
            Assert.True(partId > 0);
            var updatedPart = await _testFixture.DbContext.Parts.Include(p => p.Attributes).FirstOrDefaultAsync(p => p.ID == savedPart.ID);

            updatedPart.Should().NotBeNull();
            updatedPart.Should().BeEquivalentTo(partModel, opt => opt.Excluding(p => p.Attributes));
            updatedPart!.Attributes.Should().HaveCount(2);
            updatedPart!.Attributes!.Single(a => a.ID == firstAttribute.ID).Should().BeEquivalentTo(partModel.Attributes.Single(a => a.ID == firstAttribute.ID));
            updatedPart!.Attributes!.Single(a => a.ID != firstAttribute.ID).Should().BeEquivalentTo(partModel.Attributes.Single(a => a.ID != firstAttribute.ID), opt => opt.Excluding(a => a.ID));
            var deletedAttribute = await _testFixture.DbContext.PartAttribute.FindAsync(secondAttribute.ID);
            deletedAttribute.Should().BeNull();
        }

        [Fact]
        public async Task Delete_Should_DeletePartRecords()
        {
            var savedPart = await _dataHelper.CreatePartInDatabase();
            savedPart?.ID.Should().BeGreaterThan(0);
            savedPart!.Attributes.Should().NotBeEmpty();
            var firstAttribute = savedPart!.Attributes![0];
            _testFixture.DbContext.ChangeTracker.Clear();

            var result = await _testFixture.DeleteRequest<PartResponse>($"/api/part/?id={savedPart!.ID}");

            result.Value.Should().BeNull();
            result.HasError.Should().BeFalse();
            var deletedPart = await _testFixture.DbContext.Parts.FindAsync(savedPart.ID);
            deletedPart.Should().BeNull();
            var deletedAttribute = await _testFixture.DbContext.PartAttribute.FindAsync(firstAttribute.ID);
            deletedAttribute.Should().BeNull();  
        }        
    }
}