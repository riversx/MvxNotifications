using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace MvxNotifications.Core.ViewModels.Home
{
    public class HomeViewModel : BaseViewModel
    {
        public HomeViewModel()
        {
            SendNotificationCommand = new MvxCommand(OnSendNotificationCommand);
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

        public IMvxCommand SendNotificationCommand { get; }
        private void OnSendNotificationCommand()
        {
            _notificationNumber++;
            NotificationsList.Add($"Immediate Notification {_notificationNumber}");
            // Send Immediate message
        }

        public IMvxCommand SendScheduledNotificationCommand { get; }
        private void OnSendScheduledNotificationCommand()
        {
            _notificationNumber++;
            NotificationsList.Add($"Scheduled Notification {_notificationNumber}");
        }

        #endregion commands 
    }
}
