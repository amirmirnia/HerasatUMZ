using Domain.Enum;

namespace Client.Services.Alert
{
    public class AlertService
    {
        public event Action<string, string, AlertType>? OnShow;

        public void Show(string message, string? title = null, AlertType type = AlertType.Info)
            => OnShow?.Invoke(message, title ?? "", type);
    }
}
