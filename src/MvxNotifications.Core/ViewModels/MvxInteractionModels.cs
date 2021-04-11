using System;

namespace MvxNotifications.Core.ViewModels
{
    public class YesNoQuestion
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string AcceptText { get; set; }
        public string CancelText { get; set; }
        public Action<bool> YesNoCallback { get; set; }
    }
}
