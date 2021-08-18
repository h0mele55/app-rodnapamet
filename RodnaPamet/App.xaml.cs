using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
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
        const int smallWightResolution = 768;
        const int smallHeightResolution = 1280;

        public static UserService UserService;

        public static double HeaderFontSize = 30;
        public static double HeaderSize = 30;
        public static double BigButtonSize = 30;
        public static double SmallButtonSize = 30;
        public static double FooterIconSize = 30;
        public static double FooterSize = 30;
        public static double HeaderSizeNoFix = 30;
        public static double HeaderSizeFix = 30;

        public static bool IsStartup = true;

        public enum AppLifecycle
        { 
            Sleep,
            Resume,
            Start
        }

        public App()
        {
            InitializeComponent();
            /*
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(ItemPreviewPage), typeof(ItemPreviewPage));
            Routing.RegisterRoute(nameof(BeliefsPage), typeof(BeliefsPage));
            Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
            Routing.RegisterRoute(nameof(AdvisorPage), typeof(AdvisorPage));
            Routing.RegisterRoute(nameof(AdvisorValuesPage), typeof(AdvisorValuesPage));
            Routing.RegisterRoute(nameof(AdvisorStoryPage), typeof(AdvisorStoryPage));
            */
            UserService = new UserService("", "");

            DependencyService.Register<MockDataStore>();
            NotificationManager = DependencyService.Get<INotificationManager>();
            NotificationManager.NotificationReceived += (sender, eventArgs) =>
            {
                var evtData = (NotificationEventArgs)eventArgs;
                //ShowNotification(evtData.Title, evtData.Message);
            };

            bool IsTablet = Xamarin.Essentials.DeviceInfo.Idiom == Xamarin.Essentials.DeviceIdiom.Tablet;

            Size BigButtonSize = DependencyService.Get<IImageResource>().GetSize("redbuttonback.png");
            Size SmallButtonSize = DependencyService.Get<IImageResource>().GetSize("greenbuttonback.png");
            Size StatusBarSize = DependencyService.Get<IImageResource>().GetSize("statusbar.jpg");
            Size NavBarSize = DependencyService.Get<IImageResource>().GetSize("tabbar.jpg");
            Size StatusBarFixSize = DependencyService.Get<IImageResource>().GetSize("topbackfix.png");
            Size NavBarFixSize = DependencyService.Get<IImageResource>().GetSize("bottombackfix.png");

            double targetWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            Label Measured = new Label();
            Measured.Text = "Родна паметь";
            Measured.Style = ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.PageTitle"])[0];

            App.HeaderFontSize = DependencyService.Get<ITextSize>().GetFontSize("РоднаРпаметъ", Measured.FontFamily, 0.7 * targetWidth, 0.6 * targetWidth * StatusBarSize.Height / StatusBarSize.Width);
            App.BigButtonSize = DependencyService.Get<ITextSize>().GetFontSize("РоднаРпаметъ", Measured.FontFamily, 0.7 * targetWidth, 0.6 * targetWidth * BigButtonSize.Height / BigButtonSize.Width);
            App.SmallButtonSize = DependencyService.Get<ITextSize>().GetFontSize("РоднаРпаметъ", Measured.FontFamily, 0.7 * targetWidth, 0.6 * targetWidth * SmallButtonSize.Height / SmallButtonSize.Width);

            App.HeaderSize = targetWidth * StatusBarSize.Height / StatusBarSize.Width +
                targetWidth * StatusBarFixSize.Height / StatusBarFixSize.Width;
            App.HeaderSizeNoFix = targetWidth * StatusBarSize.Height / StatusBarSize.Width;
            App.HeaderSizeFix = targetWidth * StatusBarFixSize.Height / StatusBarFixSize.Width;
            App.FooterSize = targetWidth * NavBarSize.Height / NavBarSize.Width;
            App.FooterIconSize = App.FooterSize / 2;

            Resources["DPageTitle"] = ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.PageTitle"])[0];
            ((Style)Resources["DPageTitle"]).Setters.Add(new Setter()
            {
                Property = Label.FontSizeProperty,
                Value = (int)App.HeaderFontSize
            });
            Resources["DBigButton"] = ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.BigButton"])[0];
            ((Style)Resources["DBigButton"]).Setters.Add(new Setter()
            {   
                Property = Label.FontSizeProperty,
                Value = (int)App.HeaderFontSize//BigButtonSize
            });
            Resources["DSmallButton"] = ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.Button"])[0];
            ((Style)Resources["DSmallButton"]).Setters.Add(new Setter()
            {
                Property = Label.FontSizeProperty,
                Value = (int)App.SmallButtonSize
            });
            Resources["DBottomMenuButton"] = new Style(typeof(ImageButton));
            ((Style)Resources["DBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.HeightRequestProperty,
                Value = (int)App.FooterIconSize
            });
            ((Style)Resources["DBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.WidthRequestProperty,
                Value = (int)App.FooterIconSize
            });
            ((Style)Resources["DBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.OpacityProperty,
                Value = 0.5
            });
            Resources["DSelectedBottomMenuButton"] = new Style(typeof(ImageButton));
            ((Style)Resources["DSelectedBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.HeightRequestProperty,
                Value = (int)App.FooterIconSize
            });
            ((Style)Resources["DSelectedBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.WidthRequestProperty,
                Value = (int)App.FooterIconSize
            });
            ((Style)Resources["DSelectedBottomMenuButton"]).Setters.Add(new Setter()
            {
                Property = ImageButton.OpacityProperty,
                Value = (int)1
            });
            Resources["DHeaderFixMargin"] = new Style(typeof(ImageButton));
            ((Style)Resources["DHeaderFixMargin"]).Setters.Add(new Setter()
            {
                Property = View.MarginProperty,
                Value = new Thickness(0, App.HeaderSizeFix, 0, 0)
            });

            try
            {
                MainPage = new LoginPage();
            }
            catch (Exception ex)
            {
            }

            const string errorFileName = "Fatal.log";
            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // iOS: Environment.SpecialFolder.Resources
            var errorFilePath = Path.Combine(libraryPath, errorFileName);
            if (File.Exists(errorFilePath))
            {
                var errorMessage = File.ReadAllText(errorFilePath);
                Error error = new Error();
                error.Device = DeviceInfo.Name + ", " +
                                DeviceInfo.DeviceType.ToString() + ", " +
                                DeviceInfo.Platform + ", " +
                                DeviceInfo.Version + ", " +
                                DeviceInfo.Idiom + ", " +
                                DeviceInfo.Model + ", " +
                                DeviceInfo.Manufacturer;
                error.ErrorMessage = errorMessage;
                error.UserId = "0";
                if (App.UserService.SubscribersList.Count > 0)
                    error.UserId = UserService.SubscribersList[0].Id;
                RestService rest = new RestService(Constants.ErrorUrl);
                rest.AddItemAsync(error);
                File.Delete(errorFilePath);
            }

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
                var status = await CrossPermissions.Current.RequestPermissionsAsync(
                    new Permission[]
                    {
                        Plugin.Permissions.Abstractions.Permission.Camera,
                        Plugin.Permissions.Abstractions.Permission.Microphone,
                        Plugin.Permissions.Abstractions.Permission.Storage,
                        Plugin.Permissions.Abstractions.Permission.MediaLibrary
                    });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}