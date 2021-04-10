using System;
namespace MvxNotifications.Core.Services
{
    public class NotificationEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
    }

    public interface INotificationService
    {
        event EventHandler NotificationReceived;
        event EventHandler<int> ShowNotification;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null);
        void ReceiveNotification(string title, string message, int id);
        void OpenNotification(int id);
    }
}
