using System.Net.Http.Headers;

namespace SpareParts.Client.Services.Authentication
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IAuthTokenStore _authTokenStore;
        
        public AuthHeaderHandler(IAuthTokenStore authTokenStore) : base()
        {
            _authTokenStore = authTokenStore;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _authTokenStore.GetToken();
            if(token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
