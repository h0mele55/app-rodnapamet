using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using RodnaPamet.iOS;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(TextSize))]
namespace RodnaPamet.iOS
{
    public class TextSize : ITextSize
    {
        public double GetFontSize(string Text, string Font, double Width, double Height)
        {
            UIFont font = UIFont.FromName(Font, 100);
            UIStringAttributes attributes = new UIStringAttributes();
            float measuredWidth = 0;
            float measuredHeight = 0;
            float min = 10;
            float max = 1000;
            float fontSize = (max - min) / 2.0f + min;

            while (Math.Abs(max - min) > 1 && (Math.Abs(measuredWidth - Width) > 20 || Math.Abs(measuredHeight - Height) > 20))
            {
                fontSize = (max - min) / 2.0f + min;
                attributes.Font = font.WithSize(fontSize);
                CGSize temp = NSStringDrawing.GetSizeUsingAttributes((NSString)Text, attributes);
                measuredWidth = (float)temp.Width;
                measuredHeight = (float)temp.Height;
                if (measuredWidth < Width && measuredHeight < Height)
                    min = fontSize;
                else
                    max = fontSize;
            }

            if (measuredWidth > Width)
                fontSize = min;

            return fontSize;
        }
    }
}
