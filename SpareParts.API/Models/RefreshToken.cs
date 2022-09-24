namespace SpareParts.API.Models
{
    public class RefreshToken
    {
        public string? Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
