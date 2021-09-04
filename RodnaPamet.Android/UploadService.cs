using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using RodnaPamet.Models;
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
    //[Service(IsolatedProcess = true)]
    //[IntentFilter(new String[] { "com.rodnapamet.UploadService" })]
    class UploadService : IUploadService
    {
        public bool UploadFile(Page Page, Item Item)
        {
            Intent downloadIntent = new Intent(Android.App.Application.Context, typeof(IntentHelper));
            downloadIntent.PutExtra("UploadId", Item.Id.ToString());
            ComponentName name = Android.App.Application.Context.StartService(downloadIntent);
            return true;
        }
    }
}