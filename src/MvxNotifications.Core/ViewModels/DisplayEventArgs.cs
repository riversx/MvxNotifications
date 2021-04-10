using System;

namespace MvxNotifications.Core
{
    public class DisplayAlertEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string AcceptText { get; set; }
    }

    public class DisplayQuestionEventArgs : EventArgs
    {
        public string Title { get; set; }
        public string Question { get; set; }
        public string AcceptText { get; set; }
        public string CancelText { get; set; }
    }
}
