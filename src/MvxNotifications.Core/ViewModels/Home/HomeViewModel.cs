using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxNotifications.Core.Services;

namespace MvxNotifications.Core.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private INotificationService _notificationService { get; }

        public HomeViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;

            SendInstantNotificationCommand = new MvxCommand(OnSendInstantNotificationCommand);
            SendScheduledNotificationCommand = new MvxCommand(OnSendScheduledNotificationCommand);
        }

        private int _notificationNumber = 0;


        #region properties

        private string _title ="Mvx Notifications";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _welcomeMessage = "App scaffolded with MvxScaffolding\nand Notifications";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set => SetProperty(ref _welcomeMessage, value);
        }

        private MvxObservableCollection<string> _notificationsList = new MvxObservableCollection<string>();
        public MvxObservableCollection<string> NotificationsList
        {
            get => _notificationsList;
            set => SetProperty(ref _notificationsList, value);
        }

        #endregion properties


        #region commands

        public IMvxCommand SendInstantNotificationCommand { get; }
        private void OnSendInstantNotificationCommand()
        {
            _notificationNumber++;
            var notificationInfo = new NotificationInfo
            {
                Id = _notificationNumber,
                Title = $"Instant Notification #{_notificationNumber}",
                SubTitle = "",
                Message = $"You have now received {_notificationNumber} notifications!"
            };
            _notificationService.SendNotification(notificationInfo);
            // NotificationsList.Add($"Immediate Notification {_notificationNumber}");
        }

        public IMvxCommand SendScheduledNotificationCommand { get; }
        private void OnSendScheduledNotificationCommand()
        {
            _notificationNumber++;
            var notificationInfo = new NotificationInfo
            {
                Id = _notificationNumber,
                Title = $"Scheduled Notification #{_notificationNumber}",
                SubTitle = "",
                Message = $"You have now received {_notificationNumber} notifications!"
            };
            _notificationService.SendNotification(notificationInfo, DateTime.Now.AddSeconds(10));
            // NotificationsList.Add($"Scheduled Notification {_notificationNumber}");
        }

        #endregion commands 
    }
}
