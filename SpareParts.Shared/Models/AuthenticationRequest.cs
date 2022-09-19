namespace SpareParts.Shared.Models
{
    public record AuthenticationRequest
    {
        public AuthenticationRequest() : this("", "")  // need empty constructor for serialization
        {
        }

        public AuthenticationRequest(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
