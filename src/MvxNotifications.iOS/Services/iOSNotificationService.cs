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
        public event EventHandler NotificationReceived;
        public event EventHandler<int> ShowNotification;

        public IOSNotificationService()
        {
            Initialize();
        }

        public void Initialize()
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
                UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();

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

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            // EARLY OUT: app doesn't have permissions
            if (!_hasNotificationsPermission)
            {
                return;
            }

            _messageId++;

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                UserInfo = new NSDictionary(new NSString("id"), new NSNumber(_messageId))
            };

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

        public void ReceiveNotification(string title, string message, int id)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
                Id = id
            };
            NotificationReceived?.Invoke(null, args);
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

        public void OpenNotification(int id)
        {
            ShowNotification?.Invoke(this, id);
        }
    }
}