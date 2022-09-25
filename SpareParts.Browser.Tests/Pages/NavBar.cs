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
            await _page.WaitForSelectorAsync(".sidebar");
            var sideBar = await _page.QuerySelectorAsync(".sidebar");
            if(sideBar == null)
            {
                throw new Exception("Can't find Nav Bar on page.");
            }
            var links = await sideBar.QuerySelectorAllAsync(".nav-link");
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
            var homeNav = _page.Locator(".nav-link >> text=Home");
            await homeNav.ClickAsync();
        }

        public async Task ClickLoginNav()
        {
            var loginNav = _page.Locator(".nav-link >> text=Login");
            await loginNav.ClickAsync();
        }

        public async Task ClickLogoutNav()
        {
            var logoutNav = _page.Locator(".nav-link >> text=Logout");
            await logoutNav.ClickAsync();
        }

        public async Task ClickPartsNav()
        {
            var partsNav = _page.Locator(".nav-link >> text=Parts");
            await partsNav.ClickAsync();
        }

        public async Task ClickInventoryNav()
        {
            var inventoryNav = _page.Locator(".nav-link >> text=Inventory");
            await inventoryNav.ClickAsync();
        }        
    }
}
