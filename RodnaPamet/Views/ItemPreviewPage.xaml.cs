using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.ComponentModel;
using System.IO;
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
            ContentStack.Padding = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
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
            App.Current.MainPage = new RecordingsPage();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (File.Exists(context.Filename))
                File.Delete(context.Filename);
            await context.DataStore.DeleteItemAsync(context.Id);
            App.Current.MainPage = new RecordingsPage();
        }
    }
}