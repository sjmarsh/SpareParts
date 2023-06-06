using SpareParts.API.Entities;
using SpareParts.Client.Shared.Components.Filter;

namespace SpareParts.Browser.Tests.Pages
{
    public class SearchPage
    {
        public const string UrlPath = "part-search";

        private readonly IPage _page;
        private readonly string _baseUrl;
        private readonly NavBar _navBar;

        public SearchPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
            _navBar = new NavBar(_page);
        }

        public async Task InitializePage()
        {
            await _navBar.ClickHomeNav();
            await _navBar.ClickSearchNav();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Part Search");
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }

        public async Task<bool> IsFieldSelectionChipsVisible()
        {
            var fieldSelectionChips = await _page.QuerySelectorAllAsync("span >> text=Name");
            return fieldSelectionChips.Count() == 1;
        }

        public async Task<bool> IsFilterSelectorVisible()
        {
            var nameFilterFieldSelector = await _page.QuerySelectorAllAsync("select >> text=Name");
            return nameFilterFieldSelector.Count() == 1;
        }

        public async Task<bool> IsSearchResultsVisible()
        {
            var searchResultsTables = await _page.QuerySelectorAllAsync("table");
            return searchResultsTables != null && searchResultsTables.Count == 1;
        }

        public async Task<int> NumberOfSearchResults()
        {
            var seachResultTable = await _page.QuerySelectorAsync("table");
            var rows = await seachResultTable!.QuerySelectorAllAsync("tr");
            if (rows == null)
                return 0;
            return rows.Count -1;
        }

        public async Task<Part> GetSearchResultAtRow(int resultRowNumber)
        {
            var seachResultTable = await _page.QuerySelectorAsync("table");
            var rows = await seachResultTable!.QuerySelectorAllAsync("tr");
            var cells = await rows[resultRowNumber + 1].QuerySelectorAllAsync("td"); // allow for header row
                        
            (cells.Count == 6).Should().BeTrue();

            var endDateString = await cells[5].TextContentAsync();
            DateTime? endDate = string.IsNullOrEmpty(endDateString) ? null : Convert.ToDateTime(endDateString);
            return new Part
            {
                Name = await cells[0].TextContentAsync(),
                Description = await cells[1].TextContentAsync(),
                Price = Convert.ToDouble(await cells[2].TextContentAsync()),
                Weight = Convert.ToDouble(await cells[3].TextContentAsync()),
                StartDate = Convert.ToDateTime(await cells[4].TextContentAsync()),
                EndDate = endDate
            };
        }

        public async Task<List<string>> GetResultColumnHeadings()
        {
            var seachResultTable = await _page.QuerySelectorAsync("table");
            var rows = await seachResultTable!.QuerySelectorAllAsync("tr");
            var cells = await rows[0].QuerySelectorAllAsync("th");
            var headings = new List<string>();
            foreach (var cell in cells)
            {
                var heading = await cell.TextContentAsync();
                if(heading != null)
                {
                    headings.Add(heading);
                }
            }
            return headings;
        }

        public async Task EnterFilter(FilterLine filterLine, int row = 0)
        {
            var filterSelector = new FilterSelector(_page);
            await filterSelector.SelectField(filterLine.SelectedField.Name, row);
            await filterSelector.SelectOperator(filterLine.SelectedOperator, row);
            await filterSelector.EnterFilterValue(filterLine.Value, row);
        }

        public async Task ToggleFilterField(string fieldName)
        {
            var fieldChip = _page.Locator($"span >> text={fieldName}");
            var chipButton = fieldChip.Locator("a");
            await chipButton.ClickAsync();
            await WaitForSpinner();
        }

        public async Task AddFilter()
        {
            var addFilterButton = _page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { NameString = "Add Filter" });
            await addFilterButton.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }

        public async Task RemoveFilter(int row)
        {
            var filterSelector = new FilterSelector(_page);
            await filterSelector.RemoveFilter(row);
            await WaitForSpinner();
        }

        public async Task Search()
        {            
            var searchButton = _page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { NameString = "Search" });
            await searchButton.ClickAsync();
            await WaitForSpinner();
        }

        private async Task WaitForSpinner()
        {
            await _page.WaitForSelectorAsync(".spinner", new PageWaitForSelectorOptions { State = WaitForSelectorState.Detached });
        }

        public class FilterSelector
        {
            private readonly IPage _page;

            public FilterSelector(IPage page)
            {
                _page = page;
            }

            public async Task SelectField(string fieldName, int row = 0)
            {
                var filterFieldSelector = _page.Locator("#field").Nth(row);
                await filterFieldSelector.SelectOptionAsync(fieldName);
            }

            public async Task SelectOperator(string filterOperator, int row = 0)
            {
                var filterOperatorSelector = _page.Locator("#operator").Nth(row);
                await filterOperatorSelector.SelectOptionAsync(filterOperator);
            }

            public async Task EnterFilterValue(string filterValue, int row = 0)
            {
                await _page.Locator($"#value").Nth(row).FillAsync(filterValue);
            }

            public async Task RemoveFilter(int row = 0)
            {
                await _page.Locator("#remove").Nth(row).ClickAsync();
            }

        }
    }
}
