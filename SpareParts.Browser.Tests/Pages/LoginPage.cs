namespace SpareParts.Browser.Tests.Pages
{
    public class LoginPage
    {
        public const string UrlPath = "login";
        private readonly IPage _page;
        private string _baseUrl;

        public LoginPage(IPage page, string baseUrl)
        {
            _page = page;
            _baseUrl = baseUrl;
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

        public string CurrentUrl()
        {
            return _page.Url;
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }

        public async Task<bool> IsLoggedIn()
        {           
            var helloMessage = await _page.QuerySelectorAsync("p >> text=Hello");
            return helloMessage != null;
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
                await _page.WaitForNavigationAsync();
            }
        }

        public async Task Logout()
        {
            var logoutButton = _page.Locator("button >> text=Logout");
            await logoutButton.ClickAsync();
        }

        public async Task EnsureLoggedOut()
        {
            if (!_page.Url.Contains(UrlPath))
            {
                await GoToPage();
            }

            if (await IsLoggedIn())
            {
                await Logout();
            }
        }

        public async Task EnsureLoggedIn()
        {
            if (!_page.Url.Contains(UrlPath))
            {
                await GoToPage();
            }

            if(!await IsLoggedIn())
            {
                await Login();
            }
        }

        public async Task<string> AlertMessage()
        {
            var alert = await _page.WaitForSelectorAsync("div.alert");
            alert.Should().NotBeNull();
            return await alert!.InnerTextAsync();
        }
    }
}
