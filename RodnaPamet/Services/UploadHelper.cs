using Newtonsoft.Json;
using RodnaPamet.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RodnaPamet.Services
{
	public class UploadHelper
	{
		public static readonly string FileToUpload;
		public static readonly int BufferSize = 4096;
		private static INotificationManager NotificationManager;
		private static long uploadFileSize;
		private static Item lastItem;
		private static IDataStore<Item> store = DependencyService.Get<IDataStore<Item>>();

		private static WebClient client = new WebClient();
		public async static void AppendFileToUpload(Page page, Item item)
		{
			// Call interface to execute AddFileToUpload
			IUploadService UploadService = DependencyService.Get<IUploadService>();
            _ = UploadService.UploadFile(page, item);
		}
		public async static void DoFileToUpload(Item item)
		{
			var access = Connectivity.NetworkAccess;
			if (access != NetworkAccess.Internet)
			{
				await App.Current.MainPage.DisplayAlert("Родна памет - достъп до internet", "Приложението не може да качи видео файла, защото няма достъп до internet!", "Добре");
				return;
			}
			var profiles = Connectivity.ConnectionProfiles;
			if (!profiles.Contains(ConnectionProfile.WiFi))
			{
				if (!await App.Current.MainPage.DisplayAlert("Родна памет - достъп до internet", "Няма засечена WIFi връзка на Вашето устройство! Качването на видео файлове през мобилната мрежа може да доведе до увеличение на сметката Ви за мобилни услуги!", "Използвай мобилни данни", "Изчакай WiFi достъп"))
				{
					return;
				}
			}

			NotificationManager = DependencyService.Get<INotificationManager>();
			NotificationManager.Initialize();
			NotificationManager.SendNotification("Видео файл", "Записва се...", null, true, 0.1f);

			item.Uploading = true;
			await store.UpdateItemAsync(item);
			lastItem = item;

			var multipart = new MultipartFormBuilder();

			multipart.AddField("UserID", App.UserService.SubscribersList[0].Id);
			multipart.AddField("VideoType", item.Type.ToString());
			multipart.AddField("Subject", item.Subject);
			multipart.AddField("Age", item.Age.ToString());
			multipart.AddField("Description", item.Description);
			multipart.AddField("Village", item.Village);
			multipart.AddField("Operator", item.Cameraman);
			multipart.AddField("SubType", item.TypeDescription);

			var finfo = new FileInfo(item.Filename);
			if (finfo.Exists == false)
			{
				App.Current.MainPage.DisplayAlert("Грешка", "Записът е изтрит извън приложението!", "Добре");
				return;
			}
			uploadFileSize = finfo.Length;
			multipart.AddFile("file", finfo);

			client.UploadProgressChanged += Client_UploadProgressChanged;
            client.UploadDataCompleted += Client_UploadDataCompleted;
			//client.UploadFileCompleted += Client_UploadFileCompleted;

			client.UploadMultipartAsync(new Uri(Constants.UploadUrl), "POST", multipart);

			/*


						if (!await SaveVideoMetadata(item))
						{
							await page.DisplayAlert("Родна памет - достъп до internet", "Няма засечена WIFi връзка на Вашето устройство! Качването на видео файлове през мобилната мрежа може да доведе до увеличение на сметката Ви за мобилни услуги!", "Използвай мобилни данни", "Изчакай WiFi достъп");
						}
						//MainThread.BeginInvokeOnMainThread(() =>
						//{
							NotificationManager = DependencyService.Get<INotificationManager>();
							NotificationManager.Initialize();
							NotificationManager.SendNotification("Видео файл", "Записва се...", null, true, 0.1f);
						//});

						string fileName = item.Filename;
						string uploadUrl = Constants.UploadUrl;

						//var progress = new Progress<UploadBytesProgress>();
						//progress.ProgressChanged += Progress_ProgressChanged;
						item.Uploading = true;
						await store.UpdateItemAsync(item);
						lastItem = item;

						Task.Run(async () =>
						{
							var ut = await UploadHelper.CreateUploadTask(fileName, uploadUrl);//, progress
						});
			*/
		}

        private static void Client_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
			try
			{
				string res = System.Text.Encoding.Default.GetString(e.Result);

				ServerResponse resp = JsonConvert.DeserializeObject<ServerResponse>(res);

				if (resp.Success == 1)
				{
					NotificationManager.SendNotification("Видео файл", "Вашият запис е успешно качен!");

					lastItem.Uploading = false;
					lastItem.Uploaded = true;
					//await
					store.UpdateItemAsync(lastItem);
				}
				else
				{
					NotificationManager.SendNotification("Видео файл", "Качването е неуспешно. Приложението ще опита отново...");

					lastItem.Uploading = false;
					lastItem.Uploaded = true;
					//await
					store.UpdateItemAsync(lastItem);
				}
			}
			catch (Exception ex)
			{
				NotificationManager.SendNotification("Видео файл", "Качването е неуспешно. Приложението ще опита отново...");

				lastItem.Uploading = false;
				lastItem.Uploaded = false;
				//await
				store.UpdateItemAsync(lastItem);
			}
		}

        private static void Progress_ProgressChanged(object sender, UploadBytesProgress e)
		{
			Task.Run(() => {
				if (e.TotalBytes > e.BytesSent)
					NotificationManager.UpdateNotification(e.TotalBytes, e.BytesSent);
				else
					NotificationManager.UpdateNotification(0, 0);
			});
		}

		public static async Task<int> CreateUploadTask(string fileName, string urlToUpload)//, IProgress<UploadBytesProgress> progessReporter
		{
			uploadFileSize = new FileInfo(fileName).Length;

			//client.UploadFileTaskAsync()
			client.UploadProgressChanged += Client_UploadProgressChanged;
			client.UploadFileCompleted += Client_UploadFileCompleted;

			var content = new MultipartFormDataContent();
			content.Add(new StreamContent(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)), "VideoFile", "FileName");

			// Option - get the uploaded file key and add it as an values....
		try
			{
				var responseMessage = await client.UploadFileTaskAsync(urlToUpload, fileName);
				//var response = await responseMessage.Content.ReadAsStringAsync();
			}
			catch (Exception ex)
			{
				return 0;
			}

			//urlToUpload
			return 1;
			/*
			using (var stream = await client.OpenWriteTaskAsync(urlToUpload))
			{
				var inStream = File.OpenRead(fileName);
				byte[] buffer = new byte[BufferSize];

				for (; ; )
				{
					var bytesRead = await inStream.ReadAsync(buffer, 0, buffer.Length);
					totalBytes += bytesRead;
					if (bytesRead == 0)
					{
						break;
					}
				}
				inStream = File.OpenRead(fileName);
				DateTime synced = DateTime.Now;
				for (; ; )
				{
					var bytesRead = await inStream.ReadAsync(buffer, 0, buffer.Length);
					await stream.WriteAsync(buffer, 0, bytesRead);
					if (bytesRead == 0)
					{
						await Task.Yield();
						break;
					}
					sentBytes += bytesRead;
					if (progessReporter != null)
					{
						//System.Diagnostics.Debug.WriteLine(sentBytes + " of " + totalBytes);
						UploadBytesProgress args = new UploadBytesProgress(urlToUpload, sentBytes, totalBytes);
						if ((DateTime.Now - synced).TotalSeconds > 5)
						{
							System.Diagnostics.Debug.WriteLine("notify " + sentBytes + " / " + totalBytes + " = " + ((float) sentBytes / (float) totalBytes));
							progessReporter.Report(args);
							synced = DateTime.Now;
						}
					}
				}
				if (progessReporter != null)
				{
					UploadBytesProgress args = new UploadBytesProgress(urlToUpload, 0, 0);
					System.Diagnostics.Debug.WriteLine("notify 0 / 0");
					progessReporter.Report(args);
				}
				byte[] outs = new byte[5];
				try
				{
					int i = await stream.ReadAsync(outs, 0, 5);
				}
				catch (Exception ex)
				{ 
				
				}
			}
			return sentBytes;
			*/
		}

		private static void Client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
		{
			try
			{
				string res = System.Text.Encoding.Default.GetString(e.Result);

				ServerResponse resp = JsonConvert.DeserializeObject<ServerResponse>(res);

				if (resp.Success == 1)
				{
					NotificationManager.SendNotification("Видео файл", "Вашият запис е успешно качен!");

					lastItem.Uploading = false;
					lastItem.Uploaded = true;
					//await
						store.UpdateItemAsync(lastItem);
				}
				else
				{
					NotificationManager.SendNotification("Видео файл", "Качването е неуспешно. Приложението ще опита отново...");

					lastItem.Uploading = false;
					lastItem.Uploaded = true;
					//await
					store.UpdateItemAsync(lastItem);
				}
			}
			catch (Exception ex)
			{
				NotificationManager.SendNotification("Видео файл", "Качването е неуспешно. Приложението ще опита отново...");

				lastItem.Uploading = false;
				lastItem.Uploaded = false;
				//await
				store.UpdateItemAsync(lastItem);
			}
		}

		private static void Client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
			if (uploadFileSize > e.BytesSent)
				NotificationManager.UpdateNotification(uploadFileSize, e.BytesSent);
			else
				NotificationManager.UpdateNotification(0, 0);
		}
    }
}
