using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace SpareParts.Client.Services.Authentication
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthTokenStore _authTokenStore;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient httpClient, IAuthTokenStore authTokenStore)
        {
            _httpClient = httpClient;
            _authTokenStore = authTokenStore;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = _authTokenStore.GetToken();
            if (string.IsNullOrWhiteSpace(token) || _authTokenStore.HasTokenExpired())
                return Task.FromResult(_anonymous);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(_authTokenStore.GetClaims(), "jwtAuthType"))));
        }

        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
