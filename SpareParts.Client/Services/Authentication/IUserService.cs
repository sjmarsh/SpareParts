using Refit;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Services.Authentication
{
    public interface IUserService
    {
        [Post("/api/user/authenticate")]
        public Task<AuthenticationResponse> Authenticate(AuthenticationRequest authenticationRequest);

        [Post("/api/user/refresh")]
        public Task<AuthenticationResponse> Refresh();
    }
}
