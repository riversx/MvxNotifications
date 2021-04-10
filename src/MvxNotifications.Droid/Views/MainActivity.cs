using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Views;
using MvxNotifications.Core.Services;
using MvxNotifications.Core.ViewModels.Main;
using MvxNotifications.Droid.Services;

namespace MvxNotifications.Droid
{
    [Activity(
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : MvxFormsAppCompatActivity<MainViewModel>
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (intent.IsNotification())
            {
                var title = intent.GetStringExtra(AndroidNotificationService.TitleKey);
                var message = intent.GetStringExtra(AndroidNotificationService.MessageKey);
                var id = intent.GetIntExtra(AndroidNotificationService.IdKey, 0);
                Mvx.IoCProvider.GetSingleton<INotificationService>().OpenNotification(id);
            }
        }
    }
}
