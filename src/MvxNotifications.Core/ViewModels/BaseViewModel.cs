using System;
using MvvmCross.ViewModels;

namespace MvxNotifications.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel
    {
        public EventHandler<DisplayAlertEventArgs> DisplayAlert;
        public EventHandler<DisplayQuestionEventArgs> DisplayQuestion;
    }
}
