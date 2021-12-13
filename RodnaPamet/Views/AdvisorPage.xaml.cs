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
    public partial class AdvisorPage : ContentPage
    {
        private static INotificationManager NotificationManager;
        private AdvisorViewModel viewModel;

        public AdvisorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AdvisorViewModel(this);
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