using CoreGraphics;
using RodnaPamet.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace RodnaPamet.iOS
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
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