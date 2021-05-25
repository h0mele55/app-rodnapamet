using System;
using Android.Hardware.Camera2;

namespace RodnaPamet.Camera2
{
	public class CameraCaptureStateSessionListener : CameraCaptureSession.StateCallback
	{
		public Action<CameraCaptureSession> Failed;
		public Action<CameraCaptureSession> Configured;

		public override void OnConfigured(CameraCaptureSession session)
		{
			Configured?.Invoke(session);
		}

		public override void OnConfigureFailed(CameraCaptureSession session)
		{
			Failed?.Invoke(session);
		}
	}
}
