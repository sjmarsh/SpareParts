namespace SpareParts.Shared.Models
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()  // required for deserialization
        {
        }

        public AuthenticationResponse(bool isAuthenticated, string? message) : this(null, null, isAuthenticated, message, null)
        {
        }

        public AuthenticationResponse(string? userName, string? displayName, bool isAuthenticated, string? message, string? accessToken)
        {
            UserName = userName;
            DisplayName = displayName;
            IsAuthenticated = isAuthenticated;
            Message = message;
            AccessToken = accessToken;
        }

        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }
        public string? AccessToken { get; set; }
    }
}
