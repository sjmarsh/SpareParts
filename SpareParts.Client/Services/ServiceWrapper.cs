using Blazored.Toast.Services;
using Refit;
using SpareParts.Client.Services.Authentication;

namespace SpareParts.Client.Services
{
    public interface IServiceWrapper
    {
        /// <summary>
        /// Wraps a service call with loading states, authentication token refresh and error handling / logging.
        /// </summary>
        /// <typeparam name="TResult">The result type for the service call</typeparam>
        /// <param name="serviceCall">A function delegate for the service call</param>
        /// <param name="friendlyErrorMessage">User friendly error message</param>
        /// <param name="shouldRefreshAuthentication">Should this attempt to refresh the authentication token before making the service call.  Default = true.</param>
        /// <returns></returns>
        Task<TResult> ServiceCall<TResult>(Func<Task<TResult>> serviceCall, string friendlyErrorMessage, bool shouldRefreshAuthentication = true) where TResult : class, new();
    }

    public class ServiceWrapper : IServiceWrapper
    {        
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoadingIndicatorService _loadingIndicatorService;
        private readonly IToastService _toastService;
        private readonly ILogger<ServiceWrapper> _logger;

        public ServiceWrapper(IAuthenticationService authenticationService, ILoadingIndicatorService loadingIndicatorService, IToastService toastService, ILogger<ServiceWrapper> logger)
        {   
            _authenticationService = authenticationService;
            _loadingIndicatorService = loadingIndicatorService;
            _toastService = toastService;
            _logger = logger;
        }

        public async Task<TResult> ServiceCall<TResult>(Func<Task<TResult>> serviceCall, string friendlyErrorMessage, bool shouldRefresh = true) where TResult : class, new()
        {
            try
            {
                _loadingIndicatorService.SetIsLoading(true);

                if(shouldRefresh)
                {
                    var response = await _authenticationService.RefreshIfRequired();
                    if(response != null && response.IsAuthenticated == false)
                    {
                        _toastService.ShowWarning("You're session has expired. Please log-in and try again.");
                        _logger.LogWarning(response.Message);
                        return new TResult();
                    }
                }  

                return await serviceCall();
            }
            catch(ApiException ex)
            {
                //var apiErrors = await ex.GetContentAsAsync<Dictionary<string, string>>();
                //var apiErrorString = "";
                //if (apiErrors != null) apiErrorString = string.Join("; ", apiErrors.Values);
                
                _logger.LogError(ex, "The service call resulted in an API Error.");
                _toastService.ShowError(friendlyErrorMessage);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "The service call resulted in an error.");
                _toastService.ShowError(friendlyErrorMessage);
            }
            finally
            {
                _loadingIndicatorService.SetIsLoading(false);
            }
                        
            return new TResult();
        }
    }

    public static class ServiceWrapperServiceCollectionExtension
    {
        public static IServiceCollection AddServiceWrapper(this IServiceCollection services)
        {
            return services.AddScoped<IServiceWrapper, ServiceWrapper>();
        }
    }
}
