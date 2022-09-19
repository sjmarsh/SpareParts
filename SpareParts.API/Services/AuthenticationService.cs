using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SpareParts.API.Infrastructure;
using SpareParts.Shared.Models;

namespace SpareParts.API.Services
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest request);
        UserInfo? GetUserByUserName(string userName);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthenticationService> _logger;

        private readonly List<UserInfo> _userRepository = new()
        {
                new UserInfo { UserName = "admin", Password = "password", DisplayName = "Administrator" },
                new UserInfo { UserName = "stocktake", Password = "password", DisplayName = "Stocktake User" },
                new UserInfo { UserName = "guest", Password = "password", DisplayName = "Guest User" },
        };

        public AuthenticationService(IOptions<AppSettings> appSettings, ILogger<AuthenticationService> logger)
        {
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            if(request.UserName == null || request.Password == null)
            {
                // This should be handled by validation but just in case...
                _logger.LogWarning("Attempted to Authenticate with incomplete credentials.");  
                return new AuthenticationResponse(false, "Incomplete credentials.");
            }

            var user = GetUser(request.UserName, request.Password);
            if(user == null)
            {
                _logger.LogWarning($"User: {request.UserName} is not a valid user or password is invalid.");
                return new AuthenticationResponse(false, "Invalid credentials.");
            }

            var token = GenerateToken(user);

            return new AuthenticationResponse(user.UserName, user.DisplayName, true, "Authentication successful.", token);
        }

        public UserInfo? GetUserByUserName(string userName)
        {
            return _userRepository.FirstOrDefault(u => u.UserName == userName);
        }

        private UserInfo? GetUser(string userName, string password)
        {
            return _userRepository.FirstOrDefault(u => u.UserName == userName && u.Password == password);
        }

        private string GenerateToken(UserInfo user)
        {
            if(_appSettings.Secret == null)
            {
                const string message = "Unable to generate token without Secret in AppSettings.";
                _logger.LogError(message);
                throw new ConfigurationException(message);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userName", user.UserName!.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
