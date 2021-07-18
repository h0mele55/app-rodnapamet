using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Widget;
using RodnaPamet.Droid;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace RodnaPamet.Droid
{
    class CustomEntryRenderer : EntryRenderer
    {
        Context cont;
        TappableEntry theElement;
        public CustomEntryRenderer(Context context) : base(context)
        {
            cont = context;
        }

        void SetColor(Android.Graphics.Color color)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Control.BackgroundTintList = ColorStateList.ValueOf(color);
            }
            else
            {
                Control.Background.SetColorFilter(color, PorterDuff.Mode.SrcAtop);
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as TappableEntry;
            if (element != null && Control != null)
            {
                theElement = element;

                Control.Touch -= Control_Click;
                Control.Touch += Control_Click;
            }

            if (Control != null)
            {
                SetColor(Android.Graphics.Color.Black);

                IntPtr IntPtrtextViewClass = JNIEnv.FindClass(typeof(TextView));
                IntPtr mCursorDrawableResProperty = JNIEnv.GetFieldID(IntPtrtextViewClass, "mCursorDrawableRes", "I");
                JNIEnv.SetField(Control.Handle, mCursorDrawableResProperty, Resource.Drawable.mycursor);

                this.EditText.FocusChange += (sender, ee) => {
                    bool hasFocus = ee.HasFocus;
                    if (hasFocus)
                    {
                        SetColor(new Android.Graphics.Color(ContextCompat.GetColor(cont, Resource.Color.colorAccent)));
                    }
                    else
                    {
                        SetColor(Android.Graphics.Color.Black);
                    }
                };
            }
        }
        private void Control_Click(object sender, System.EventArgs e)
        {
            theElement?.Tap?.Execute(theElement.TapParameter);
        }
    }
}