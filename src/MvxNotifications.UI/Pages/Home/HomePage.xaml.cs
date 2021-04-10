using System;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvxNotifications.Core;
using MvxNotifications.Core.ViewModels.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvxNotifications.UI.Pages.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [MvxContentPagePresentation(WrapInNavigationPage = true)]
    public partial class HomePage : MvxContentPage<HomeViewModel>, IDisposable
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.DisplayAlert += ViewModel_DisplayAlert;
            ViewModel.DisplayQuestion += ViewModel_DisplayQuestion;
        }

        public void Dispose()
        {
            if (ViewModel != null)
            {
                ViewModel.DisplayAlert -= ViewModel_DisplayAlert;
                ViewModel.DisplayQuestion -= ViewModel_DisplayQuestion;
            }
        }

        private void ViewModel_DisplayAlert(object sender, DisplayAlertEventArgs e)
        {
            _ = DisplayAlert(e.Title, e.Message, e.AcceptText);
        }

        private void ViewModel_DisplayQuestion(object sender, DisplayQuestionEventArgs e)
        {
            _ = DisplayAlert(e.Title, e.Question, e.AcceptText, e.CancelText);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Application.Current.MainPage is NavigationPage navigationPage)
            {
                navigationPage.BarTextColor = Color.White;
                navigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PrimaryColor"];
            }
        }
    }
}
