using System;
namespace MvxNotifications.Core.Services
{
    public interface INotificationService
    {
        // publischer
        void Publish(NotificationInfo notificationInfo, DateTime? notifyTime = null);

        // listener
        event EventHandler<NotificationInfo> OnNotificationReceived;
        event EventHandler<NotificationInfo> OnNotificationOpened;
        void NotificationReceived(NotificationInfo notificationInfo);
        void NotificationOpened(NotificationInfo notificationInfo);
    }
}
