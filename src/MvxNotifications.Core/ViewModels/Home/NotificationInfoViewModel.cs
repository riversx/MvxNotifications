using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using MvxNotifications.Core.Services;

namespace MvxNotifications.Core.ViewModels.Home
{
    public class NotificationInfoViewModel: MvxViewModel
    {
        public IMvxCommand<NotificationInfoViewModel> DeleteNotificationCommand { get; set; }

        public string EnvelopeClosedIconCode => Resources.Fonts.MaterialDesignIconsCodes.EmailOutline;
        public string EnvelopeOpenedIconCode => Resources.Fonts.MaterialDesignIconsCodes.EmailOpenOutline;
        public string DeleteIconCode => Resources.Fonts.MaterialDesignIconsCodes.TrashCanOutline;

        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Message { get; set; }
        private bool _isRead;
        public bool IsRead
        {
            get => _isRead;
            set
            {
                SetProperty(ref _isRead, value);
                RaisePropertyChanged(nameof(IsUnread));
            }
        }
        public bool IsUnread => !_isRead;
    }

    public static class NotificationInfoExtensions
    {
        public static NotificationInfoViewModel ToViewModel(this NotificationInfo info, IMvxCommand<NotificationInfoViewModel> deleteNotificationCommand,  bool isRead = false)
        {
            return new NotificationInfoViewModel
            {
                Id = info.Id,
                Title = info.Title,
                SubTitle = info.SubTitle,
                Message = info.Message,
                IsRead = isRead,
                DeleteNotificationCommand = deleteNotificationCommand
            };
        }
    }
}