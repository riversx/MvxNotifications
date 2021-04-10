using MvxNotifications.Core.Services;
using Foundation;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(MvxNotifications.iOS.Services.IOSNotificationService))]
namespace MvxNotifications.iOS.Services
{
    public static class UNMutableNotificationContentExtension
    {
        private const string NotificationIdKey = "keyNotificationId";

        public static void SetNotificationInfo(this UNMutableNotificationContent content, NotificationInfo notificationInfo)
        {
            content.Title = notificationInfo.Title;
            content.Subtitle = notificationInfo.SubTitle;
            content.Body = notificationInfo.Message;
            content.UserInfo = new NSDictionary(new NSString(NotificationIdKey), new NSNumber(notificationInfo.Id));
            // Add here extra info
        }

        public static NotificationInfo GetNotificationInfo(this UNNotification notification)
        {
            NSDictionary userInfo = notification.Request.Content.UserInfo;
            return new NotificationInfo
            {
                Title = notification.Request.Content.Title,
                Message = notification.Request.Content.Body,
                SubTitle = notification.Request.Content.Subtitle,
                Id = ((NSNumber)userInfo.ValueForKey((NSString)NotificationIdKey)).Int32Value,
                // Read here extra info
            };
        }
    }
}
