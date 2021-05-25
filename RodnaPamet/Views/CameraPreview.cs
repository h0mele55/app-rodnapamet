using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public enum CameraOptions
    {
        Rear,
        Front
    }

    public enum OrientationOptions
    {
        Portrait,
        Landscape
    }

    public class CameraPreview : View
    {
        Command startRecording;
        Command stopRecording;

        public static readonly BindableProperty CameraProperty = BindableProperty.Create(
            propertyName: "Camera",
            returnType: typeof(CameraOptions),
            declaringType: typeof(CameraPreview),
            defaultValue: CameraOptions.Rear);

        public bool IsPreviewing { get; set; }
        public bool IsRecording { get; set; }

        public CameraOptions Camera
        {
            get { return (CameraOptions)GetValue(CameraProperty); }
            set { SetValue(CameraProperty, value); }
        }

        public Command StartRecording
        {
            get { return startRecording; }
            set { startRecording = value; }
        }

        public Command StopRecording
        {
            get { return stopRecording; }
            set { stopRecording = value; }
        }

        public void VideoTaken(string fileName)
        {
            VideoFinished?.Invoke(this, new VideoEventArgs() { Filename = fileName });
        }

        public event EventHandler VideoFinished;
    }
}
