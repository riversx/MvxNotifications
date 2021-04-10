using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using Xamarin.Forms;
using MvxNotifications.Core.Services;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(MvxNotifications.Droid.Services.AndroidNotificationService))]
namespace MvxNotifications.Droid.Services
{
    public class AndroidNotificationService : INotificationService
    {
        private const string channelId = "default";
        private const string channelName = "Default";
        private const string channelDescription = "The default channel for notifications.";

        private bool channelInitialized = false;
        private int pendingIntentId = 0;
        private NotificationManager manager;

        public static AndroidNotificationService Instance { get; private set; }

        public AndroidNotificationService()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        private void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        #region publisher

        public void Publish(NotificationInfo notificationInfo, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                var intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(notificationInfo);

                var pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                var triggerTime = GetNotifyTime(notifyTime.Value);
                var alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                ProcessNotification(notificationInfo);
            }
        }

        public void ProcessNotification(NotificationInfo notificationInfo)
        {
            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(notificationInfo);

            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(notificationInfo.Title)
                .SetSubText(notificationInfo.SubTitle)
                .SetContentText(notificationInfo.Message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mvvmcross_logo))
                .SetSmallIcon(Resource.Drawable.ic_mvvmcross_logo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(notificationInfo.Id, notification);

            NotificationReceived(notificationInfo);
        }

        private long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            var epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            var utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
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
            manager.Cancel(notificationInfo.Id);
            OnNotificationOpened?.Invoke(this, notificationInfo);
        }

        #endregion listener
    }
}
