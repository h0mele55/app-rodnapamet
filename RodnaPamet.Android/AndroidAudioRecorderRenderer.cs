using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RodnaPamet.Droid;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(AudioPreview), typeof(AndroidAudioRecorderRenderer))]
namespace RodnaPamet.Droid
{
	using System;
	using RodnaPamet.Views;
	using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    public class AndroidAudioRecorderRenderer : ViewRenderer<AudioPreview, AndroidAudioRecorder>
		{
			AndroidAudioRecorder recorder;

			protected override void OnElementChanged(ElementChangedEventArgs<AudioPreview> e)
			{
				base.OnElementChanged(e);

				if (e.NewElement == null)
					return;

				if (Control == null)
				{
					recorder = new AndroidAudioRecorder(Context);//e.NewElement, e.NewElement.Orientation
					recorder.SetPreview(e.NewElement);
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
