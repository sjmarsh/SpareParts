namespace SpareParts.Shared.Models
{
    public record RefreshRequest
    {
        public RefreshRequest(string? accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
    }
}
