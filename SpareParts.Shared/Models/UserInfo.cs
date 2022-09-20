namespace SpareParts.Shared.Models
{
    public class UserInfo
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? DisplayName { get; set; }

        public string[]? Roles { get; set; }
    }
}
