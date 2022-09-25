using SpareParts.Browser.Tests.Pages;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class NavigationTests : IAsyncLifetime
    {        
        private readonly NavBar _navBar;
        private readonly LoginPage _loginPage;
        private readonly SparePartsBrowserTestFixture _fixture;

        public NavigationTests(SparePartsBrowserTestFixture fixture)
        {
            _navBar = fixture.Pages.NavBar;
            _loginPage = fixture.Pages.Login;
            _fixture = fixture;
        }

        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
            });
        }

        public async Task DisposeAsync()
        {
            await _loginPage.EnsureLoggedIn();
        }

        [Fact]
        public async Task Should_HaveExpectedPageTitle()
        {
            var title = await _navBar.GetPageTitle();
            title.Should().Be("Spare Parts");
        }

        [Fact]
        public async Task Should_HaveRequiredNavItems()
        {
            var navItemTitles = await _navBar.GetNavItemTitles();
            navItemTitles.Should().HaveCount(4);
            var expectedTitles = new[] { "Home", "Logout", "Parts", "Inventory" };
            expectedTitles.Should().BeEquivalentTo(navItemTitles);
        }

        [Fact]
        public async Task Should_HideNavItemsWhenNotLoggedIn()
        {
            await _loginPage.EnsureLoggedOut();
            var navItemTitles = await _navBar.GetNavItemTitles();
            navItemTitles.Should().HaveCount(2);
            var expectedTitles = new[] { "Home", "Login" };
            expectedTitles.Should().BeEquivalentTo(navItemTitles);
        }

        [Fact]
        public async Task SelectPartsNav_Should_NavigateToParts()
        {
            await _navBar.ClickPartsNav();

            _navBar.GetCurrentUrl().Should().Contain(PartsPage.UrlPath);
        }

        [Fact]
        public async Task SelectInventoryNav_Should_NavigateToInventory()
        {
            await _navBar.ClickInventoryNav();

            _navBar.GetCurrentUrl().Should().Contain("inventory");
        }

        [Fact]
        public async Task SelectHomeNav_Should_NavigateToHome()
        {
            await _navBar.ClickHomeNav();

            _navBar.GetCurrentUrl().Should().Be(_fixture.BaseUrl + "/");
        }
    }
}