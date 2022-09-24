using SpareParts.Shared.Models;

namespace SpareParts.API.Models
{
    public class AuthenticationDetails
    {
        public AuthenticationDetails(AuthenticationResponse authenticationResponse) : this(authenticationResponse, null)
        { 
        }

        public AuthenticationDetails(AuthenticationResponse authenticationResponse, string? refreshToken)
        {
            AuthenticationResponse = authenticationResponse;
            RefreshToken = refreshToken;
        }

        public AuthenticationResponse AuthenticationResponse { get; private set; }
        public string? RefreshToken { get; private set; }
    }
}
