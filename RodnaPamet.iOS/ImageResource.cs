using RodnaPamet.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageResource))]
namespace RodnaPamet.iOS
{
    public class ImageResource : IImageResource
    {
        public Size GetSize(string fileName)
        {
            UIImage image = UIImage.FromFile(fileName);
            return new Size((double)image.Size.Width, (double)image.Size.Height);
        }
    }
}