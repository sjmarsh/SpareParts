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
            var searchResultsTable = await _page.QuerySelectorAllAsync("table");
            return searchResultsTable.Count() == 1;
        }

        public async Task EnterFilter(FilterLine filterLine, int row = 0)
        {
            var filterSelector = new FilterSelector(_page);
            await filterSelector.SelectField(filterLine.SelectedField.Name, row);
            await filterSelector.SelectOperator(filterLine.SelectedOperator, row);
            await filterSelector.EnterFilterValue(filterLine.Value, row);
        }

        public async Task Search()
        {            
            var searchButton = _page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { NameString = "Search" });
            await searchButton.ClickAsync();
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

        }
    }
}
