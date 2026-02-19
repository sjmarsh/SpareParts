using Microsoft.JSInterop;

namespace SpareParts.Client.Services
{
    public interface IThemeService
    {
        Task InitializeTheme();
        Task SetDarkMode(bool isDarkModeEnabled);
        Task<bool> IsDarkModeEnabled();
    }

    public class ThemeService : IThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly ILocalStorageService _localStorageService;

        public ThemeService(IJSRuntime jsRuntime, ILocalStorageService localStorageService)
        {
            _jsRuntime = jsRuntime;
            _localStorageService = localStorageService;
        }

        public async Task InitializeTheme()
        {
            var isDarkModeEnabled = await IsDarkModeEnabled();
            await SetDarkMode(isDarkModeEnabled);
        }

        public async Task<bool> IsDarkModeEnabled()
        {
            return await _localStorageService.ContainsKeyAsync(LocalStorageKey.IsDarkModeEnabled) 
                   && await _localStorageService.GetDataAsync<bool>(LocalStorageKey.IsDarkModeEnabled);
        }

        public async Task SetDarkMode(bool isDarkModeEnabled)
        {
            IJSObjectReference module;
            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./js/helpers.js");
            await module.InvokeVoidAsync("setDarkMode", isDarkModeEnabled);
        }
    }

    public static class ThemeServiceCollectionExtension
    {
        public static IServiceCollection AddThemeService(this IServiceCollection services)
        {
            return services.AddScoped<IThemeService, ThemeService>();
        }
    }
}
