using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SpareParts.API.Infrastructure;
using SpareParts.Shared.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using SpareParts.API.Entities;
using SpareParts.API.Models;

namespace SpareParts.API.Services
{
    public interface IAuthenticationService
    {        
        Task<AuthenticationDetails> Authenticate(AuthenticationRequest request);
        Task<AuthenticationDetails> Refresh(RefreshRequest refreshRequest);
        Task<bool> SetupUsersAndRoles();
    }

    public class AuthenticationService : IAuthenticationService
    {        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwtSettings, ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthenticationDetails> Authenticate(AuthenticationRequest request)
        {
            if (request.UserName == null || request.Password == null)
            {
                // This should be handled by validation but just in case...
                _logger.LogWarning("Attempted to Authenticate with incomplete credentials.");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Incomplete credentials."));
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
            if(user == null)
            {
                _logger.LogWarning($"User: {request.UserName} is not a valid user.");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Invalid credentials"));
            }
            else
            {
                var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isValidPassword)
                {
                    _logger.LogWarning($"Password is invalid for User: {request.UserName}.");
                    return new AuthenticationDetails(new AuthenticationResponse(false, "Invalid credentials"));
                }

                return await BuildSuccessfulAuthenticationDetails(user);
            }
        }
        
        public async Task<AuthenticationDetails> Refresh(RefreshRequest refreshRequest)
        {
            if(refreshRequest == null || refreshRequest.AccessToken == null || refreshRequest.RefreshToken == null)
            {
                _logger.LogWarning("Attempted to Refresh with missing tokens.");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Missing tokens."));
            }

            var principal = GetPrincipalFromExpiredToken(refreshRequest.AccessToken);
            
            if(principal == null)
            {
                _logger.LogWarning("Attempted to Refresh with invalid accesss token.");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Invalid token."));
            }
            
            var userName = principal?.Identity?.Name;
            if(userName == null)
            {
                _logger.LogError("Unable to extract user name from the Principal Identity");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Unable to identify user name."));
            }
            var user = await _userManager.FindByNameAsync(userName);
            
            if(user == null || user.RefreshToken != refreshRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogWarning("Attempted to Refresh with invalid or expired refresh token.");
                return new AuthenticationDetails(new AuthenticationResponse(false, "Invalid or expired token."));
            }

            return await BuildSuccessfulAuthenticationDetails(user);
        }
        
        public async Task<bool> SetupUsersAndRoles()
        {
            // One-Off method to establish users and roles in the database (for the sake of demo).
            // Normally would be replaced by some role manager or 3rd party auth service
            
            // Check if users already created.  (Assume if admin exists then they all do).
            var existing = await _userManager.FindByNameAsync("admin");
            if(existing != null)
            {
                _logger.LogInformation("Users and Roles have already been created.");
                return true;
            }

            _logger.LogInformation("Setting up users and roles.");
            var users = new List<UserInfo>
            {
                new UserInfo { UserName = "admin", Password = "password", DisplayName = "Administrator", Roles = new []{ Role.Administrator } },
                new UserInfo { UserName = "stocktake", Password = "password", DisplayName = "Stocktake User", Roles = new []{ Role.StocktakeUser } },
                new UserInfo { UserName = "guest", Password = "password", DisplayName = "Guest User", Roles = new []{Role.Guest } }
            };

            foreach (var userInfo in users)
            {
                var identityUser = new ApplicationUser { UserName = userInfo.UserName, DisplayName = userInfo.DisplayName };
                identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, userInfo.Password!);
                await _userManager.CreateAsync(identityUser);

                if(userInfo.Roles != null && userInfo.Roles.Any())
                {
                    foreach (var roleName in userInfo.Roles)
                    {                        
                        var identityRole = new IdentityRole(roleName);
                        await _roleManager.CreateAsync(identityRole);
                        await _userManager.AddToRoleAsync(identityUser, roleName);
                    }
                }
            }

            return true;
        }

        private async Task<AuthenticationDetails> BuildSuccessfulAuthenticationDetails(ApplicationUser user)
        {
            string accessToken = await CreateAccessToken(user);
            var refreshToken = CreateRefreshToken();
            await StoreRefreshToken(user, refreshToken);
            var authenticationResponse = new AuthenticationResponse(user.UserName, user.DisplayName, true, "Authentication successful.", accessToken);
            return new AuthenticationDetails(authenticationResponse, refreshToken.Token);
        }

        private async Task<string> CreateAccessToken(ApplicationUser user)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return accessToken;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SigninKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            if(user.UserName == null)
            {
                _logger.LogWarning("Unable to get claims for user as UserName was not provided.");
                return new List<Claim>();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.ExpiryInMinutes)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private RefreshToken CreateRefreshToken()
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiryTime = DateTime.Now.AddHours(_jwtSettings.RefreshTokenExpiryInHours)
            };
        }

        private async Task StoreRefreshToken(ApplicationUser user, RefreshToken refreshToken)
        {
            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenExpiryTime = refreshToken.ExpiryTime;
            await _userManager.UpdateAsync(user);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            token = token.Replace("Bearer ", "").Replace("bearer ", "");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigninKey)),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.FromMinutes(1)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
