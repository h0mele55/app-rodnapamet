using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public partial class ItemPreviewPage : ContentPage
    {
        ItemDetailViewModel context;
        public ItemPreviewPage(Guid newGuid)
        {
            InitializeComponent();
            context = new ItemDetailViewModel(this);
            context.ItemId = newGuid.ToString();
            BindingContext = context;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);
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
            App.Current.MainPage = new RecordingsPage();
        }
    }
}