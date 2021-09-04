using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using RodnaPamet.Models;
using RodnaPamet.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.Droid
{
    [Service(IsolatedProcess=true, Name="app.RodnaPamet.Service", Label="РоднаПаметъ", Icon="@drawable/icon_about")]
    public class IntentHelper : IntentService
    {
        public override void OnCreate()
        {
            base.OnCreate();
            //StartForeground();
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
        public override void OnStart(Intent intent, int startId)
        {
            base.OnStart(intent, startId);
            string guid = intent.GetStringExtra("UploadId");

            MockDataStore DataStore = DependencyService.Get<MockDataStore>();
            Item item = DataStore.GetItemAsync(guid).Result;
            if (item != null)
            {
                Task.Run(async() => {
                    UploadHelper.DoFileToUpload(item);
                    return true;
                });
            }
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
/*            string guid = intent.GetStringExtra("UploadId");

            MockDataStore DataStore = DependencyService.Get<MockDataStore>();
            Item item = DataStore.GetItemAsync(guid).Result;
            if (item != null)
            {
                UploadHelper.DoFileToUpload(item);
            }
*/
            return base.OnStartCommand(intent, flags, startId);
        }

        protected override void OnHandleIntent(Intent intent)
        {
            //throw new NotImplementedException();
        }
    }
}