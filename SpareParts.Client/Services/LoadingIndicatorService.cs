namespace SpareParts.Client.Services
{
    public interface ILoadingIndicatorService
    {
        event Action<bool>? OnLoadingStateChanged;
        void SetIsLoading(bool isLoading);
    }

    public class LoadingIndicatorService : ILoadingIndicatorService
    {
        private bool _isLoading;

        public event Action<bool>? OnLoadingStateChanged;
        
        public void SetIsLoading(bool isLoading)
        {
            _isLoading = isLoading;
            OnLoadingStateChanged?.Invoke(_isLoading);   
        }
    }

    public static class LoadingIndicatorServiceCollectionExtension
    {
        public static IServiceCollection AddLoadingIndicatorService(this IServiceCollection services)
        {
            return services.AddScoped<ILoadingIndicatorService, LoadingIndicatorService>();
        }
    }
}
