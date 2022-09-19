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

        public AuthenticationResponse(string? userName, string? displayName, bool isAuthenticated, string? message, string? token)
        {
            UserName = userName;
            DisplayName = displayName;
            IsAuthenticated = isAuthenticated;
            Message = message;
            Token = token;
        }

        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
