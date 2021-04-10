using System;
using Foundation;
using MvvmCross;
using ObjCRuntime;
using UIKit;
using UserNotifications;
using MvxNotifications.Core.Services;

namespace MvxNotifications.iOS.Services
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(
            UNUserNotificationCenter center,
            UNNotification notification,
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            ProcessNotification(notification);
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        void ProcessNotification(UNNotification notification)
        {
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;
            var info = notification.Request.Content.UserInfo;
            var id = (NSNumber)info.ValueForKey((NSString)"id");

            Mvx.IoCProvider.GetSingleton<INotificationService>().ReceiveNotification(title, message, id.Int32Value);
        }

        public override void DidReceiveNotificationResponse(
            UNUserNotificationCenter center,
            UNNotificationResponse response,
            [BlockProxy(typeof(UIAction))] Action completionHandler)
        {
            switch (response.ActionIdentifier)
            {
                case "com.apple.UNNotificationDefaultActionIdentifier":
                    var info = response.Notification.Request.Content.UserInfo;
                    var id = (NSNumber)info.ValueForKey((NSString)"id");
                    Mvx.IoCProvider.GetSingleton<INotificationService>().OpenNotification(id.Int32Value);
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