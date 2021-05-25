using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Platform.Android;
using Xamarin.Essentials;
using Plugin.Permissions;

namespace RodnaPamet.Droid
{
    [Activity(Label = "Родна паметь", Icon = "@mipmap/icon", Theme = "@style/Theme.Loader", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static int StatusBarHeight;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            this.Window.AddFlags(WindowManagerFlags.TranslucentStatus); 
            base.OnCreate(savedInstanceState);

            Forms.SetFlags("SwipeView_Experimental");
            Forms.SetFlags("CarouselView_Experimental");
            Forms.SetFlags("Shell_Experimental");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);


            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                //Window.DecorView.SystemUiVisibility = 0;
                //var statusBarHeightInfo = typeof(FormsAppCompatActivity).GetField("statusBarHeight", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                //statusBarHeightInfo.SetValue(this, 0);
                //Window.SetStatusBarColor(new Android.Graphics.Color(18, 52, 86, 255));
            }

            LoadApplication(new App());

            StatusBarHeight = getStatusBarHeight();

            CreateNotificationFromIntent(Intent);
        }
        public int getStatusBarHeight()
        {

            int statusBarHeight = 0, totalHeight = 0, contentHeight = 0;
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
            {
                statusBarHeight = Resources.GetDimensionPixelSize(resourceId);


            }
            return statusBarHeight;
        }
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(DroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(DroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
