using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using RodnaPamet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(RodnaPamet.Droid.UploadService))]
namespace RodnaPamet.Droid
{
    class UploadService : IUploadService
    {
        public bool UploadFile()
        {
            INotificationManager NotificationManager = DependencyService.Get<INotificationManager>();
            NotificationManager.SendNotification("Send", "Send body", null, true);

            int max = 100;
            int current = 0;

            //FileUpload

            /*
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                // call your method to check for notifications here
                NotificationManager.UpdateNotification(max, current);
                if (current == max)
                {
                    NotificationManager.UpdateNotification(0, 0);
                }
                current += 10;

                // Returning true means you want to repeat this timer
                return current < max;
            });
            */

            // Do the job here that tracks the progress.
            // Usually, this should be in a 
            // worker thread 
            // To show progress, update PROGRESS_CURRENT and update the notification with:
            // builder.setProgress(PROGRESS_MAX, PROGRESS_CURRENT, false);
            // notificationManager.notify(notificationId, builder.build());



            // When done, update the notification one more time to remove the progress bar


            return true;
        }
    }
}