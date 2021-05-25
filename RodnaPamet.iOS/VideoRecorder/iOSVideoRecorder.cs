using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using AVFoundation;
using CoreMedia;
using System.IO;
using RodnaPamet.Views;

namespace RodnaPamet.iOS
{
	public class iOSVideoRecorder: UIView
	{
		CameraOptions CameraOption;
		OrientationOptions OrientationOption;
		CameraPreview XamRecorder;
		int cameraId = 1;
		NSObject rotationNotifation;
		AVCaptureMovieFileOutput output;
		AVCaptureDevice device;
		AVCaptureDevice audioDevice;

		AVCaptureDeviceInput input;
		AVCaptureDeviceInput audioInput;
		AVCaptureSession session;
		AVCaptureVideoPreviewLayer previewlayer;


		public iOSVideoRecorder(CameraPreview CrossPlatformRecorder, CameraOptions cameraOption, OrientationOptions orientationOption)
		{
			//Store references of recorder and options for later use
			XamRecorder = CrossPlatformRecorder;
			CameraOption = cameraOption;
			OrientationOption = orientationOption;

			//register for rotation events
			rotationNotifation = NSNotificationCenter.DefaultCenter.AddObserver(UIDevice.OrientationDidChangeNotification, RotationCallback);

			//Init the camera
			InitCamera();
			
		}

		public bool IsCameraAvailable
		{
			get
			{
				return UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera);
			}
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			if (previewlayer != null)
			{
				previewlayer.Frame = rect;
			}
		}

		public void RotationCallback(NSNotification notification)
		{
			//Resize the preview layer to match the new bounds.
			//			Console.WriteLine ("Received notification: " + notification.ToString ());	
			if (IsCameraAvailable)
			{
				previewlayer.Frame = Bounds;
			}
		}

		private void InitCamera()
		{
			//ADD DEVICE INPUTS
			try
			{
				//If no camera avaiable, return
				if (!IsCameraAvailable)
				{
					return;
				}

				//Set up a new AV capture session
				session = new AVCaptureSession(); //Set up a new session

				//add video capture device
				var videoDevices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
				AVCaptureDevicePosition cameraPosition = (CameraOption == CameraOptions.Front) ? AVCaptureDevicePosition.Front : AVCaptureDevicePosition.Back;
				var device = videoDevices.FirstOrDefault(d => d.Position == cameraPosition); //Get the first device where the camera matches the requested camera

				if (device == null)
				{
					//use the default camera if front isn't available
					device = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
				}

				if (device == null)
				{
					return; //No device available 
				}

				input = AVCaptureDeviceInput.FromDevice(device);
				session.AddInput(input);

				//add audio capture device
				audioDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Audio);
				audioInput = AVCaptureDeviceInput.FromDevice(audioDevice);
				session.AddInput(audioInput);

			}
			catch (Exception ex)
			{
				return;
			}

			//Set up preview layer (shows what the input device sees)
			previewlayer = new AVCaptureVideoPreviewLayer(session);
			previewlayer.Frame = Bounds;


			if (OrientationOption == OrientationOptions.Landscape)
			{
				//landscape
				previewlayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeRight; //Video is recoreded upside down but oriented correctly for right handed people
																									 //previewlayer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait; //VIdeo recorded portrait, face to left
																									 //previewlayer.Connection.VideoOrientation = AVCaptureVideoOrientation.LandscapeLeft; 
			}
			else {
				//portrait
				previewlayer.Connection.VideoOrientation = AVCaptureVideoOrientation.Portrait;
			}

			output = new AVCaptureMovieFileOutput();
			long totalSeconds = 10000;
			Int32 preferredTimeScale = 30;
			CMTime maxDuration = new CMTime(totalSeconds, preferredTimeScale);
			output.MinFreeDiskSpaceLimit = 1024 * 1024;
			output.MaxRecordedDuration = maxDuration;

			if (session.CanAddOutput(output))
			{
				session.AddOutput(output);
			}

			//Resolutions available @ http://stackoverflow.com/questions/19422322/method-to-find-devices-camera-resolution-ios
			session.SessionPreset = AVCaptureSession.PresetHigh; //Widescreen (Medium is 4:3)
			Layer.AddSublayer(previewlayer);
			//session.StartRunning(); //Moved this to StartPreviewing
		}

		private string FileName;

		public void StartPreviewing(object sender, EventArgs e)
		{
			if (XamRecorder.IsPreviewing)
			{
				throw new Exception("Preview has already started.");
			}

			session.StartRunning();

			XamRecorder.IsPreviewing = true;
		}

		public void StopPreviewing(object sender, EventArgs e)
		{
			if (XamRecorder.IsRecording)
			{
				throw new Exception("You can't stop previewing while you're recording.");
			}

			if (XamRecorder.IsPreviewing)
			{
				session.StopRunning();

				XamRecorder.IsPreviewing = false;
			} 

		}

		public void StartRecording(object sender, EventArgs e)
		{
			//Make sure we're in a good state before we start recording
			if (!XamRecorder.IsPreviewing)
			{
				throw new Exception("You can't start recording until you are previewing.");
			}

			if (XamRecorder.IsRecording)
			{
				throw new Exception("You can't start recording because you are already recording.");
			}

			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var library = System.IO.Path.Combine(documents, "..", "Library");
			FileName = System.IO.Path.Combine(library, DateTime.Now.ToString("yymmdd-hhmmss") + ".mov");

			NSUrl url = new NSUrl(FileName, false);

			NSFileManager manager = new NSFileManager();
			NSError error = new NSError();

			if (manager.FileExists(FileName))
			{
				manager.Remove(FileName, out error);
			}

			if (IsCameraAvailable)
			{
				AVCaptureFileOutputRecordingDelegate avDel = new iOSVideoRecorderDelegate();
				output.StartRecordingToOutputFile(url, avDel);
			}
			else {
				//No camera available - use the sample file
				//FileInfo sample = new FileInfo("sample.mov");
				//sample.CopyTo(FileName);
			}
			XamRecorder.IsRecording = true;
		}

		public void StopRecording(object sender, EventArgs e)
		{

			if (!XamRecorder.IsRecording)
			{
				throw new Exception("You can't stop recording because it's not started yet.");
			}

			if (IsCameraAvailable)
			{
				output.StopRecording();
			}
			XamRecorder.IsRecording = false;

			try
			{
				var fileSize = NSFileManager.DefaultManager.GetAttributes(FileName).Size;
				System.Diagnostics.Debug.WriteLine("Video Recorded: {0} ({1} bytes)", FileName, fileSize);
				UIVideo.SaveToPhotosAlbum(FileName, (fileName, saveStatus) => {
					System.Diagnostics.Debug.WriteLine("Video Copied to Photos: {0}", fileName, fileSize);
					File.Delete(FileName);
					XamRecorder.VideoTaken(fileName);
				});
			}
			catch (Exception ex)
			{
				
			}

		}

		public void DoCleanup(object sender, EventArgs e)
		{
			//Nothing really to do on iOS, is there?
		}

		protected override void Dispose(bool disposing)
		{

			//remove rotation notifications
			NSNotificationCenter.DefaultCenter.RemoveObserver(rotationNotifation);

			session.Dispose();
			base.Dispose(disposing);

		}
	}
}
