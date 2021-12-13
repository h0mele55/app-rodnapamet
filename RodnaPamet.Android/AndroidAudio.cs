using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RodnaPamet.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidAudio))]
namespace RodnaPamet.Droid
{
    class AndroidAudio : IAudio
    {
        public event EventHandler ProgressChanged;
        public double GetDuration()
        {
            if (player != null)
                return (double) player.Duration / 1000;
            return 0;
        }

        public double GetPosition()
        {
            if (player != null)
                return (double) player.CurrentPosition / 1000;
            return 0;
        }

        public void PauseAudioFile()
        {
            if (player != null)
                player.Pause();
        }

        public void PlayAtPosition(double position)
        {
            if (player != null)
            {
                player.SeekTo(((int)position * 1000));
                player.Start();
            }
        }

        public void PlayAudioFile()
        {
            if (player != null)
            {
                player.Start();
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    ProgressChanged?.Invoke(this, null);
                    return true; // return true to repeat counting, false to stop timer
                });
            }
        }

        protected MediaPlayer player;
        public void SetAudioFile(string fileName)
        {
            if (player == null)
            {
                player = new MediaPlayer();
            }

            player.Reset();
            player.SetDataSource(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), fileName));
            player.Prepare();
        }

        public void StopAudioFile()
        {
            if (player != null)
                player.Stop();
        }
    }
}