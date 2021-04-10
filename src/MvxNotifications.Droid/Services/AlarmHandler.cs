using Android.Content;
using MvxNotifications.Core.Services;
using Xamarin.Forms;

namespace MvxNotifications.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.IsNotification())
            {
                NotificationInfo notificationInfo = intent.GetNotificationInfo();
                AndroidNotificationService manager = AndroidNotificationService.Instance ?? new AndroidNotificationService();
                manager.ProcessNotification(notificationInfo);
            }
        }
    }
}
