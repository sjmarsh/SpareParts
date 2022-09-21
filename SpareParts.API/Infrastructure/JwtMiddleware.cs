using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SpareParts.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SpareParts.API.Infrastructure
{
    //ref: https://jasonwatmore.com/post/2021/12/14/net-6-jwt-authentication-tutorial-with-example-api

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _appSettings = appSettings.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, authenticationService, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, IAuthenticationService authenticationService, string token)
        {
            try
            {
                if (_appSettings.Secret == null)
                {
                    const string message = "Unable to validate token without Secret in AppSettings.";
                    _logger.LogError(message);
                    throw new ConfigurationException(message);
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userName = jwtToken.Claims.First(x => x.Type == "userName").Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = authenticationService.GetUserByUserName(userName);
            }
            catch(Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                _logger.LogError("Failed to validate JWT token", ex);
            }
        }
    }
}
