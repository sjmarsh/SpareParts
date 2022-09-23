using Blazored.Toast.Services;
using Refit;

namespace SpareParts.Client.Services
{
    public interface IServiceWrapper
    {
        Task<TResult> ServiceCall<TResult>(Func<Task<TResult>> serviceCall, string friendlyErrorMessage) where TResult : class, new();
    }

    public class ServiceWrapper : IServiceWrapper
    {
        private readonly ILogger<ServiceWrapper> _logger;
        private readonly ILoadingIndicatorService _loadingIndicatorService;
        private readonly IToastService _toastService;

        public ServiceWrapper(ILogger<ServiceWrapper> logger, ILoadingIndicatorService loadingIndicatorService, IToastService toastService)
        {
            _logger = logger;
            _loadingIndicatorService = loadingIndicatorService;
            _toastService = toastService;
        }

        public async Task<TResult> ServiceCall<TResult>(Func<Task<TResult>> serviceCall, string friendlyErrorMessage) where TResult : class, new()
        {
            try
            {
                _loadingIndicatorService.SetIsLoading(true);
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
