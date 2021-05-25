using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RodnaPamet.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ContentPage), typeof(MyContentPageRenderer))]
namespace RodnaPamet.Droid
{
  class MyContentPageRenderer : PageRenderer
    {
        public MyContentPageRenderer() : base()
        {
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);


        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);



            float pixWidth = (float)Resources.DisplayMetrics.WidthPixels;
            float fdpWidth = (float)App.Current.MainPage.Width;
            float pixPerDp = pixWidth / fdpWidth;

            //this.Element.Padding = new Thickness(0, MainActivity.StatusBarHeight / pixPerDp, 0, 0);
            
        }
    }
}