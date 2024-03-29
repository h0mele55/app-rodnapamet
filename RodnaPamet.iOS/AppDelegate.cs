﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using UIKit;
using UserNotifications;

namespace RodnaPamet.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("Swipe-View_Experimental");
            global::Xamarin.Forms.Forms.SetFlags("SwipeView_Experimental");
            global::Xamarin.Forms.Forms.Init();

            try
            {

                FormsVideoPlayer.Init();

                UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();

                LoadApplication(new App());

            }
            catch (Exception ex)
            {
            }

            var settings = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;
            if (settings == UIUserNotificationType.None)
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (approved, error) => {
                });
            }

            UITextAttributes txtAttributes = new UITextAttributes
            {
                Font = UIFont.FromName("Alegreya", 10.0F)
            };
            UITabBarItem.Appearance.SetTitleTextAttributes(txtAttributes, UIControlState.Normal);
            UINavigationBar.Appearance.SetTitleTextAttributes(txtAttributes);
            return base.FinishedLaunching(app, options);
        }
    }
}
