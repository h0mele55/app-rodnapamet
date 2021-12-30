using System;
using RodnaPamet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using RodnaPamet.iOS;

[assembly: ExportRenderer(typeof(AudioPreview), typeof(iOSAudioRecorderRenderer))]
namespace RodnaPamet.iOS
{
	public class iOSAudioRecorderRenderer : ViewRenderer<AudioPreview, iOSAudioRecorder>
	{
		iOSAudioRecorder recorder;

		protected override void OnElementChanged(ElementChangedEventArgs<AudioPreview> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
				return;

			if (Control == null)
			{
				recorder = new iOSAudioRecorder(e.NewElement);//e.NewElement, e.NewElement.Orientation
				SetNativeControl(recorder);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe
				e.NewElement.StartRecording = null;
				e.NewElement.StopRecording = null;
				//e.OldElement.OnStartRecording -= OnStartRecording; //unsubscribe from start recording event in xamarin.forms control
				//e.OldElement.OnStopRecording -= OnStopRecording; //unsubscribe from stop recording event in xamarin.forms control
				//e.OldElement.OnStopPreviewing -= OnStopPreviewing;
				//e.OldElement.OnStartPreviewing -= OnStartPreviewing;
				//e.OldElement.OnDoCleanup -= OnDoCleanup;

			}
			if (e.NewElement != null)
			{
				// Subscribe
				e.NewElement.StartRecording = new Command(() => OnStartRecording(this, null));
				e.NewElement.StopRecording = new Command(() => OnStopRecording(this, null));
				//e.NewElement.OnStartRecording += OnStartRecording; //subscribe from start recording event in xamarin.forms control
				//e.NewElement.OnStopRecording += OnStopRecording;//subscribe from stop recording event in xamarin.forms control
				//e.NewElement.OnStartPreviewing += OnStartPreviewing;
				//e.NewElement.OnStopPreviewing += OnStopPreviewing;
				//e.NewElement.OnDoCleanup += OnDoCleanup;
				//recorder.StartPreviewing(this, e);
				//cameraPreview.Click += OnCameraPreviewClicked;
			}
		}

		void OnStartRecording(object sender, EventArgs e)
		{
			recorder.StartRecording(sender);
		}
		void OnStopRecording(object sender, EventArgs e)
		{
			recorder.StopRecording(sender);
		}
		void OnStartPreviewing(object sender, EventArgs e)
		{
			//recorder.StartPreviewing(sender, e);
		}
		void OnStopPreviewing(object sender, EventArgs e)
		{
			//recorder.StopPreviewing(sender, e);
		}

		void OnDoCleanup(object sender, EventArgs e)
		{
			//recorder.DoCleanup(sender, e);
		}



		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}