using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            await _page.GotoAsync($"{_baseUrl}/{UrlPath}");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForSelectorAsync("h3 >> text=Login");
        }

        public async Task<string> PageHeader()
        {
            var h3 = _page.Locator("h3");
            return await h3.InnerTextAsync();
        }
    }
}
