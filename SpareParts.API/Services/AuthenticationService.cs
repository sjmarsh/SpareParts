using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SpareParts.API.Infrastructure;
using SpareParts.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace SpareParts.API.Services
{
    public interface IAuthenticationService
    {        
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
        Task<bool> SetupUsersAndRoles();
    }

    public class AuthenticationService : IAuthenticationService
    {        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthenticationService> _logger;
        
        public AuthenticationService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JwtSettings> jwtSettings, ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            if(request.UserName == null || request.Password == null)
            {
                // This should be handled by validation but just in case...
                _logger.LogWarning("Attempted to Authenticate with incomplete credentials.");  
                return new AuthenticationResponse(false, "Incomplete credentials.");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);
                        
            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (user == null || !isValidPassword)
            {
                _logger.LogWarning($"User: {request.UserName} is not a valid user or password is invalid.");
                return new AuthenticationResponse(false, "Invalid credentials");
            }

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new AuthenticationResponse(user.UserName, user.NormalizedUserName, true, "Authentication successful.", token);
        }

        public async Task<bool> SetupUsersAndRoles()
        {
            // One-Off method to establish users and roles in the database (for the sake of demo).
            // Normally would be replaced by some role manager or 3rd party auth service
            var users = new List<UserInfo>
            {
                new UserInfo { UserName = "admin", Password = "password", DisplayName = "Administrator", Roles = new []{ Role.Administrator } },
                new UserInfo { UserName = "stocktake", Password = "password", DisplayName = "Stocktake User", Roles = new []{ Role.StocktakeUser } },
                new UserInfo { UserName = "guest", Password = "password", DisplayName = "Guest User", Roles = new []{Role.Guest } }
            };

            foreach (var user in users)
            {
                var identityUser = new IdentityUser { UserName = user.UserName, NormalizedUserName = user.DisplayName };
                identityUser.PasswordHash = _userManager.PasswordHasher.HashPassword(identityUser, user.Password);
                await _userManager.CreateAsync(identityUser);

                if(user.Roles != null && user.Roles.Any())
                {
                    foreach (var roleName in user.Roles)
                    {                        
                        var identityRole = new IdentityRole(roleName);
                        await _roleManager.CreateAsync(identityRole);
                        await _userManager.AddToRoleAsync(identityUser, roleName);
                    }
                }
            }

            return true;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.SigninKey);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
        private async Task<List<Claim>> GetClaims(IdentityUser user)
        {
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

    }
}
