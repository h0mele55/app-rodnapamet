using System;
using System.Diagnostics;
using Foundation;
using RodnaPamet.Models;
using RodnaPamet.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(RodnaPamet.iOS.iOSUploadService))]
namespace RodnaPamet.iOS
{
    public class iOSUploadService : IUploadService
    {
        public NSUrlSession session = null;
        public iOSUploadService()
        {
            NSUrlSessionConfiguration configuration =
                NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(@"com.RodnaPamet.BackgroundSession");
            configuration.AllowsCellularAccess = true;
            configuration.NetworkServiceType = NSUrlRequestNetworkServiceType.Default;
            configuration.HttpMaximumConnectionsPerHost = 2;
            try
            {
                session = NSUrlSession.FromConfiguration
                    (configuration, new MySessionDelegate(), null);
            }
            catch (Exception ex)
            {
            }
        }

        public bool UploadFile(Page Page, Item file)
        {
            var appDel = UIApplication.SharedApplication.Delegate as AppDelegate;
            appDel.AddFileToUpload(file);
            Console.WriteLine("Added " + file.Subject + " to upload list");
            appDel.PrepareUpload();
            return true;
            string UploadURLString = Constants.AudioUrl;
            NSUrlSessionUploadTask uploadTask;
            NSUrl uploadFile = NSUrl.FromFilename(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), file.Filename));

            NSUrl uploadURL = NSUrl.FromString(UploadURLString);
            NSUrlRequest request = NSUrlRequest.FromUrl(uploadURL);
            uploadTask = session.CreateUploadTask(request, uploadFile);
            try
            {
                uploadTask.Resume();
                Debug.WriteLine("Starting download. State of task: '{0}'. ID: '{1}'", uploadTask.State, uploadTask.TaskIdentifier);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return true;
        }

        public class MySessionDelegate : NSObject, INSUrlSessionTaskDelegate
        {
            private object progress;

            public void DidFinishUploading(NSUrlSession session, NSUrlSessionUploadTask downloadTask, NSUrl location)
            {
                Debug.WriteLine(string.Format("Did finish uploading: {0}", downloadTask));
                InvokeOnMainThread(() =>
                {
                    // update UI with progress bar, if desired
                });
            }

            public void DidWriteData(NSUrlSession session, NSUrlSessionUploadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
            {
                Debug.WriteLine(string.Format("DownloadTask: {0}  progress: {1}", downloadTask, progress));
                InvokeOnMainThread(() =>
                {
                    // update UI with progress bar, if desired
                });
            }
        }
    }
}