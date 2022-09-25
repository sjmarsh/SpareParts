using SpareParts.Browser.Tests.Pages;

namespace SpareParts.Browser.Tests.Features
{
    [Collection("Browser Tests")]
    public class LoginTests : IAsyncLifetime
    {
        private readonly LoginPage _loginPage;
        private readonly string _baseUrl;

        public LoginTests(SparePartsBrowserTestFixture fixture)
        {
            _loginPage = fixture.Pages.Login;
            _baseUrl = fixture.BaseUrl;
        }

        public async Task InitializeAsync()
        {
            await _loginPage.InitializePage();
        }

        public async Task DisposeAsync()
        {
            await _loginPage.EnsureLoggedIn();
        }
        
        [Fact]
        public async Task Should_Login_and_Logout()
        {
            await _loginPage.EnsureLoggedOut();
            await _loginPage.NavigateToPage();

            await _loginPage.Login();

            _loginPage.CurrentUrl().Should().Be($"{_baseUrl}/");  // Home
            (await _loginPage.IsLoggedIn()).Should().BeTrue();

            await _loginPage.Logout();

            _loginPage.CurrentUrl().Should().Be($"{_baseUrl}/"); // Home
            await _loginPage.NavigateToPage();
            (await _loginPage.IsLoggedIn()).Should().BeFalse();
        }

        [Fact]
        public async Task Should_DisplayMessage_WhenInvalidCredentials()
        {
            await _loginPage.EnsureLoggedOut();
            await _loginPage.GoToPage();

            await _loginPage.Login("NotAUser", "NotAPassword", false);

            (await _loginPage.AlertMessage()).Should().Be("Invalid login details.");
            await _loginPage.EnsureLoggedOut();
        }
    }
}
