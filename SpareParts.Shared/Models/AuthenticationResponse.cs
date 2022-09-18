namespace SpareParts.Shared.Models
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()  // required for deserialization
        {
        }

        public AuthenticationResponse(bool isAuthenticated) : this(null, null, isAuthenticated, null)
        {
        }

        public AuthenticationResponse(string? userName, string? displayName, bool isAuthenticated, string? token)
        {
            UserName = userName;
            DisplayName = displayName;
            IsAuthenticated = isAuthenticated;
            Token = token;
        }

        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
    }
}
