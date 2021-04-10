using MvvmCross.IoC;
using MvvmCross.ViewModels;
using MvxNotifications.Core.ViewModels.Home;

namespace MvxNotifications.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<HomeViewModel>();
        }
    }
}
