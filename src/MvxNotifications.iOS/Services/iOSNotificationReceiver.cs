using System;
using Foundation;
using MvvmCross;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using MvxNotifications.Core.Services;

namespace MvxNotifications.iOS.Services
{
    public class IOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            ProcessNotification(notification);
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        private void ProcessNotification(UNNotification notification)
        {
            NotificationInfo notificationInfo = notification.GetNotificationInfo();
            Mvx.IoCProvider.GetSingleton<INotificationService>().NotificationReceived(notificationInfo);
        }

        public override void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            [BlockProxy(typeof(UIAction))] Action completionHandler)
        {
            switch (response.ActionIdentifier)
            {
                case "com.apple.UNNotificationDefaultActionIdentifier":
                    NotificationInfo notificationInfo = response.Notification.GetNotificationInfo();
                    Mvx.IoCProvider.GetSingleton<INotificationService>().NotificationOpened(notificationInfo);
                    break;
                default:
                    break;
            }
            // Close Notification ???
            // completionHandler(UNNotificationContentExtensionResponseOption.Dismiss);
        }

        public override void DidChange(NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey)
        {
            base.DidChange(changeKind, indexes, forKey);
        }
    }
}