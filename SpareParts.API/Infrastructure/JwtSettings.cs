namespace SpareParts.API.Infrastructure
{
    public class JwtSettings
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string SigninKey { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public int ExpiryInMinutes { get; set; }
        public int RefreshTokenExpiryInHours { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
