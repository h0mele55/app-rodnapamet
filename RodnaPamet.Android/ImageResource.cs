using Android.App;
using Android.Content;
using Android.Graphics;
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

[assembly: Dependency(typeof(ImageResource))]
namespace RodnaPamet.Droid
{
    public class ImageResource : Java.Lang.Object, IImageResource
    {
        public Size GetSize(string fileName)
        {
            var options = new BitmapFactory.Options
            {
                InJustDecodeBounds = true
            };

            fileName = fileName.Replace('-', '_').Replace(".png", "").Replace(".jpg", "");
            var resId = Forms.Context.Resources.GetIdentifier(fileName, "drawable", Forms.Context.PackageName);
            BitmapFactory.DecodeResource(Forms.Context.Resources, resId, options);

            return new Size((double)options.OutWidth, (double)options.OutHeight);
        }
    }
}