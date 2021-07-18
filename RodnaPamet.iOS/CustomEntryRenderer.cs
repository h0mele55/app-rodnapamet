﻿using CoreGraphics;
using RodnaPamet.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace RodnaPamet.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.BackgroundColor = UIColor.FromRGBA(251, 197, 93, 200);
                Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = CGColor.CreateSrgb(0, 0, 0, 255);
                Control.Layer.BorderWidth = 1;
                Control.TextColor = UIColor.Black;
                Control.TintColor = UIColor.Black;
            }
        }
    }
}