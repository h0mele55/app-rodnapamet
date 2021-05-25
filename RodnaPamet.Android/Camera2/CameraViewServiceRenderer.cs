using Android.Content;
using Android.Graphics;
using RodnaPamet.Camera2;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CameraPreview = RodnaPamet.Views.CameraPreview;

[assembly: ExportRenderer(typeof(CameraPreview), typeof(CameraViewServiceRenderer))]
namespace RodnaPamet.Camera2
{
    public class CameraViewServiceRenderer : ViewRenderer<CameraPreview, CameraDroid>
	{
		private CameraDroid _camera;
        private CameraPreview _currentElement;
        private readonly Context _context;

		public CameraViewServiceRenderer(Context context) : base(context)
		{
			_context = context;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
		{
			base.OnElementChanged(e);

			_camera = new CameraDroid(Context);

            SetNativeControl(_camera);

            if (e.NewElement != null && _camera != null)
			{
                e.NewElement.StartRecording = new Command(() => StartRecording());
                e.NewElement.StopRecording = new Command(() => StopRecording());
                _currentElement = e.NewElement;
                _camera.SetCameraOption(_currentElement.Camera);
                _camera.Video += OnVideo;
            }
		}

        private void StartRecording()
        {
            _camera.StartRecording.Execute(null);
        }

        private void StopRecording()
        {
            _camera.StopRecording.Execute(null);
        }

        public void TakePicture()
        {
            _camera.LockFocus();
        }

        private void OnVideo(object sender, string filePath)
		{
           //Here you have the image byte data to do whatever you want 


            Device.BeginInvokeOnMainThread(() =>
            {
                _currentElement?.VideoTaken(filePath);
            });   
        }

        protected override void Dispose(bool disposing)
		{
			_camera.Video -= OnVideo;

			base.Dispose(disposing);
		}
	}
}
