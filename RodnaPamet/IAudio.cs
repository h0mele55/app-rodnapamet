using System;
namespace RodnaPamet
{
    public interface IAudio
    {
        void SetAudioFile(string fileName);
        void PlayAudioFile();
        void PauseAudioFile();
        void StopAudioFile();
        double GetDuration();
        void PlayAtPosition(double position);
        event EventHandler ProgressChanged;
        double GetPosition();
    }

    public class PositionChanged : EventArgs
    {
        public PositionChanged(double position)
        {
            Position = position;
        }
        public double Position { get; set; }
    }
}
