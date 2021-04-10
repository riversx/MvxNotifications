using Android.Content;
using Xamarin.Forms;

namespace MvxNotifications.Droid.Services
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                var title = intent.GetStringExtra(AndroidNotificationService.TitleKey);
                var message = intent.GetStringExtra(AndroidNotificationService.MessageKey);
                var id = intent.GetIntExtra(AndroidNotificationService.IdKey, 0);

                AndroidNotificationService manager = AndroidNotificationService.Instance ?? new AndroidNotificationService();
                manager.ProcessNotification(title, message, id);
            }
        }
    }
}
