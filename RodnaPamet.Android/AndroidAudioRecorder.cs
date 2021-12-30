using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace RodnaPamet.Droid
{
    public partial class AndroidAudioRecorder : View
    {
        public AudioPreview XamRecorder { get; private set; }
        private MediaRecorder recorder;
        private Stopwatch stopwatch;
        private Status status;

        public AndroidAudioRecorder(Context c) : base(c)
        {
            recorder = new MediaRecorder();
        }
        public void SetPreview(AudioPreview CrossPlatformRecorder)
        {
            XamRecorder = CrossPlatformRecorder;
        }
        public void StartRecording(object sender)
        {
            Console.WriteLine("Start Recording");

            var isPrepared = PrepareAudioRecording();
            if (isPrepared)
            {
                recorder.Start(); // Recording state.
                stopwatch = new Stopwatch();
                stopwatch.Start();
                Status = Status.Recording;
                XamRecorder.IsRecording = true;
                CrossCurrentActivity.Current.Activity.Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            }
            else
            {
                Status = Status.PreparingError;
            }
        }

        public void StopRecording(object sender)
        {
            if (recorder != null)
            {
                try
                {
                    recorder.Stop();
                }
                catch (Exception ex) { }
                stopwatch?.Stop();
                CrossCurrentActivity.Current.Activity.Window.SetFlags(WindowManagerFlags.AllowLockWhileScreenOn, WindowManagerFlags.AllowLockWhileScreenOn);

                Status = Status.Recorded;

                if (!XamRecorder.IsRecording)
                {
                    throw new Exception("You can't stop recording because it's not started yet.");
                }

                XamRecorder.IsRecording = false;

                try
                {
                    //var fileSize = NSFileManager.DefaultManager.GetAttributes(FileName).Size;
                    //System.Diagnostics.Debug.WriteLine("Audio Recorded: {0} ({1} bytes)", FileName, fileSize);
                    //                    UIVideo.SaveToPhotosAlbum(FileName, (fileName, saveStatus) => {
                    //                        System.Diagnostics.Debug.WriteLine("Audio Copied to Photos: {0}", fileName, fileSize);

                    var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    //var library = System.IO.Path.Combine(documents, "..", "Library");
                    string fileName = FileName.Substring(documents.Length + 1);
                    XamRecorder.AudioTaken(fileName);
                    //                    });
                }
                catch (Exception ex)
                {

                }
            }
        }

        private bool PrepareAudioRecording()
        {
            var result = false;

            var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //var library = System.IO.Path.Combine(documents, "..", "Library");
            FileName = System.IO.Path.Combine(documents, DateTime.Now.ToString("yymmdd-hhmmss") + ".aac");

            try
            {
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                if (recorder == null)
                {
                    recorder = new MediaRecorder(); // Initial state.
                }
                else
                {
                    recorder.Reset();
                    recorder.SetAudioSource(AudioSource.Mic);
                    recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                    recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                    // Initialized state.
                    recorder.SetOutputFile(FileName);
                    // DataSourceConfigured state.
                    recorder.Prepare(); // Prepared state
                    recorder.Info += Recorder_Info;
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }

            return result;
        }

        private void Recorder_Info(object sender, MediaRecorder.InfoEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void UpdateUserInterface()
        {
            switch (status)
            {
                case Status.PreparingError:
                    /*
                    statusLabel.Text = "Error preparing";
                    lenghtButton.Text = string.Empty;
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    playButton.Enabled = false;
                    */
                    break;

                case Status.Playing:
                    /*
                    statusLabel.Text = "Playing";
                    startButton.Enabled = false;
                    stopButton.Enabled = false;
                    playButton.Enabled = false;
                    */
                    break;

                case Status.Recording:
                    /*
                    lenghtButton.Text = string.Empty;
                    statusLabel.Text = "Recording";
                    startButton.Enabled = false;
                    stopButton.Enabled = true;
                    playButton.Enabled = false;
                    */
                    break;

                case Status.Recorded:
                    /*
                    lenghtButton.Text = string.Format("{0:hh\\:mm\\:ss}", stopwatch.Elapsed);
                    statusLabel.Text = string.Empty;
                    startButton.Enabled = true;
                    stopButton.Enabled = false;
                    playButton.Enabled = true;
                    */
                    break;
            }
        }

        protected Status Status
        {
            get
            {
                return status;
            }

            set
            {
                if (status != value)
                {
                    status = value;
                    UpdateUserInterface();
                }
            }
        }

        public string FileName { get; private set; }
    }

    public enum Status
    {
        Unknown,
        PreparingError,
        Recording,
        Recorded,
        Playing,
    }
}