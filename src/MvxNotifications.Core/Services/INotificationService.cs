using System;
namespace MvxNotifications.Core.Services
{
    public class NotificationInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Message { get; set; }
    }

    public interface INotificationService
    {
        //event EventHandler NotificationReceived;
        //event EventHandler<int> ShowNotification;
        // void Initialize();
        void SendNotification(NotificationInfo notificationInfo, DateTime? notifyTime = null);
        //void ReceiveNotification(string title, string message, int id);
        //void OpenNotification(int id);
    }
}
