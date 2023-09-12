namespace SpareParts.Client.Services
{
    public interface IMessageBoxService
    {
        ValueTask<MessageBoxResult> ShowMessage(string message, string title, MessageBoxType messageBoxType);
        void CloseMessage(MessageBoxResult messageBoxResult);
        event Action<string, string, MessageBoxType>? OnShowMessage;
    }

    public class MessageBoxService : IMessageBoxService
    {
        private SemaphoreSlim _semaphore = new(0, 1);
        private MessageBoxResult _messageBoxResult;

        public event Action<string, string, MessageBoxType>? OnShowMessage;
        
        public MessageBoxResult? MessageBoxResult { get; private set; }

        public async ValueTask<MessageBoxResult> ShowMessage(string message, string title, MessageBoxType messageBoxType)
        {
            OnShowMessage?.Invoke(message, title, messageBoxType);
            await _semaphore.WaitAsync();
            return _messageBoxResult;
        }

        public void CloseMessage(MessageBoxResult messageBoxResult)
        {
            _messageBoxResult = messageBoxResult;
            _semaphore.Release();
        }
    }

    public static class MessageBoxServiceCollectionExtension
    {
        public static IServiceCollection AddMessageBoxService(this IServiceCollection services)
        {
            return services.AddScoped<IMessageBoxService, MessageBoxService>();
        }
    }

    public enum MessageBoxType
    {
        OK,
        OKCancel,
        YesNo
    }

    public enum MessageBoxResult
    {
        OK,
        Cancel,
        Yes,
        No
    }
}
