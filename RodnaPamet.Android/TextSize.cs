using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using RodnaPamet.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(TextSize))]
	namespace RodnaPamet.Droid
{
    class TextSize : ITextSize
    {
		private Typeface textTypeface;

		//public static Xamarin.Forms.Size MeasureTextSize(string text, double width, double fontSize, string fontName = null)
		public double GetFontSize(string text, string fontName, double width, double height)
		{
			var textView = new TextView(global::Android.App.Application.Context);
			textView.Typeface = GetTypeface(fontName);
			textView.SetText(text, TextView.BufferType.Normal);

			double outWidth = -1;
			double outHeight = -1;
			float fontSize = 10;

			while(textView.MeasuredWidth <= width && textView.MeasuredHeight <= height)
			{
				fontSize++;
				textView.SetTextSize(ComplexUnitType.Px, (float)fontSize);

				int widthMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
					(int)width, MeasureSpecMode.AtMost);
				int heightMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(
					0, MeasureSpecMode.Unspecified);

				textView.Measure(widthMeasureSpec, heightMeasureSpec);
			}
			fontSize--;

			//return new Xamarin.Forms.Size((double)textView.MeasuredWidth, (double)textView.MeasuredHeight);
			//(double)textView.MeasuredHeight
			return fontSize;
		}

		private Typeface GetTypeface(string fontName)
		{
			if (fontName == null)
			{
				return Typeface.Default;
			}

			if (textTypeface == null)
			{
				textTypeface = Typeface.Create(fontName, TypefaceStyle.Normal);
			}

			return textTypeface;
		}
	}
}