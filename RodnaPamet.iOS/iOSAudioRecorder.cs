using System;
using System.Diagnostics;
using System.IO;
using AVFoundation;
using Foundation;
using RodnaPamet.Views;
using UIKit;

namespace RodnaPamet.iOS
{
    public partial class iOSAudioRecorder : UIView
    {
        private NSObject observer;

        private AVPlayer player;

        private NSUrl audioFilePath;

        private AVAudioRecorder recorder;

        private Stopwatch stopwatch;

        private Status status;

        public iOSAudioRecorder(AudioPreview CrossPlatformRecorder)
        {
            //Store references of recorder and options for later use
            XamRecorder = CrossPlatformRecorder;
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

        public AudioPreview XamRecorder { get; private set; }

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

        private string FileName;

        public void StartRecording(object sender)
        {
            Console.WriteLine("Begin Recording");

            var session = AVAudioSession.SharedInstance();
            session.RequestRecordPermission((granted) =>
            {
                Console.WriteLine($"Audio Permission: {granted}");

                if (granted)
                {
                    session.SetCategory(AVAudioSession.CategoryRecord, out NSError error);
                    if (error == null)
                    {
                        session.SetActive(true, out error);
                        if (error != null)
                        {
                            Status = Status.PreparingError;
                        }
                        else
                        {
                            var isPrepared = PrepareAudioRecording() && recorder.Record();
                            if (isPrepared)
                            {
                                stopwatch = new Stopwatch();
                                stopwatch.Start();
                                Status = Status.Recording;
                                XamRecorder.IsRecording = true;
                            }
                            else
                            {
                                Status = Status.PreparingError;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine(error.LocalizedDescription);
                    }
                }
                else
                {
                    Console.WriteLine("YOU MUST ENABLE MICROPHONE PERMISSION");
                }
            });
        }

        public void StopRecording(object sender)
        {
            if (recorder != null)
            {
                recorder.Stop();
                stopwatch?.Stop();

                Status = Status.Recorded;

                if (!XamRecorder.IsRecording)
                {
                    throw new Exception("You can't stop recording because it's not started yet.");
                }

                XamRecorder.IsRecording = false;

                try
                {
                    var fileSize = NSFileManager.DefaultManager.GetAttributes(FileName).Size;
                    System.Diagnostics.Debug.WriteLine("Audio Recorded: {0} ({1} bytes)", FileName, fileSize);
                    //                    UIVideo.SaveToPhotosAlbum(FileName, (fileName, saveStatus) => {
                    //                        System.Diagnostics.Debug.WriteLine("Audio Copied to Photos: {0}", fileName, fileSize);

                    var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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

        private void OnDidPlayToEndTime(object sender, NSNotificationEventArgs e)
        {
            player.Dispose();
            player = null;

            Status = Status.Recorded;
        }

        public void PlayRecorded(UIButton sender)
        {
            Console.WriteLine($"Playing Back Recording {audioFilePath}");

            // The following line prevents the audio from stopping
            // when the device autolocks. will also make sure that it plays, even
            // if the device is in mute
            AVAudioSession.SharedInstance().SetCategory(AVAudioSession.CategoryPlayback, out NSError error);
            if (error == null)
            {
                Status = Status.Playing;
                player = new AVPlayer(audioFilePath);
                player.Play();
            }
            else
            {
                Status = Status.Recorded;
                Console.WriteLine(error.LocalizedDescription);
            }
        }

        private bool PrepareAudioRecording()
        {
            var result = false;

            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //var library = System.IO.Path.Combine(documents, "..", "Library");
            FileName = System.IO.Path.Combine(documents, DateTime.Now.ToString("yymmdd-hhmmss") + ".aac");

            NSUrl url = new NSUrl(FileName, false);

            NSFileManager manager = new NSFileManager();
            NSError error = new NSError();

            if (manager.FileExists(FileName))
            {
                manager.Remove(FileName, out error);
            }

            var audioSettings = new AudioSettings
            {
                SampleRate = 44100,
                NumberChannels = 1,
                AudioQuality = AVAudioQuality.High,
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,
            };

            // Set recorder parameters
            recorder = AVAudioRecorder.Create(url, audioSettings, out error);
            if (error == null)
            {
                // Set Recorder to Prepare To Record
                if (!recorder.PrepareToRecord())
                {
                    recorder.Dispose();
                    recorder = null;
                }
                else
                {
                    recorder.FinishedRecording += OnFinishedRecording;
                    result = true;
                }
            }
            else
            {
                Console.WriteLine(error.LocalizedDescription);
            }

            return result;
        }

        private void OnFinishedRecording(object sender, AVStatusEventArgs e)
        {
            /*
            recorder.Dispose();
            recorder = null;

            Console.WriteLine($"Done Recording (status: {e.Status})");
            */
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (observer != null)
            {
                observer.Dispose();
                observer = null;
            }

            if (player != null)
            {
                player.Dispose();
                player = null;
            }

            if (recorder != null)
            {
                recorder.Dispose();
                recorder = null;
            }

            if (audioFilePath != null)
            {
                audioFilePath.Dispose();
                audioFilePath = null;
            }
        }
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
