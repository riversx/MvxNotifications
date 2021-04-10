using MvvmCross;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvxNotifications.Core.Services;
using MvxNotifications.iOS.Services;

namespace MvxNotifications.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, UI.App>
    {

        protected override void InitializeFirstChance()
        {
            Mvx.IoCProvider.RegisterSingleton<INotificationService>(new IOSNotificationService());
            base.InitializeFirstChance();
        }
    }
}
