using Microsoft.Extensions.Logging;
using MvvmCross.Forms.Platforms.Ios.Core;
using MvvmCross.IoC;
using MvxNotifications.Core.Services;
using MvxNotifications.iOS.Services;
using Serilog;
using Serilog.Extensions.Logging;

namespace MvxNotifications.iOS
{
    public class Setup : MvxFormsIosSetup<Core.App, UI.App>
    {
        protected override ILoggerProvider CreateLogProvider() => new SerilogLoggerProvider();

        protected override ILoggerFactory CreateLogFactory()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();

            return new SerilogLoggerFactory();
        }

        protected override void InitializeFirstChance(IMvxIoCProvider iocProvider)
        {
            iocProvider.RegisterSingleton<INotificationService>(new IOSNotificationService());
            base.InitializeFirstChance(iocProvider);
        }
    }
}
