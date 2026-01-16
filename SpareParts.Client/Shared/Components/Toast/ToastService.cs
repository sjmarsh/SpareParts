namespace SpareParts.Client.Shared.Components.Toast
{
    public interface IToastService
    {
        event Action? OnHide;
        event Action<string, ToastLevel>? OnShow;

        void HideToast();
        void ShowError(string message);
        void ShowInfo(string message);
        void ShowSuccess(string message);
        void ShowToast(string message, ToastLevel level);
        void ShowWarning(string message);
    }

    public class ToastService : IToastService
    {
        public void ShowInfo(string message)
        {
            ShowToast(message, ToastLevel.Info);
        }

        public void ShowSuccess(string message)
        {
            ShowToast(message, ToastLevel.Success);
        }

        public void ShowWarning(string message)
        {
            ShowToast(message, ToastLevel.Warning);
        }

        public void ShowError(string message)
        {
            ShowToast(message, ToastLevel.Error);
        }

        public void ShowToast(string message, ToastLevel level)
        {
            OnShow?.Invoke(message, level);
        }

        public void HideToast()
        {
            OnHide?.Invoke();
        }

        public event Action<string, ToastLevel>? OnShow;
        public event Action? OnHide;
    }

    public static class ToastServiceCollectionExtension
    {
        public static IServiceCollection AddToastService(this IServiceCollection services)
        {
            return services.AddSingleton<IToastService, ToastService>();
        }
    }
}
