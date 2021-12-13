using System;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(RodnaPamet.iOS.iOSSettings))]
namespace RodnaPamet.iOS
{
    public class iOSSettings : ISettings
    {
        public void Open()
        {
            UIApplication.SharedApplication.OpenUrl(new NSUrl("app-settings:"));
        }
    }
}
