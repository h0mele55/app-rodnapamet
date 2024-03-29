﻿using Plugin.Permissions;
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
    public partial class TermsPage : ContentPage
    {
        private static INotificationManager NotificationManager;
        private AboutViewModel viewModel;
        public TermsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel(this);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);
        }

        private void RodnaPametLink_Tapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://rodnapamet.bg/");
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
    }
}