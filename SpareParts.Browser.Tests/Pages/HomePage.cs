using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpareParts.Browser.Tests.Pages
{
    public class HomePage
    {
        private readonly IPage _page;
        private NavBar _navBar;

        public HomePage(IPage page)
        {
            _page = page;
            _navBar = new NavBar(_page);
        }

        public async Task NavigateToPage()
        {
            await _navBar.ClickHomeNav();
        }
    }
}
