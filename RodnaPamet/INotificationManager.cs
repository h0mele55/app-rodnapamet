using System;
using System.Collections.Generic;
using System.Text;

namespace RodnaPamet
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, DateTime? notifyTime = null, bool? hasProgress = false, float? progress = 0f);
        void ReceiveNotification(string title, string message);
        void UpdateNotification(long max, long current);
    }
}
