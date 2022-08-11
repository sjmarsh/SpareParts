namespace SpareParts.Browser.Tests.Pages
{
    public class InventoryPage
    {
        public const string UrlPath = "inventory-home";
        private readonly IPage _page;
        private string _baseUrl;

        public InventoryPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
        }

        public async Task InitializePage()
        {
            await _page.GotoAsync($"{_baseUrl}/{UrlPath}");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Inventory");
        }

        public ManualStockEntryTab ManualStockEntry => new(_page);

        public StocktakeTab Stocktake => new(_page);

        public InventoryTable CurrentStock => new(_page);

        public InventoryTable History => new(_page);

        public async Task<string> PageHeader()
        {
            var header = _page.Locator("h3");
            return await header.InnerTextAsync();
        }

        public async Task<int> SelectedTabIndex()
        {
            var allTabs = await _page.QuerySelectorAllAsync(".nav-tabs > .nav-item > .nav-link");
            IElementHandle? selectedTab = null;
            foreach(var tab in allTabs)
            {
                var isSelected = await tab.EvaluateAsync<bool>("t => t.classList.contains('is-selected')");
                if (isSelected)
                {
                    selectedTab = tab;
                }
            }
            selectedTab.Should().NotBeNull();

            return allTabs.ToList().IndexOf(selectedTab!);
        }

        public async Task SelectTabNumber(int tabIndex)
        {
            var allTabs = await _page.QuerySelectorAllAsync(".nav-tabs > .nav-item > .nav-link");
            allTabs.Count().Should().BeGreaterThan(tabIndex);
            await allTabs[tabIndex].ClickAsync();
        }

        
    }

    public class ManualStockEntryTab
    {
        private readonly IPage _page;

        public ManualStockEntryTab(IPage page)
        {
            _page = page;
        }
        public async Task SelectPart(string partName)
        {
            await _page.WaitForSelectorAsync("#selectPart");
            await _page.Locator("#selectPart").SelectOptionAsync(new SelectOptionValue { Label = partName });
        }

        public async Task EnterQuantity(int quantity)
        {
            await _page.Locator($"#partQty").FillAsync(quantity.ToString());
        }

        public async Task SaveManualStock()
        {
            await _page.Locator("text=Submit").ClickAsync();
            await _page.WaitForResponseAsync(r => r.Status == 200);
        }
    }

    public class StocktakeTab
    {
        private readonly IPage _page;

        public StocktakeTab(IPage page)
        {
            _page = page;
        }

        public async Task EnterQuantityFor(string partName, int quantity)
        {
            var row = _page.Locator($"tr:has-text('{partName}')");
            var qtyInput = row.Locator("input");
            await qtyInput.FillAsync(quantity.ToString());
        }

        public async Task SubmitStocktake()
        {
            await _page.Locator("text=Submit").ClickAsync();
            await _page.WaitForResponseAsync(r => r.Status == 200);
        }
    }

    public class InventoryTable
    {
        private readonly IPage _page;

        public InventoryTable(IPage page)
        {
            _page = page;
        }

        public async Task<Shared.Models.InventoryItemDetail> GetItemForRow(int row)
        {
            var tableRow = _page.Locator("tr").Nth(row + 1);  // zero is header
            var cells = tableRow.Locator("td");

            return new Shared.Models.InventoryItemDetail
            {
                PartName = await cells.Nth(0).InnerTextAsync(),
                Quantity = Convert.ToInt32(await cells.Nth(1).InnerTextAsync()),
                DateRecorded = Convert.ToDateTime(await cells.Nth(2).InnerTextAsync())
            };
        }
    }
}
