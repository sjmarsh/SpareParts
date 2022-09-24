﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using SpareParts.Shared.Models;
using SpareParts.Shared.Constants;

namespace SpareParts.Client.Services.Authentication
{
    //ref: https://code-maze.com/blazor-webassembly-authentication-aspnetcore-identity/
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task Logout();
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(IUserService userService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorage)
        {
            _userService = userService;
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            var result = await _userService.Authenticate(request);
            
            if (result.IsAuthenticated && result.AccessToken != null)
            {
                await _localStorage.SetItemAsync(AuthToken.AccessTokenName, result.AccessToken);
                ((AuthStateProvider)_authenticationStateProvider).NotifyUserAuthentication(result.AccessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            }

            return result;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(AuthToken.AccessTokenName);
            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
