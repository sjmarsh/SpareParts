using SpareParts.API.Data;
using SpareParts.API.Entities;
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
            var partToFilterOn = parts[1];

            var filter = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.Equal,
                Value = partToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();
            (await _searchPage.NumberOfSearchResults()).Should().Be(1);
            var searchResult = await _searchPage.GetSearchResultAtRow(0);
            AssertExpectedSearchResult(partToFilterOn, searchResult);
        }
        
        [Fact]
        public async Task Should_FindPartsWithMultipleFilters()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(5);
            var partsOrderdByWeight = parts.OrderBy(p => p.Weight).ToArray();
                        
            var part1ToFilterOn = partsOrderdByWeight[0];
            var part2ToFilterOn = partsOrderdByWeight[1];

            var filter1 = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.GreatherThanOrEqual,
                Value = part1ToFilterOn.Weight.ToString()
            };

            var filter2 = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.LessThanOrEqual,
                Value = part2ToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter1, 0);
            await _searchPage.AddFilter();
            await _searchPage.EnterFilter(filter2, 1);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();
            (await _searchPage.NumberOfSearchResults()).Should().Be(2);

            var searchResult1 = await _searchPage.GetSearchResultAtRow(0);
            var searchResult2 = await _searchPage.GetSearchResultAtRow(1);
            var results = new[] { searchResult1, searchResult2 };
            var orderedResults = results.OrderBy(r => r.Weight).ToArray();

            AssertExpectedSearchResult(part1ToFilterOn, orderedResults[0]);            
            AssertExpectedSearchResult(part2ToFilterOn, orderedResults[1]);
        }

        private static void AssertExpectedSearchResult(Part partToFilterOn, Part searchResult)
        {
            searchResult.Name.Should().Be(partToFilterOn.Name);
            searchResult.Description.Should().Be(partToFilterOn.Description);
            searchResult.Price.Should().Be(partToFilterOn.Price);
            searchResult.Weight.Should().Be(partToFilterOn.Weight);
            searchResult.StartDate.Should().BeCloseTo(partToFilterOn.StartDate, TimeSpan.FromSeconds(5));
            searchResult.EndDate.Should().Be(partToFilterOn.EndDate);
        }

        [Fact]
        public async Task Should_UpdateResultsWhenFitlerIsRemoved()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(5);
            var partsOrderdByWeight = parts.OrderBy(p => p.Weight).ToArray();

            var part1ToFilterOn = partsOrderdByWeight[0];
            var part2ToFilterOn = partsOrderdByWeight[1];

            var filter1 = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.GreatherThanOrEqual,
                Value = part1ToFilterOn.Weight.ToString()
            };

            var filter2 = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.LessThanOrEqual,
                Value = part2ToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter1, 0);
            await _searchPage.AddFilter();
            await _searchPage.EnterFilter(filter2, 1);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();
            (await _searchPage.NumberOfSearchResults()).Should().Be(2);

            await _searchPage.RemoveFilter(1);
            (await _searchPage.NumberOfSearchResults()).Should().BeGreaterThan(2);
        }

        [Fact]
        public async Task Should_NotDisplayResultsWhenAllFiltersAreRemoved()
        {
            var parts = await _dataHelper.CreatePartListInDatabase(3);
            var partToFilterOn = parts[1];

            var filter = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.Equal,
                Value = partToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();
           
            await _searchPage.RemoveFilter(0);
            (await _searchPage.IsSearchResultsVisible()).Should().BeFalse();
        }

        [Fact]
        public async Task Should_UpdateResultsWhenFieldToggled()
        {
            const string FieldToToggle = "Description";
            var parts = await _dataHelper.CreatePartListInDatabase(3);
            var partToFilterOn = parts[1];

            var filter = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.Equal,
                Value = partToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();

            var headings = await(_searchPage.GetResultColumnHeadings());
            headings.Should().HaveCount(6);
            headings.Should().Contain(FieldToToggle);

            await _searchPage.ToggleFilterField(FieldToToggle);
            headings = await (_searchPage.GetResultColumnHeadings());
            headings.Should().HaveCount(5);
            headings.Should().NotContain(FieldToToggle);

            await _searchPage.ToggleFilterField(FieldToToggle);
            headings = await (_searchPage.GetResultColumnHeadings());
            headings.Should().HaveCount(6);
            headings.Should().Contain(FieldToToggle);
        }

        [Fact]
        public async Task Should_NotUpdateResultWhenFieldToggledIsAlreadyUsedInAFilter()
        {
            const string FieldToToggle = "Weight";
            var parts = await _dataHelper.CreatePartListInDatabase(3);
            var partToFilterOn = parts[1];

            var filter = new FilterLine
            {
                SelectedField = new FilterField("Weight", typeof(double), true),
                SelectedOperator = FilterOperator.Equal,
                Value = partToFilterOn.Weight.ToString()
            };

            await _searchPage.EnterFilter(filter);
            await _searchPage.Search();

            (await _searchPage.IsSearchResultsVisible()).Should().BeTrue();

            var headings = await (_searchPage.GetResultColumnHeadings());
            headings.Should().HaveCount(6);
            headings.Should().Contain(FieldToToggle);

            await _searchPage.ToggleFilterField(FieldToToggle);
            headings = await (_searchPage.GetResultColumnHeadings());
            headings.Should().HaveCount(6);
        }
    }
}
