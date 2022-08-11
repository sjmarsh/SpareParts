using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class PartsTests : IAsyncLifetime
    {
        private readonly PartsPage _partsPage;
        private readonly SparePartsDbContext _dbContext;

        public PartsTests(SparePartsBrowserTestFixture fixture)
        {
            _partsPage = fixture.Pages.Parts;
            _dbContext = fixture.DbContext;

            // clear parts table between tests
            _dbContext.Parts.RemoveRange(_dbContext.Parts);
            _dbContext.SaveChanges();
        }

        public async Task InitializeAsync() // runs before each test
        {
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
            var itemCount = await _partsPage.PartListItemCount();

            itemCount.Should().Be(0); 
        }

        [Fact]
        public async Task AddPart_Should_AddPartToList()
        {
            (await _partsPage.PartListItemCount(false)).Should().Be(0);
            var part = new Shared.Models.Part { Name = "Part 1", Description = "The first one", Weight = 2.2, Price = 3.33, StartDate = DateTime.Today.AddYears(-2), EndDate = DateTime.Today.AddYears(2) };
            await _partsPage.ClickAddButton();
            
            await EnterPart(part);

            (await _partsPage.PartListItemCount()).Should().Be(1);
        }

        [Fact]
        public async Task EditPart_Should_UpdatePartInList()
        {
            var part = new Shared.Models.Part { Name = "Part 1", Description = "The first one", Weight = 2.2, Price = 3.33, StartDate = DateTime.Today.AddYears(-2), EndDate = DateTime.Today.AddYears(2) };
            await _partsPage.ClickAddButton();
            await EnterPart(part);
            (await _partsPage.PartListItemCount()).Should().Be(1);
            
            await _partsPage.ClickEditButtonForRow(0);
            var updatedPart = new Shared.Models.Part { Name = "Part 1", Description = "This is one part", Weight = 1.2, Price = 1.33, StartDate = DateTime.Today.AddYears(-3), EndDate = DateTime.Today.AddYears(3) };

            await EnterPart(updatedPart);

            (await _partsPage.PartListItemCount()).Should().Be(1);
            var rowPart = _partsPage.GetPartFromRow(0);
            rowPart.Should().NotBeNull();
            rowPart.Should().BeEquivalentTo(updatedPart);
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
