using Xamarin.Forms;

namespace RodnaPamet
{
	public class ShadowEffect : RoutingEffect
	{
		public bool HasShadow { get; set; }
		public float Radius { get; set; }

		public Color Color { get; set; }

		public float DistanceX { get; set; }

		public float DistanceY { get; set; }

		public ShadowEffect() : base("RodnaPamet.LabelShadowEffect")
		{
		}
	}
}