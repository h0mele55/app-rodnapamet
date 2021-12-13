using RodnaPamet.ViewModels;
using System;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public partial class BeliefsPage : ContentPage
    {
        private static INotificationManager NotificationManager;
        private AboutViewModel viewModel;
        public BeliefsPage()
        {
            InitializeComponent();
            viewModel = new AboutViewModel(this);
            viewModel.Title = "Верую";
            BindingContext = viewModel;
            ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new AboutPage();
            return true;
        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }

        private void Record_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AudioChooserPage();
        }

        private void Records_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RecordingsPage();
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }
    }
}