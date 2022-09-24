using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task<AuthenticationResponse> Refresh();
        Task<AuthenticationResponse> RefreshIfRequired();
        void Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IAuthTokenStore _authTokenStore;

        public AuthenticationService(IUserService userService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, IAuthTokenStore authTokenStore)
        {
            _userService = userService;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _authTokenStore = authTokenStore;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var result = await _userService.Authenticate(request);
            SetAuthenticationState(result);
            return result;
        }
        
        public async Task<AuthenticationResponse> RefreshIfRequired()
        {
            if (_authTokenStore.HasTokenExpired())
            {
                return await Refresh();
            }

            return new AuthenticationResponse(true, "Token has not expired. Refresh not required.");
        }

        public async Task<AuthenticationResponse> Refresh()
        {
            var result = await _userService.Refresh();
            SetAuthenticationState(result);
            if (!result.IsAuthenticated)
            {
                Logout();
            }
            return result;
        }

        public void Logout()
        {
            _authTokenStore.ClearToken();
            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private void SetAuthenticationState(AuthenticationResponse result)
        {
            if (result.IsAuthenticated && result.AccessToken != null)
            {
                _authTokenStore.SetToken(result.AccessToken);
                ((AuthStateProvider)_authenticationStateProvider).NotifyUserAuthentication(result.AccessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            }
        }
    }
}
