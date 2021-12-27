using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using FFImageLoading.Forms.Platform;
using Foundation;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using RodnaPamet.Models;
using RodnaPamet.Services;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RodnaPamet.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //

        private List<Item> UploadFiles = new List<Item>();
        private List<SerializableKeyValuePair<int, Item>> UploadTasks = new List<SerializableKeyValuePair<int, Item>>();

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            ServicePointManager
                .ServerCertificateValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            Forms.SetFlags("CarouselView_Experimental");
            Forms.SetFlags("Shell_Experimental");
            Forms.SetFlags("SwipeView_Experimental");
            Forms.Init();

            DependencyService.Register<IAppTrackingTransparencyPermission, AppTrackingTransparencyPermission>();

            CachedImageRenderer.Init();

            try
            {

                //FormsVideoPlayer.Init();

                UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();

                var ap = new App();
                LoadApplication(ap);
//                App.HeaderSize -= UIKit.UIApplication.SharedApplication.StatusBarFrame.Height;
                App.HeaderSizeNoFix -= UIKit.UIApplication.SharedApplication.StatusBarFrame.Height;
            }
            catch (Exception ex)
            {
            }

            var settings = UIApplication.SharedApplication.CurrentUserNotificationSettings.Types;
            if (settings == UIUserNotificationType.None)
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (approved, error) => {
                });
            }

            UITextAttributes txtAttributes = new UITextAttributes
            {
                Font = UIFont.FromName("Alegreya", 10.0F)
            };
            UITabBarItem.Appearance.SetTitleTextAttributes(txtAttributes, UIControlState.Normal);
            UINavigationBar.Appearance.SetTitleTextAttributes(txtAttributes);

            return base.FinishedLaunching(app, options);
        }

        public void AddFileToUpload(Item item)
        {
            foreach (Item i in UploadFiles)
                if (i.Filename == item.Filename)
                    return;
            UploadFiles.Add(item);
        }

        public void ProcessCompletedTask(NSUrlSessionTask sessionTask)
        {
            try
            {
                Console.WriteLine(string.Format("Task ID: {0}, State: {1}, Response: {2}", sessionTask.TaskIdentifier, sessionTask.State, sessionTask.Response));

                // Make sure that we have a response to process
                if (sessionTask.Response == null || sessionTask.Response.ToString() == "")
                {
                    Console.WriteLine("ProcessCompletedTask no response...");
                }
                else
                {
                    // Get response
                    var resp = (NSHttpUrlResponse)sessionTask.Response;

                    // Check that our task completed and server returned StatusCode 201 = CREATED.
                    if (sessionTask.State == NSUrlSessionTaskState.Completed && resp.StatusCode == 200)
                    {
                        
                        for(int i = 0; i < UploadTasks.Count; i++)
                        {
                            SerializableKeyValuePair<int, Item> kvp = UploadTasks[i];
                            if (kvp.Key == (int)sessionTask.TaskIdentifier)
                            {
                                int pos = UploadTasks.IndexOf(kvp);
                                kvp.Value.Uploaded = true;
                                kvp.Value.Uploading = false;
                                MockDataStore ItemsStore = DependencyService.Get<MockDataStore>();
                                ItemsStore.UpdateItemAsync(kvp.Value);
                                UploadTasks.RemoveAt(pos);

                                Console.WriteLine("Item " + kvp.Value.Subject + " updated, task removed: {0}", kvp.Value.Id);
                                i = UploadTasks.Count + 1;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProcessCompletedTask Ex: {0}", ex.Message);
            }
        }
        public class UploadDelegate : NSUrlSessionTaskDelegate
        {
            // Called by iOS when the task finished trasferring data. It's important to note that his is called even when there isn't an error.
            // See: https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionTaskDelegate_protocol/index.html#//apple_ref/occ/intfm/NSURLSessionTaskDelegate/URLSession:task:didCompleteWithError:
            public override void DidCompleteWithError(NSUrlSession session, NSUrlSessionTask task, NSError error)
            {
                Console.WriteLine(string.Format("DidCompleteWithError TaskId: {0}{1}", task.TaskIdentifier, (error == null ? "" : " Error: " + error.Description)));

                if (error == null)
                {
                    var appDel = UIApplication.SharedApplication.Delegate as AppDelegate;
                    appDel.ProcessCompletedTask(task);
                }
            }

            // Called by iOS when session has been invalidated.
            // See: https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionDelegate_protocol/index.html#//apple_ref/occ/intfm/NSURLSessionDelegate/URLSession:didBecomeInvalidWithError:
            public override void DidBecomeInvalid(NSUrlSession session, NSError error)
            {
                Console.WriteLine("DidBecomeInvalid" + (error == null ? "undefined" : error.Description));
            }

            // Called by iOS when all messages enqueued for a session have been delivered.
            // See: https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionDelegate_protocol/index.html#//apple_ref/occ/intfm/NSURLSessionDelegate/URLSessionDidFinishEventsForBackgroundURLSession:
            public override void DidFinishEventsForBackgroundSession(NSUrlSession session)
            {
                Console.WriteLine("DidFinishEventsForBackgroundSession");
            }

            // Called by iOS to periodically inform the progress of sending body content to the server.
            // See: https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionTaskDelegate_protocol/index.html#//apple_ref/occ/intfm/NSURLSessionTaskDelegate/URLSession:task:didSendBodyData:totalBytesSent:totalBytesExpectedToSend:
            public override void DidSendBodyData(NSUrlSession session, NSUrlSessionTask task, long bytesSent, long totalBytesSent, long totalBytesExpectedToSend)
            {
                // Uncomment line below to see file upload progress outputed to the console. You can track/manage this in your app to monitor the upload progress.
                Console.WriteLine ("DidSendBodyData bSent: {0}, totalBSent: {1} totalExpectedToSend: {2}", bytesSent, totalBytesSent, totalBytesExpectedToSend);
            }
        }
        public NSUrlSession InitBackgroundSession()
        {
            // See URL below for configuration options
            // https://developer.apple.com/library/ios/documentation/Foundation/Reference/NSURLSessionConfiguration_class/index.html

            // Use same identifier for background tasks so in case app terminiated, iOS can resume tasks when app relaunches.
            string identifier = "MyBackgroundTaskId";

            using (var config = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration(identifier))
            {
                config.HttpMaximumConnectionsPerHost = 4; //iOS Default is 4
                config.TimeoutIntervalForRequest = 600.0; //30min allowance; iOS default is 60 seconds.
                config.TimeoutIntervalForResource = 120.0; //2min; iOS Default is 7 days
                return NSUrlSession.FromConfiguration(config, new UploadDelegate(), new NSOperationQueue());
            }
        }
        nint taskId;
        public override void DidEnterBackground(UIApplication application)
        {
            Console.WriteLine("DidEnterBackground called...");

            // Ask iOS for additional background time and prepare upload.
            taskId = application.BeginBackgroundTask(delegate {
                if (taskId != 0)
                {
                    application.EndBackgroundTask(taskId);
                    taskId = 0;
                }
            });
            return;

            new System.Action(async delegate {

                //await PrepareUpload("");

                application.BeginInvokeOnMainThread(delegate {
                    if (taskId != 0)
                    {
                        application.EndBackgroundTask(taskId);
                        taskId = 0;
                    }
                });

            }).BeginInvoke(null, null);
        }

        public NSUrlSession session = null;
        public Item lastItem;

        public async Task PrepareUpload()
    {
            Console.WriteLine("Found " + UploadTasks.Count + " tasks");
            try
            {
                Console.WriteLine("PrepareUpload called...");

                if (session == null)
                    session = InitBackgroundSession();

                // Check if task already exits
                var tsk = await GetPendingTask();
            if (tsk != null)
            {
                Console.WriteLine("TaskId {0} found, state: {1}", tsk.TaskIdentifier, tsk.State);

                // If our task is suspended, resume it.
                if (tsk.State == NSUrlSessionTaskState.Suspended)
                {
                    Console.WriteLine("Resuming taskId {0}...", tsk.TaskIdentifier);
                    tsk.Resume();
                }

                return; // exit, we already have a task
            }
                if (UploadFiles.Count == 0)
                {
                    UploadTasks = new List<SerializableKeyValuePair<int, Item>>();
                    return;
                }
                Item item = UploadFiles[0];
                Console.WriteLine("Working on " + item.Subject);
                var bodyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "bp");
                var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), item.Filename);
                //if (File.Exists(bodyPath))
                {
                    var boundary = "FileBoundary";
                    //((UploadDelegate)session.Delegate).

                    // Create request
                    NSUrl uploadHandleUrl = NSUrl.FromString(Constants.AudioUrl);
                NSMutableUrlRequest request = new NSMutableUrlRequest(uploadHandleUrl);
                request.HttpMethod = "POST";
                request["Content-Type"] = "multipart/form-data; boundary=" + boundary;
                request["FileName"] = Path.GetFileName(bodyPath);
                    request["FileName1"] = Path.GetFileName(bodyPath);

                    // Construct the body
                    System.Text.StringBuilder sb = new System.Text.StringBuilder("");

                    sb.AppendFormat("--{0}\r\n", boundary);
                sb.AppendFormat("Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\n", Path.GetFileName(filePath));
                sb.Append("Content-Type: application/octet-stream\r\n\r\n");

                    // Delete any previous body data file
                    if (File.Exists(bodyPath))
                    File.Delete(bodyPath);

                // Write file to BodyPart
                var fileBytes = File.ReadAllBytes(filePath);
                using (var writeStream = new FileStream(bodyPath, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    writeStream.Write(Encoding.Default.GetBytes(sb.ToString()), 0, sb.Length);
                    writeStream.Write(fileBytes, 0, fileBytes.Length);

                    sb.Clear();
                        sb.AppendFormat("\r\n--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"UserID\";\r\n\r\n{0}\r\n", App.UserService.SubscribersList[0].Id);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"VideoType\";\r\n\r\n{0}\r\n", item.Type);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"Subject\";\r\n\r\n{0}\r\n", item.Subject);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"Age\";\r\n\r\n{0}\r\n", item.Age);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"Description\";\r\n\r\n{0}\r\n", item.Description);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"Village\";\r\n\r\n{0}\r\n", item.Village);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"Operator\";\r\n\r\n{0}\r\n", item.Cameraman);
                        sb.AppendFormat("--{0}\r\n", boundary);
                        sb.AppendFormat("Content-Disposition: form-data; name=\"SubType\";\r\n\r\n{0}\r\n", item.SubDescription);
                        sb.AppendFormat("--{0}--\r\n", boundary);

                        writeStream.Write(Encoding.Default.GetBytes(sb.ToString()), 0, sb.Length);
                }
                sb = null;
                fileBytes = null;

                // Creating upload task
                var uploadTask = session.CreateUploadTask(request, NSUrl.FromFilename(bodyPath));
                Console.WriteLine("New TaskID: {0}", uploadTask.TaskIdentifier);
                    UploadTasks.Add(new SerializableKeyValuePair<int, Item>((int) uploadTask.TaskIdentifier, item));

                    // Start task
                    uploadTask.Resume();
            }/*
            else
            {
                Console.WriteLine("Upload file doesn't exist. File: {0}", bodyPath);
            }*/
        }
        catch (Exception ex)
        {
            Console.WriteLine("PrepareUpload Ex: {0}", ex.Message);
        }
    }
        private async Task<NSUrlSessionUploadTask> GetPendingTask()
        {
            NSUrlSessionUploadTask uploadTask = null;

            if (session != null)
            {
                try
                {
                    var tasks = await session.GetTasksAsync();

                    var taskList = tasks.UploadTasks;
                    if (taskList.Count() > 0)
                        uploadTask = taskList[0];
                }
                catch (Exception ex) { }
            }

            return uploadTask;
        }
    }
}
