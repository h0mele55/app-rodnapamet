using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
//using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//https://hofmadresu.com/2018/09/11/android-camera2-trials-and-tribulations.html
namespace RodnaPamet.Views
{
    public partial class AboutPage : ContentPage
    {
        private static INotificationManager NotificationManager;
        private AboutViewModel viewModel;
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutViewModel(this);
        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            //            Task.Run(() => {
            //                NotificationManager = DependencyService.Get<INotificationManager>();
            //                NotificationManager.Initialize();
            //                NotificationManager.SendNotification("Видео файл", "Записва се...", null, true);
            //                Thread.Sleep(3000);
            //                NotificationManager.UpdateNotification(10, 7);
            //            });
            // MessagingCenter.Send<string>("TabBarRendered", "Yes");

            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);

            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                DisplayAlert("Родна памет - достъп до internet", "Изглежда Вашето устройство няма достъп до internet. Можете да използвате пълните възможности на приложението, като записаните видео файлове ще бъдат качени когато имате достъп до internet.", "Добре");
            }
            else
            {
                MockDataStore DataStore = DependencyService.Get<MockDataStore>();
                IEnumerable<Item> items = DataStore.GetItemsAsync(null).Result;
                foreach (Item item in items)
                {
                    if (item.InfoComplete && !item.Uploaded && !item.Uploading)
                    {
                        UploadHelper.AddFileToUpload(this, item);
                    }
                }
            }

//            foreach (String family in UIFont.FamilyNames)
//                foreach (String font in UIFont.FontNamesForFamilyName(family))
//                    Debug.WriteLine(font);
            

            const string errorFileName = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            if (File.Exists(errorFilePath))
            {
                var errorMessage = File.ReadAllText(errorFilePath);
                DisplayAlert("ГРЕШКА", errorMessage, "Добре");

                File.Delete(errorFilePath);
            }

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