using Microsoft.JSInterop;

namespace SpareParts.Client.Services
{
    public interface ILocalStorageService
    {
        Task<bool> ContainsKeyAsync(LocalStorageKey key);
        Task<string> GetDataAsync(LocalStorageKey key);
        Task<T?> GetDataAsync<T>(LocalStorageKey key);
        Task SaveDataAsync(LocalStorageKey key, string value);
        Task SaveDataAsync<T>(LocalStorageKey key, T value);
        Task RemoveDataAsync(LocalStorageKey key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> ContainsKeyAsync(LocalStorageKey key)
        {
            var value = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key.ToString());
            return value != null;
        }

        public async Task<string> GetDataAsync(LocalStorageKey key)
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key.ToString());
        }

        public async Task<T?> GetDataAsync<T>(LocalStorageKey key)
        {
            var value = await GetDataAsync(key);
            if(value == null)
            {
                throw new KeyNotFoundException($"The local storage key: '{key}' was not found.");
            }
            return value != null ? System.Text.Json.JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task SaveDataAsync(LocalStorageKey key, string value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key.ToString(), value);
        }

        public async Task SaveDataAsync<T>(LocalStorageKey key, T value)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key.ToString(), value);
        }

        public async Task RemoveDataAsync(LocalStorageKey key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key.ToString());
        }
    }

    public enum LocalStorageKey
    {
        IsDarkModeEnabled
    }

    public static class LocalStorageServiceCollectionExtension
    {
        public static IServiceCollection AddLocalStorageService(this IServiceCollection services)
        {
            return services.AddScoped<ILocalStorageService, LocalStorageService>();
        }
    }

}

