using Microsoft.AspNetCore.Mvc;
using SpareParts.API.Models;
using SpareParts.API.Services;
using SpareParts.Shared.Constants;
using SpareParts.Shared.Models;

namespace SpareParts.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticatinService;
        private CookieOptions _cookieOptions;
        
        public UserController(IAuthenticationService authenticatinService)
        {
            _authenticatinService = authenticatinService;
            _cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };
        }

        [HttpPost("authenticate")]
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest authenticationRequest)
        {
            var authenticationDetails = await _authenticatinService.Authenticate(authenticationRequest);
            return BuildAuthenticationResponse(authenticationDetails);
        }

        [HttpPost("refresh")]
        public async Task<AuthenticationResponse> Refresh()
        {
            var accessToken = Request.Headers.Authorization.FirstOrDefault();
            var refreshToken = Request.Cookies[AuthToken.RefreshTokenName];
            var refreshRequest = new RefreshRequest(accessToken, refreshToken);
            
            var authenticationDetails = await _authenticatinService.Refresh(refreshRequest);
            return BuildAuthenticationResponse(authenticationDetails);
        }

        private AuthenticationResponse BuildAuthenticationResponse(AuthenticationDetails authenticationDetails)
        {
            if (authenticationDetails.AuthenticationResponse.IsAuthenticated && authenticationDetails.RefreshToken != null)
            {
                Response.Cookies.Append(AuthToken.RefreshTokenName, authenticationDetails.RefreshToken, _cookieOptions);
            }

            return authenticationDetails.AuthenticationResponse;
        }

        [HttpGet("setup")]
        public Task<bool> SetupUsersAndRoles()
        {
            return _authenticatinService.SetupUsersAndRoles();
        }
    }
}
