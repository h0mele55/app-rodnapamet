using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public class AudioPreview : View
    {
        Command startRecording;
        Command stopRecording;

        public bool IsRecording { get; set; }

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

        public void AudioTaken(string fileName)
        {
            AudioFinished?.Invoke(this, new VideoEventArgs() { Filename = fileName });
        }

        public event EventHandler AudioFinished;
    }
}
