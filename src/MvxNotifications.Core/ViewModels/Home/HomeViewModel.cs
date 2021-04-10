using System;
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
            _notificationService.OnNotificationReceived += NotificationService_OnNotificationReceived;
            _notificationService.OnNotificationOpened += NotificationService_OnNotificationOpened;

            SendInstantNotificationCommand = new MvxCommand(OnSendInstantNotificationCommand);
            SendScheduledNotificationCommand = new MvxCommand(OnSendScheduledNotificationCommand);
        }

        private int _notificationNumber = 0;


        #region properties

        private string _title = "Mvx Notifications";
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

        private MvxObservableCollection<NotificationInfo> _notificationsList = new MvxObservableCollection<NotificationInfo>();
        public MvxObservableCollection<NotificationInfo> NotificationsList
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
                Title = $"Notification #{_notificationNumber}",
                SubTitle = "Instant",
                Message = $"You have now received {_notificationNumber} notifications!"
            };
            _notificationService.Publish(notificationInfo);
        }

        public IMvxCommand SendScheduledNotificationCommand { get; }
        private void OnSendScheduledNotificationCommand()
        {
            _notificationNumber++;
            DateTime instant = DateTime.Now.AddSeconds(10);
            var notificationInfo = new NotificationInfo
            {
                Id = _notificationNumber,
                Title = $"Notification #{_notificationNumber}",
                SubTitle = $"Scheduled at {instant.ToString("dd/MM/yy HH:mm:ss")}",
                Message = $"You have now received {_notificationNumber} notifications!"
            };
            _notificationService.Publish(notificationInfo, DateTime.Now.AddSeconds(10));
        }

        #endregion commands



        #region events

        private void NotificationService_OnNotificationReceived(object sender, NotificationInfo e)
        {
            NotificationsList.Add(e);
        }

        private void NotificationService_OnNotificationOpened(object sender, NotificationInfo e)
        {
            DisplayAlert?.Invoke(sender, new DisplayAlertEventArgs
            {
                Title = e.Title,
                Message = $"{e.SubTitle}\n{e.Message}",
                AcceptText = "OK"
            });
        }

        #endregion events 
    }
}
