using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace SpareParts.Client.Services.Authentication
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorage;

        public AuthHeaderHandler(ILocalStorageService localStorage) : base()
        {
            _localStorage = localStorage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _localStorage.GetItemAsStringAsync(AuthToken.Name, cancellationToken);
            if (!string.IsNullOrEmpty(token))
            {
                token = token.Replace("\"", "");
            }

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
