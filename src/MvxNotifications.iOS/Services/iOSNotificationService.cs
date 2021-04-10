using System;
using MvxNotifications.Core.Services;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(MvxNotifications.iOS.Services.IOSNotificationService))]
namespace MvxNotifications.iOS.Services
{
    public class IOSNotificationService : INotificationService
    {
        private int _messageId = 0;
        private bool _hasNotificationsPermission;

        public IOSNotificationService()
        {
            // UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10 or later
                UNAuthorizationOptions authOptions =
                    UNAuthorizationOptions.Alert
                    | UNAuthorizationOptions.Badge
                    | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    _hasNotificationsPermission = granted;
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = new IOSNotificationReceiver();

                // For iOS 10 data message (sent via FCM)
                //Messaging.SharedInstance.RemoteMessageDelegate = this;
            }
            else
            {
                // iOS 9 or before
                UIUserNotificationType allNotificationTypes =
                    UIUserNotificationType.Alert |
                    UIUserNotificationType.Badge |
                    UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
        }


        #region publisher

        public void Publish(NotificationInfo notificationInfo, DateTime? notifyTime = null)
        {
            // EARLY OUT: app doesn't have permissions
            if (!_hasNotificationsPermission)
            {
                return;
            }

            _messageId++;

            var content = new UNMutableNotificationContent();
            content.SetNotificationInfo(notificationInfo);

            UNNotificationTrigger trigger;
            if (notifyTime != null)
            {
                // Create a calendar-based trigger.
                trigger = UNCalendarNotificationTrigger.CreateTrigger(GetNSDateComponents(notifyTime.Value), false);
            }
            else
            {
                // Create a time-based trigger, interval is in seconds and must be greater than 0.
                trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);
            }

            var request = UNNotificationRequest.FromIdentifier(_messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
            });
        }

        private NSDateComponents GetNSDateComponents(DateTime dateTime)
        {
            return new NSDateComponents
            {
                Month = dateTime.Month,
                Day = dateTime.Day,
                Year = dateTime.Year,
                Hour = dateTime.Hour,
                Minute = dateTime.Minute,
                Second = dateTime.Second
            };
        }

        #endregion publisher



        #region listener

        public event EventHandler<NotificationInfo> OnNotificationReceived;
        public event EventHandler<NotificationInfo> OnNotificationOpened;

        public void NotificationReceived(NotificationInfo notificationInfo)
        {
            OnNotificationReceived?.Invoke(null, notificationInfo);
        }

        public void NotificationOpened(NotificationInfo notificationInfo)
        {
            OnNotificationOpened?.Invoke(this, notificationInfo);
        }

        #endregion listener
    }
}