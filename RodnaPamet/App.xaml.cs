using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet
{
    public partial class App : Application
    {
        public static string Lifecycle = "";
        //public MockDataStore DataStore { get; set; }
        public INotificationManager NotificationManager;
        public static bool HasCommunicationError = false;

        public static UserService UserService;
        public enum AppLifecycle
        { 
            Sleep,
            Resume,
            Start
        }

        public App()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(ItemPreviewPage), typeof(ItemPreviewPage));
            Routing.RegisterRoute(nameof(BeliefsPage), typeof(BeliefsPage));
            Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
            Routing.RegisterRoute(nameof(AdvisorPage), typeof(AdvisorPage));
            Routing.RegisterRoute(nameof(AdvisorValuesPage), typeof(AdvisorValuesPage));
            Routing.RegisterRoute(nameof(AdvisorStoryPage), typeof(AdvisorStoryPage));

            UserService = new UserService("", "");

            DependencyService.Register<MockDataStore>();
            NotificationManager = DependencyService.Get<INotificationManager>();
            NotificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                //ShowNotification(evtData.Title, evtData.Message);
            };

            try
            {
                MainPage = new AboutPage();
            }
            catch (Exception ex)
            {
            }

            //Task.Run(async () => {
                //await GetAllPermissions();
            //});

            MessagingCenter.Subscribe<LoginPage>("System", "login_success", (args) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage = new AboutPage();
                });
            });

            //if (!permissions)
            //{
                //Device.OpenUri(new Uri("app-settings:"));
            //}

        }

        public void NavigateTo()
        {
            MainPage = new CameraPage();
        }

        protected override void OnStart()
        {
            Lifecycle = "Start";
            AppLifecycle.Start.ToString();
            MessagingCenter.Send<App>(this, AppLifecycle.Start.ToString());
        }

        protected override void OnSleep()
        {
            Lifecycle = "Sleep";
            AppLifecycle.Sleep.ToString();
            MessagingCenter.Send<App>(this, AppLifecycle.Sleep.ToString());
        }

        protected override void OnResume()
        {
            Lifecycle = "Resume";
            AppLifecycle.Resume.ToString();
            MessagingCenter.Send<App>(this, AppLifecycle.Resume.ToString());
        }

        public class NotificationEventArgs : EventArgs
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }

        public async Task<bool> GetAllPermissions()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        var result = await App.Current.MainPage.DisplayAlert("Camera access needed", "App needs Camera access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                }

                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Could not access Camera", "App needs Camera access to work. Go to Settings >> App to enable Camera access ", "GOT IT");
                    return false;
                }

                status = await CrossPermissions.Current.CheckPermissionStatusAsync<MicrophonePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                    {
                        var result = await App.Current.MainPage.DisplayAlert("Microphone access needed", "App needs Microphone access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<MicrophonePermission>();
                }

                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Could not access Microphone", "App needs Microphone access to work. Go to Settings >> App to enable Microphone access ", "GOT IT");
                    return false;
                }

                status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        var result = await App.Current.MainPage.DisplayAlert("Storage access needed", "App needs Storage access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                }

                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Could not access Storage", "App needs Storage access to work. Go to Settings >> App to enable Storage access ", "GOT IT");
                    return false;
                }

                status = await CrossPermissions.Current.CheckPermissionStatusAsync<MediaLibraryPermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.MediaLibrary))
                    {
                        var result = await App.Current.MainPage.DisplayAlert("Media library access needed", "App needs Media lirary access enabled to work.", "ENABLE", "CANCEL");

                        if (!result)
                            return false;
                    }

                    status = await CrossPermissions.Current.RequestPermissionAsync<MediaLibraryPermission>();
                }

                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    await App.Current.MainPage.DisplayAlert("Could not access Media library", "App needs Media library access to work. Go to Settings >> App to enable Microphone access ", "GOT IT");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}