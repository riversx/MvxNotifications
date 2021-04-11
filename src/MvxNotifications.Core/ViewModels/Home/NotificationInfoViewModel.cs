using MvvmCross.ViewModels;
using MvxNotifications.Core.Services;

namespace MvxNotifications.Core.ViewModels.Home
{
    public class NotificationInfoViewModel: MvxViewModel
    {
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
        public static NotificationInfoViewModel ToViewModel(this NotificationInfo info, bool isRead = false)
        {
            return new NotificationInfoViewModel
            {
                Id = info.Id,
                Title = info.Title,
                SubTitle = info.SubTitle,
                Message = info.Message,
                IsRead = isRead
            };
        }
    }
}