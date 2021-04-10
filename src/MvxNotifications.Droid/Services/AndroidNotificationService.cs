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

        public const string TitleKey = "keyTitle";
        public const string MessageKey = "keyMessage";
        public const string IdKey = "keyId";
        private bool channelInitialized = false;
        private int messageId = 0;
        private int pendingIntentId = 0;
        private NotificationManager manager;

        public event EventHandler NotificationReceived;
        public event EventHandler<int> ShowNotification;

        public static AndroidNotificationService Instance { get; private set; }

        public AndroidNotificationService() => Initialize();

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            messageId++;
            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);
                intent.PutExtra(IdKey, messageId);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                ProcessNotification(title, message, messageId);
            }
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

        public void ProcessNotification(string title, string message, int id)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra(IdKey, id);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_mvvmcross_logo))
                .SetSmallIcon(Resource.Drawable.ic_mvvmcross_logo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(id, notification);

            ReceiveNotification(title, message, id);
        }

        void CreateNotificationChannel()
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

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }

        public void OpenNotification(int id)
        {
            manager.Cancel(id);
            ShowNotification?.Invoke(this, id);
        }
    }

    public static class IntentExtensions
    {
        public static bool IsNotification(this Intent intent)
        {
            return intent.HasExtra(AndroidNotificationService.IdKey);
        }
    }
}
