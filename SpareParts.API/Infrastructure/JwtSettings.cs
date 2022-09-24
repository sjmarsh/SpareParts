namespace SpareParts.API.Infrastructure
{
    public class JwtSettings
    {
        public string SigninKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInHours { get; set; }
    }
}
