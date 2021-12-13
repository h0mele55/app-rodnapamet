using System;
using System.Collections.Generic;
using System.IO;
using AVFoundation;
using Foundation;
using RodnaPamet.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSAudio))]
namespace RodnaPamet.iOS
{
    public class iOSAudio : IAudio
    {
        AVAudioPlayer player;
        NSTimer timer;
        public event EventHandler ProgressChanged;

        public iOSAudio()
        {
        }

        private double pp;
        public double PlayProgress
        {
            get => pp;
            set
            {
                pp = value;
                ProgressChanged?.Invoke(this, null);
            }
        }

        public double GetPosition()
        {
            return player.CurrentTime;
        }

        public void SetAudioFile(string fileName)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
            sFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
            NSUrl url = NSUrl.FromString(sFilePath);
            if (!File.Exists(sFilePath))
                return;
            //player = AVAudioPlayer.FromUrl(url);
            NSError err;
            player = new AVAudioPlayer(url, "Записъ", out err);
            
            player.SetVolume(1, 0);
        }

        public double GetDuration()
        {
            if (player != null)
                return player.Duration;
            return 0;
        }

        public void PauseAudioFile()
        {
            if (player != null)
                player.Pause();
            timer.Dispose();
        }

        public void PlayAtPosition(double position)
        {
            if (player != null)
            {
                timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate {
                    ProgressChanged?.Invoke(this, new PositionChanged(player.CurrentTime));
                });
                player.CurrentTime = position;
                player.FinishedPlaying += (object sender, AVStatusEventArgs e) =>
                {
                    timer.Dispose();
                };
                player.Play();
            }

        }

        public void PlayAudioFile()
        {
            timer = NSTimer.CreateRepeatingScheduledTimer(TimeSpan.FromSeconds(0.5), delegate {
                ProgressChanged?.Invoke(this, new PositionChanged(player.CurrentTime));
            });
            player.FinishedPlaying += (object sender, AVStatusEventArgs e) =>
            {
                timer.Dispose();
            };
            player.Play();
        }

        public void StopAudioFile()
        {
            if (player != null)
                player.Pause();
            player.CurrentTime = 0;
            ProgressChanged?.Invoke(this, new PositionChanged(0));
            timer.Dispose();
        }
    }
}
