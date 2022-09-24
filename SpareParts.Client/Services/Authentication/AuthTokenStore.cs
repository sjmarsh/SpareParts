using System.Security.Claims;

namespace SpareParts.Client.Services.Authentication
{
    // Using InMemory storage for the Access token.  There is much debate over the best way to store this in the client.
    // By having this interface, it leaves things open to implement local storage, session storage or some other means of storage later on by creating another implmentation of this interface.
    public interface IAuthTokenStore
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
        IEnumerable<Claim> GetClaims();
        bool HasTokenExpired();
    }

    public class InMemroryAuthTokenStore : IAuthTokenStore
    {
        private string? _token;

        public void SetToken(string token)
        {
            _token = token;
        }

        public string? GetToken()
        {
            return _token;
        }

        public void ClearToken()
        {
            _token = null;
        }


        public bool HasTokenExpired()
        {
            if (_token == null)
                return true;
                        
            var expiryClaim = GetClaims().FirstOrDefault(c => c.Type == "exp");
            if (expiryClaim != null)
            {
                var expiry = Convert.ToInt64(expiryClaim.Value);
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiry).ToLocalTime();
                return DateTime.Now >= expiryDate;
            }

            return false;
        }

        public IEnumerable<Claim> GetClaims()
        {
            if(_token == null)
                return Enumerable.Empty<Claim>();

            return JwtParser.ParseClaimsFromJwt(_token);
        }
    }

    public static class AuthTokenStoreServiceCollectionExtension
    {
        public static IServiceCollection AddInMemoryAuthTokenStore(this IServiceCollection services)
        {
            return services.AddSingleton<IAuthTokenStore, InMemroryAuthTokenStore>();
        }
    }
}
