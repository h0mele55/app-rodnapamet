using Foundation;
using AVFoundation;

namespace RodnaPamet.iOS
{
    public class iOSVideoRecorderDelegate: AVCaptureFileOutputRecordingDelegate
	{
		public iOSVideoRecorderDelegate()
		{
		}
		public override void DidStartRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections)
		{
			//			throw new System.NotImplementedException ();
		}

		public override void FinishedRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections, NSError error)
		{
			//			throw new NotImplementedException ();
		}
	}
}
