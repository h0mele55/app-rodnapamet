using CoreGraphics;
using RodnaPamet;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(BackgroundImageTabbedPageRenderer))]
namespace RodnaPamet
{
    public class BackgroundImageTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillLayoutSubviews()
        {
            base.ViewWillLayoutSubviews();

            var image = UIImage.FromBundle("tabbar.jpg");

            image = image.Scale(new CGSize(TabBar.Frame.Width, TabBar.Frame.Height));
            TabBar.BackgroundImage = image;
            TabBar.UnselectedItemTintColor = UIColor.FromRGBA(255, 255, 255, 150);
            TabBar.ShadowImage = new UIImage();
        }

    }
}