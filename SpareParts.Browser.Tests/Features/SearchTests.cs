using SpareParts.API.Data;
using SpareParts.Browser.Tests.Pages;
using SpareParts.Client.Shared.Components.Filter;
using SpareParts.Test.Helpers;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class SearchTests : IAsyncLifetime
    {
        private readonly SearchPage _searchPage;

        private readonly SparePartsDbContext _dbContext;
        private readonly DataHelper _dataHelper;

        public SearchTests(SparePartsBrowserTestFixture fixture)
        {
            _searchPage = fixture.Pages.Search;

            _dbContext = fixture.DbContext;
            _dataHelper = new DataHelper(_dbContext);
        }

        public async Task InitializeAsync()
        {
            // clear parts table between tests
            _dbContext.InventoryItems.RemoveRange(_dbContext.InventoryItems);
            await _dbContext.SaveChangesAsync();
            _dbContext.Parts.RemoveRange(_dbContext.Parts);
            await _dbContext.SaveChangesAsync();

            await _searchPage.InitializePage();
        }

        public async Task DisposeAsync()
        {
            await Task.Run(() =>
            {
            });
        }

        [Fact]
        public async Task Should_HaveExpectedHeader()
        {
            var header = await _searchPage.PageHeader();

            header.Should().Be("Part Search");
        }

        [Fact]
        public async Task Should_HaveFieldSelectionChipsAvailable()
        {
            (await _searchPage.IsFieldSelectionChipsVisible()).Should().BeTrue();
        }

        [Fact]
        public async Task Should_HaveEmptyFilterReadyToSearch()
        {
            (await _searchPage.IsFilterSelectorVisible()).Should().BeTrue();
        }

        [Fact]
        public async Task Should_DefaultToNoSearchResults()
        {
            (await _searchPage.IsSearchResultsVisible()).Should().BeFalse();
        }

        [Fact]
        public async Task Should_FindPartWithCorrectFilter()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(3);
            
            var filter = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.Equal,
                Value = parts[1].Weight.ToString()
            };

            await _searchPage.EnterFilter(filter);
            await _searchPage.Search();

            // TODO check search results as expected.

        }
    }
}
