using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;
using SpareParts.Test.Helpers;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class PartsTests : IAsyncLifetime
    {
        private readonly PartsPage _partsPage;
        
        private readonly SparePartsDbContext _dbContext;
        private readonly DataHelper _dataHelper;

        public PartsTests(SparePartsBrowserTestFixture fixture)
        {
            _partsPage = fixture.Pages.Parts;
            
            _dbContext = fixture.DbContext;
            _dataHelper = new DataHelper(_dbContext);
        }

        public async Task InitializeAsync() // runs before each test
        {
            // clear parts table between tests
            _dbContext.InventoryItems.RemoveRange(_dbContext.InventoryItems);
            await _dbContext.SaveChangesAsync();
            _dbContext.Parts.RemoveRange(_dbContext.Parts);
            await _dbContext.SaveChangesAsync();
            
            await _partsPage.InitializePage();
        }

        public async Task DisposeAsync()
        {
        }

        [Fact]
        public async Task Should_HaveExpectedHeader()
        {
            var header = await _partsPage.PageHeader();

            header.Should().Be("Part List");
        }

        [Fact]
        public async Task Should_HaveEmptyPartList()
        {
            (await _partsPage.IsPartTableVisible()).Should().BeFalse();
        }

        [Fact]
        public async Task AddPart_Should_AddPartToList()
        {
            (await _partsPage.IsPartTableVisible()).Should().BeFalse();
            var part = new Shared.Models.Part { Name = "Part 1", Description = "The first one", Weight = 2.2, Price = 3.33, StartDate = DateTime.Today.AddYears(-2), EndDate = DateTime.Today.AddYears(2) };
            await _partsPage.ClickAddButton();
            
            await EnterPart(part);
                        
            (await _partsPage.PartListItemCount()).Should().Be(1);
        }

        [Fact]
        public async Task EditPart_Should_UpdatePartInList()
        {
            await _dataHelper.CreatePartInDatabase();
            await _partsPage.InitializePage();
            (await _partsPage.IsPartTableVisible()).Should().BeTrue();
            (await _partsPage.PartListItemCount()).Should().Be(1);
            
            await _partsPage.ClickEditButtonForRow(0);
            var updatedPart = new Shared.Models.Part { Name = "Part 1", Description = "This is one part", Weight = 1.2, Price = 1.33, StartDate = DateTime.Today.AddYears(-3), EndDate = DateTime.Today.AddYears(3) };
            await EnterPart(updatedPart);

            (await _partsPage.IsPartTableVisible()).Should().BeTrue();
            (await _partsPage.PartListItemCount()).Should().Be(1);
            var rowPart = await _partsPage.GetPartFromRow(0); 
            rowPart.Should().NotBeNull();
            rowPart.Should().BeEquivalentTo(updatedPart);
        }

        [Fact]
        public async Task DeletePart_Should_RemovePartFromList()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(2);
            await _partsPage.InitializePage();
            (await _partsPage.IsPartTableVisible()).Should().BeTrue();
            (await _partsPage.PartListItemCount()).Should().Be(2);

            await _partsPage.ClickDeleteButtonForRow(0);  // delete part1

            (await _partsPage.IsPartTableVisible()).Should().BeTrue();
            (await _partsPage.PartListItemCount()).Should().Be(1);
            var rowPart = await _partsPage.GetPartFromRow(0);  
            rowPart.Should().NotBeNull();
            rowPart.Should().BeEquivalentTo(parts[1], opt => opt
                    .Excluding(p => p.ID)
                    .Using<DateTime>(ctx => ctx.Subject.Date.Should().Be(ctx.Expectation.Date))
                    .WhenTypeIs<DateTime>());
        }

        private async Task EnterPart(Shared.Models.Part part)
        {
            var partModal = await _partsPage.GetPartModal();
            await partModal.EnterName(part.Name);
            await partModal.EnterDescription(part.Description);
            await partModal.EnterWeight(part.Weight);
            await partModal.EnterPrice(part.Price);
            await partModal.EnterStartDate(part.StartDate);
            await partModal.EnterEndDate(part.EndDate);

            await partModal.Submit();
            await partModal.Close();
        }

    }
}
