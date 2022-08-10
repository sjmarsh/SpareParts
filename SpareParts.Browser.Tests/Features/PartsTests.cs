using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class PartsTests
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
            var part = new Shared.Models.Part { Name = "Part 1", Description = "The first one", Weight = 2.2, Price = 3.33, StartDate = DateTime.Today.AddYears(-2), EndDate = DateTime.Today.AddYears(2) };
            await _partsPage.ClickAddButton();
            await EnterPart(part);
            var hasParts = await _partsPage.PartListHasItems();
            hasParts.Should().BeTrue();
            var itemCount = await _partsPage.PartListItemCount();
            itemCount.Should().Be(1);
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
