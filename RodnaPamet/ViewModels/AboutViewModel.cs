using RodnaPamet.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public string VersionCode { get; set; }
        public AboutViewModel(IAnimatable cont) : base(cont)
        {
            Title = "Родна паметь";
            OpenWebCommand = new Command(async () => App.Current.MainPage = new AudioChooserPage());
            OpenRecordsCommand = new Command(async () => App.Current.MainPage = new RecordingsPage());
            OpenBeliefsCommand = new Command(async () => App.Current.MainPage = new BeliefsPage());
            OpenAdvisorCommand = new Command(async () =>
            {
                Device.BeginInvokeOnMainThread(() => {
                    IsBusy = true;
                });
                App.Current.MainPage = new AdvisorPage();
            });
            VersionCode = VersionTracking.CurrentVersion;
        }

        public ICommand OpenWebCommand { get; set; }
        public ICommand OpenRecordsCommand { get; }
        public ICommand OpenBeliefsCommand { get; }
        public ICommand OpenAdvisorCommand { get; }
    }
}