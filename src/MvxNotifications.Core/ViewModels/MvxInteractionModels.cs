using System;

namespace MvxNotifications.Core.ViewModels
{
    public class YesNoQuestion
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string AcceptText { get; set; } = "Yes";
        public string CancelText { get; set; } = "No";
        public Action<bool> YesNoCallback { get; set; }
    }

    public class AlertMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string AcceptText { get; set; } = "OK";
        public Action OkCallback { get; set; }
    }
}
