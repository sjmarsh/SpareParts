namespace SpareParts.Browser.Tests.Pages
{
    public class NavBar
    {
        private readonly IPage _page;
        
        public NavBar(IPage page)
        {
            _page = page;
        }
               
        public async Task<string> GetPageTitle()
        {
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            return await _page.TitleAsync();
        }

        public string GetCurrentUrl()
        {
            return _page.Url;
        }

        public async Task<List<string>> GetNavItemTitles()
        {
            await _page.WaitForSelectorAsync(".nav-link");
            var links = await _page.QuerySelectorAllAsync(".nav-link");
            links.Should().NotBeNullOrEmpty();
            var titles = new List<string>();
            foreach (var link in links) 
            { 
                titles.Add(await link.InnerTextAsync()); 
            }
            return titles;
        }

        public async Task ClickHomeNav()
        {
            var partsNav = _page.Locator(".nav-link >> text=Home");
            await partsNav.ClickAsync();
        }

        public async Task ClickPartsNav()
        {
            var partsNav = _page.Locator(".nav-link >> text=Parts");
            await partsNav.ClickAsync();
        }

        public async Task ClickInventoryNav()
        {
            var partsNav = _page.Locator(".nav-link >> text=Inventory");
            await partsNav.ClickAsync();
        }        
    }
}
