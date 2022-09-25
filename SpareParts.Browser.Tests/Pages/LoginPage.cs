namespace SpareParts.Browser.Tests.Pages
{
    public class LoginPage
    {
        public const string UrlPath = "login";
        private readonly IPage _page;
        private string _baseUrl;
        private readonly NavBar _navBar;

        public LoginPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
            _navBar = new NavBar(_page);
        }

        public async Task InitializePage()
        {
            await GoToPage();
        }

        public async Task GoToPage()
        {
            await _page.GotoAsync($"{_baseUrl}/{UrlPath}");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Login");
        }

        public async Task NavigateToPage()
        {
            await _navBar.ClickLoginNav();
        }

        public string CurrentUrl()
        {
            return _page.Url;
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }
                
        public async Task Login(bool waitForNav = true)
        {
            // TODO store test credentials in appsettings
            await Login("admin", "password", waitForNav);
        }

        public async Task Login(string userName, string password, bool waitForNav = true)
        {            
            await _page.Locator("#userName").FillAsync(userName);
            await _page.Locator("#password").FillAsync(password);

            var loginButton = await _page.QuerySelectorAsync("button >> text=Login");
            loginButton.Should().NotBeNull();
                        
            await loginButton!.ClickAsync();

            if (waitForNav)
            {
                await _page.WaitForNavigationAsync(new PageWaitForNavigationOptions { WaitUntil = WaitUntilState.NetworkIdle });
                //await _page.WaitForSelectorAsync("h1 >> Spare Parts");
            }
        }

        public async Task<bool> IsLoggedIn()
        {
            return (await _navBar.GetNavItemTitles()).Contains("Logout");
        }

        public async Task Logout()
        {
            await _navBar.ClickLogoutNav();
        }

        public async Task GoToLogoutPage()
        {
            // note. navigating directly to this page just simply logs you out there is no page content to speak of.
            await _page.GotoAsync($"{_baseUrl}/logout");
        }

        public async Task<bool> IsLoggedOut()
        {
            return (await _navBar.GetNavItemTitles()).Contains("Login");
        }


        public async Task EnsureLoggedOut()
        {
            if(await IsLoggedIn())
            {
                await Logout();
            }
            else
            {
                await GoToLogoutPage();
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            }
            await _page.WaitForTimeoutAsync(1000);
            (await _navBar.GetNavItemTitles()).Should().Contain("Login");
        }

        public async Task EnsureLoggedIn()
        {
            if(await IsLoggedOut())
            {
                await NavigateToPage();
                await Login();
            }
            await _page.WaitForTimeoutAsync(1000);
            (await _navBar.GetNavItemTitles()).Should().Contain("Logout");
        }

        public async Task<string> AlertMessage()
        {
            var alert = await _page.WaitForSelectorAsync("div.alert");
            alert.Should().NotBeNull();
            return await alert!.InnerTextAsync();
        }
    }
}
