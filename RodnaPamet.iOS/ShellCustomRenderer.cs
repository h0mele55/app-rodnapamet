using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RodnaPamet.CustomShell), typeof(RodnaPamet.iOS.CustomShellRenderer))]
namespace RodnaPamet.iOS
{
    public class CustomShellRenderer : ShellRenderer
    {
        protected override IShellSectionRenderer CreateShellSectionRenderer(ShellSection shellSection)
        {
            var renderer = base.CreateShellSectionRenderer(shellSection);
            if (renderer != null)
            {
                //(renderer as ShellSectionRenderer).NavigationBar.SetBackgroundImage(UIImage.FromFile("topback.png"), UIBarMetrics.Default);



                using (var logo = UIImage.FromBundle("statusbar.jpg"))
                {
                    var statusFrame = UIApplication.SharedApplication.StatusBarFrame;
                    var navigationFrame = (renderer as ShellSectionRenderer).NavigationBar.Frame;

                    var width = Math.Max(navigationFrame.Width, statusFrame.Width);
                    var fullArea = new CGRect(0, 0, width, navigationFrame.Height + statusFrame.Height);

                    UIGraphics.BeginImageContext(fullArea.Size);

                    var logoArea = new CGRect(
                        0,
                        0,//navigationFrame.Height + statusFrame.Height - (logo.Size.Height * width / logo.Size.Width),
                        width,
                        navigationFrame.Height + statusFrame.Height);
                    logo.Draw(logoArea);
                    var backgroundImage = UIGraphics.GetImageFromCurrentImageContext();

                    UIGraphics.EndImageContext();

                    (renderer as ShellSectionRenderer).NavigationBar.SetBackgroundImage(backgroundImage, UIBarMetrics.Default);

                    backgroundImage.Dispose();
                }
            }
            return renderer;
        }

        protected override IShellTabBarAppearanceTracker CreateTabBarAppearanceTracker()
        {
            return new MyOtherShellTabBarAppearanceTracker();
        }



        private class MyOtherShellTabBarAppearanceTracker : ShellTabBarAppearanceTracker
        {
            public override void ResetAppearance(UITabBarController controller)
            {
                base.ResetAppearance(controller);
            }

            public override void UpdateLayout(UITabBarController controller)
            {
                base.UpdateLayout(controller);
            }

            public override void SetAppearance(UITabBarController controller, ShellAppearance appearance)
            {
                base.SetAppearance(controller, appearance);
                var tabBar = controller.TabBar;
                UIImage image = UIImage.FromFile("tabbar.jpg"); // tab bar background image
                //tabBar.BackgroundImage = image;
                tabBar.ClipsToBounds = true;// if needed. If image is larger than the tab bar, the tab bar will expand to the image size. With this the image will be clipped to the tab bar size.



                using (var logo = UIImage.FromBundle("tabbar.jpg"))
                {
                    var navigationFrame = tabBar.Frame;

                    var width = Math.Max(navigationFrame.Width, 0);
                    var fullArea = new CGRect(0, 0, width, navigationFrame.Height);

                    UIGraphics.BeginImageContext(fullArea.Size);

                    var logoArea = new CGRect(
                        0,
                        0,//navigationFrame.Height + statusFrame.Height - (logo.Size.Height * width / logo.Size.Width),
                        width,
                        navigationFrame.Height);
                    logo.Draw(logoArea);
                    var backgroundImage = UIGraphics.GetImageFromCurrentImageContext();

                    UIGraphics.EndImageContext();

                    tabBar.BackgroundImage = backgroundImage;

                    backgroundImage.Dispose();
                }
            }

        }
    }
}
