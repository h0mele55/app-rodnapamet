using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//https://hofmadresu.com/2018/09/11/android-camera2-trials-and-tribulations.html
namespace RodnaPamet.Views
{
    public partial class AdvisorValuesPage : ContentPage
    {
        private static INotificationManager NotificationManager;
        private AboutViewModel viewModel;
        public AdvisorValuesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel(this);
            viewModel.OpenWebCommand = new Command(async () => App.Current.MainPage = new CameraChooserPage("values"));
            ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new AdvisorPage();
            return true;
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
            App.Current.MainPage = new AdvisorPage();
        }
    }
}