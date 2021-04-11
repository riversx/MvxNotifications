using System;
using System.Linq;
using System.Threading.Tasks;
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
            DeleteNotificationCommand = new MvxCommand<NotificationInfoViewModel>(OnDeleteNotificationCommand);
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

        private MvxObservableCollection<NotificationInfoViewModel> _notificationsList = new MvxObservableCollection<NotificationInfoViewModel>();
        public MvxObservableCollection<NotificationInfoViewModel> NotificationsList
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

        public IMvxCommand<NotificationInfoViewModel> DeleteNotificationCommand { get; }
        private void OnDeleteNotificationCommand(NotificationInfoViewModel e)
        {
            var request = new YesNoQuestion
            {
                YesNoCallback = async (ok) =>
                {
                    if (ok)
                    {
                        NotificationInfoViewModel notificationToRemove = NotificationsList.FirstOrDefault(n => n.Id.Equals(e.Id));
                        if (notificationToRemove != null)
                            NotificationsList.Remove(notificationToRemove);
                        await Task.Delay(100);
                    }
                },
                Title = "Delete notification",
                Question = "Do you want to remove this notification from list?"
            };
            _requestDeleteInteraction.Raise(request);
        }

        #endregion commands


        #region interactions

        private MvxInteraction<YesNoQuestion> _requestDeleteInteraction = new MvxInteraction<YesNoQuestion>();
        public IMvxInteraction<YesNoQuestion> RequestDeleteInteraction => _requestDeleteInteraction;

        private MvxInteraction<AlertMessage> _requestOpenInteraction = new MvxInteraction<AlertMessage>();
        public IMvxInteraction<AlertMessage> RequestOpenInteraction => _requestOpenInteraction;

        #endregion interactions


        #region events

        private void NotificationService_OnNotificationReceived(object sender, NotificationInfo e)
        {
            NotificationInfoViewModel viewModel = e.ToViewModel(DeleteNotificationCommand);
            NotificationsList.Add(viewModel);
        }

        private void NotificationService_OnNotificationOpened(object sender, NotificationInfo e)
        {
            var request = new AlertMessage
            {
                Title = e.Title,
                Message =  $"{e.SubTitle}\n{e.Message}",
                AcceptText = "OK",
                OkCallback = () =>
                {
                    NotificationInfoViewModel notificatioOpened = NotificationsList.FirstOrDefault(n => n.Id.Equals(e.Id));
                    notificatioOpened.IsRead = true;
                }
            };
            _requestOpenInteraction.Raise(request);
        }

        #endregion events 
    }
}
