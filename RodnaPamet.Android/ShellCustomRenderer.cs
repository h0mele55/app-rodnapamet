using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using RodnaPamet;
using RodnaPamet.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomShell), typeof(ShellCustomRenderer))]
namespace RodnaPamet.Droid
{
    public class ShellCustomRenderer : ShellRenderer
    {
        public ShellCustomRenderer(Context context) : base(context)
        {
        }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new CustomToolbarAppearanceTracker();
        }
    }
    public class CustomToolbarAppearanceTracker : IShellToolbarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {

        }

        public void SetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            toolbar.SetBackgroundResource(Resource.Drawable.topback);
        }
    }
}