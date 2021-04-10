using Android.Content;
using Xamarin.Forms;
using MvxNotifications.Core.Services;

[assembly: Dependency(typeof(MvxNotifications.Droid.Services.AndroidNotificationService))]
namespace MvxNotifications.Droid.Services
{
    public static class IntentBaseNotificationExtensions
    {
        private const string NotificationTitleKey = "keyTitle";
        private const string NotificationSubTitleKey = "keySubTitle";
        private const string NotificationMessageKey = "keyMessage";
        private const string NotificationIdKey = "keyNotificationId";

        public static void PutExtra(this Intent intent, NotificationInfo notificationInfo)
        {
            intent.PutExtra(NotificationTitleKey, notificationInfo.Title);
            intent.PutExtra(NotificationSubTitleKey, notificationInfo.SubTitle);
            intent.PutExtra(NotificationMessageKey, notificationInfo.Message);
            intent.PutExtra(NotificationIdKey, notificationInfo.Id);
            // add here extra info
        }

        public static bool IsNotification(this Intent intent)
        {
            return intent.HasExtra(NotificationIdKey);
        }

        public static NotificationInfo GetNotificationInfo(this Intent intent)
        {
            return new NotificationInfo
            {
                Title = intent.GetStringExtra(NotificationTitleKey),
                SubTitle = intent.GetStringExtra(NotificationSubTitleKey),
                Message = intent.GetStringExtra(NotificationMessageKey),
                Id = intent.GetIntExtra(NotificationIdKey, 0),
                // read here extra info 
            };
        }
    }
}
