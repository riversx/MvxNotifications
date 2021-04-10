using Android.App;
using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using MvxNotifications.Core.Services;
using MvxNotifications.Droid.Services;

#if DEBUG
[assembly: Application(Debuggable = true)]
#else
[assembly: Application(Debuggable = false)]
#endif

namespace MvxNotifications.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.App, UI.App>
    {
        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterSingleton<INotificationService>(new AndroidNotificationService());
            base.InitializeFirstChance();
        }
    }
}
