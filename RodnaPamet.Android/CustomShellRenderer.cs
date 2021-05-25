using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Internal;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using RodnaPamet;
using RodnaPamet.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomShell), typeof(ShellCustomRenderer))]
namespace RodnaPamet.Droid
{
    public class ShellCustomRenderer : ShellRenderer
    {
        private Context context;
        public ShellCustomRenderer(Context cont) : base(cont)
        {
            context = cont;
        }
        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new ToolbarAppearance();
        }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new TabbarAppearance(this, shellItem);
        }

        protected override IShellTabLayoutAppearanceTracker CreateTabLayoutAppearanceTracker(ShellSection shellSection)
        {
            return new TabLayoutAppearance(this);
        }
    }

    public class TabLayoutAppearance : ShellTabLayoutAppearanceTracker
    {
        public TabLayoutAppearance(IShellContext shellSection) : base(shellSection)
        { 
            
        }
        public override void ResetAppearance(TabLayout tabLayout)
        {
            base.ResetAppearance(tabLayout);
            tabLayout.SetClipChildren(true);
        }

        public override void SetAppearance(TabLayout tabLayout, ShellAppearance appearance)
        {
            base.SetAppearance(tabLayout, appearance);
            tabLayout.SetClipChildren(true);
            tabLayout.TabIndicatorFullWidth = false;
        }

        protected override void SetColors(TabLayout tabLayout, Color foreground, Color background, Color title, Color unselected)
        {
            base.SetColors(tabLayout, foreground, background, title, unselected);
        }
    }

    public class TabbarAppearance : ShellBottomNavViewAppearanceTracker
    {
        IShellContext context;
        BottomNavigationView lastBottomView;
        public TabbarAppearance(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem)
        {
            context = shellContext;

            //MessagingCenter.Subscribe<string>("TabBarRendered", "Yes", (value) => {
            //    this.ResetAppearance(lastBottomView);
            //});
        }
        public new void Dispose()
        {
            base.Dispose();
        }

        public override void ResetAppearance(BottomNavigationView bottomView)
        {
            base.ResetAppearance(bottomView);
            changeColor(bottomView);
        }
        public override void SetAppearance(BottomNavigationView bottomView, IShellAppearanceElement appearance)
        {
            base.SetAppearance(bottomView, appearance);
            changeColor(bottomView, appearance);

            if (!(bottomView.GetChildAt(0) is ViewGroup layout))
                return;

            //if (!(layout.GetChildAt(1) is BottomNavigationView bottomNavigationView))
            //    return;

            var bottomNavMenuView = bottomView.GetChildAt(0) as BottomNavigationMenuView;

            for (int i = 0; i < bottomNavMenuView.ChildCount; i++)
            {
                var item = bottomNavMenuView.GetChildAt(i) as BottomNavigationItemView;
                var lp = new FrameLayout.LayoutParams(60, 60);
                item.LayoutParameters = lp;
                item.SetPadding(0, 0, 0, 0);
                //item.ClipToOutline = true;
                //item.ClipBounds = new Android.Graphics.Rect(0, 0, 60, 60);
                var itemTitle = item.GetChildAt(1);

                var smallTextView = ((TextView)((BaselineLayout)itemTitle).GetChildAt(0));
                var largeTextView = ((TextView)((BaselineLayout)itemTitle).GetChildAt(1));

                //smallTextView.SetTextSize(Android.Util.ComplexUnitType.Sp, 8);// this is unselected textview size
                //largeTextView.SetTextSize(Android.Util.ComplexUnitType.Sp, 8); //this is selected textview size
            }

            /*
                        var metrics = bottomView.Resources.DisplayMetrics;
                        var width = metrics.WidthPixels;
                        floatingActionButton = new FloatingActionButton(bottomView.Context);
                        var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        ViewGroup.LayoutParams paramss = new ViewGroup.LayoutParams(
                            ViewGroup.LayoutParams.WrapContent,
                            ViewGroup.LayoutParams.WrapContent
                        );
                        ViewGroup.MarginLayoutParams marginLayout = new ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                        marginLayout.SetMargins((int)(width / 2.3), 20, 0, 40);

                        floatingActionButton.LayoutParameters = marginLayout;
                        floatingActionButton.SetImageResource(Resource.Drawable.heart);
                        floatingActionButton.SetForegroundGravity(GravityFlags.Center);
                        floatingActionButton.SetExpanded(true);
                        floatingActionButton.Elevation = 6;

                        BottomNavigationMenuView bottomNavigationView = bottomView.GetChildAt(0) as BottomNavigationMenuView;
                        bottomView.AddView(floatingActionButton);
            */

        }

        void changeColor(BottomNavigationView view, IShellAppearanceElement appearance = null)
        {
            IMenu myMenu = view.Menu;

            IMenuItem myItemOne = myMenu.GetItem(0);
            /*
            if (myItemOne.IsChecked)
            {
                myItemOne.SetIcon(Resource.Drawable.tab_about);
            }
            else
            {
                myItemOne.SetIcon(Resource.Drawable.tab_feed);
            }
            */
            //The same logic if you have myItemTwo, myItemThree....

            if (appearance != null)
            {
                view.ItemIconTintList = null;
                //method 1
                var tl = new Android.Content.Res.ColorStateList(
                                     new int[][]{
                                      new int[]{ -Android.Resource.Attribute.StateChecked },
                                      new int[]{ Android.Resource.Attribute.StateChecked }
                                     },
                                     new int[]{
                                      appearance.EffectiveTabBarUnselectedColor.ToAndroid(),
                                      appearance.EffectiveTabBarForegroundColor.ToAndroid()
                                     });
                view.ItemTextColor = tl;
                view.ForegroundTintList = tl;
                view.ItemIconTintList = tl;
            }



            //method 2
            //view.SetBackgroundResource(Resource.Layout.tabbar_background);
            Drawable id = Android.App.Application.Context.GetDrawable(Resource.Layout.tabbar_background);
            view.Background = id;
            view.ItemIconSize = (int) (((float)DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.3f) * 0.65);

            var parameters = view.LayoutParameters;
            parameters.Height = (int)((float) DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density * 0.3f);
            view.LayoutParameters = parameters;
        }
    }

    public class ToolbarAppearance : IShellToolbarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {
            //toolbar.SetBackgroundColor(Android.Graphics.Color.Red);
            //toolbar.SetNavigationIcon(Resource.Layout.statusbar_background);// Resource.Drawable.star_small;

            //Color color = Color.FromHex("#80ff0000");
            //toolbar.SetBackgroundColor(color.ToAndroid());
            toolbar.SetPadding(0, 40, 0, 10);
        }

        public void SetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            //toolbar.SetBackgroundColor(Android.Graphics.Color.Red);
            //toolbar.SetNavigationIcon(Resource.Layout.statusbar_background);
            toolbar.SetMinimumHeight(60);
            toolbar.Background = Android.App.Application.Context.GetDrawable(Resource.Layout.statusbar_background);
        }
    }
}