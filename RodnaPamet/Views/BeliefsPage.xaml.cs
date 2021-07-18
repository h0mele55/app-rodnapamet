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
            BindingContext = viewModel = new AboutViewModel(this);
            ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void Home_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }

        private void Record_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new CameraChooserPage();
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