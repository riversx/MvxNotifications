using System;
using MvvmCross.Base;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using MvvmCross.ViewModels;
using MvxNotifications.Core;
using MvxNotifications.Core.ViewModels;
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

            CreateBindingSet().Bind(this)
                .For(view => view.DeleteInteraction).To(viewModel => viewModel.RequestDeleteInteraction)
                .OneWay()
                .Apply();

            CreateBindingSet().Bind(this)
                .For(view => view.OpenInteraction).To(viewModel => viewModel.RequestOpenInteraction)
                .OneWay()
                .Apply();
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



        #region interactions

        private IMvxInteraction<YesNoQuestion> _deleteInteraction;
        public IMvxInteraction<YesNoQuestion> DeleteInteraction
        {
            get => _deleteInteraction;
            set
            {
                if (_deleteInteraction != null)
                    _deleteInteraction.Requested -= OnDisplayQuestionAsync;

                _deleteInteraction = value;
                _deleteInteraction.Requested += OnDisplayQuestionAsync;
            }
        }

        private async void OnDisplayQuestionAsync(object sender, MvxValueEventArgs<YesNoQuestion> eventArgs)
        {
            YesNoQuestion question = eventArgs.Value;
            var answer = await DisplayAlert(question.Title, question.Question, question.AcceptText, question.CancelText);
            question.YesNoCallback(answer);
        }


        private IMvxInteraction<AlertMessage> _openInteraction;
        public IMvxInteraction<AlertMessage> OpenInteraction
        {
            get => _openInteraction;
            set
            {
                if (_openInteraction != null)
                    _openInteraction.Requested -= OnDisplayAlertAsync;

                _openInteraction = value;
                _openInteraction.Requested += OnDisplayAlertAsync;
            }
        }

        private async void OnDisplayAlertAsync(object sender, MvxValueEventArgs<AlertMessage> eventArgs)
        {
            AlertMessage openInteraction = eventArgs.Value;
            await DisplayAlert(openInteraction.Title, openInteraction.Message, openInteraction.AcceptText);
            openInteraction.OkCallback();
        }

        #endregion interactions 

    }
}
