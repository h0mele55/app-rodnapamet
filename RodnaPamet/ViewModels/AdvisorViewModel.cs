using RodnaPamet.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class AdvisorViewModel : BaseViewModel
    {
        public string VersionCode { get; set; }
        public AdvisorViewModel(IAnimatable cont) : base(cont)
        {
            Title = "Съветникь";
            OpenAdvisorValuesCommand = new Command(async () => App.Current.MainPage = new AdvisorValuesPage());
            OpenAdvisorStoryCommand = new Command(async () => App.Current.MainPage = new AdvisorStoryPage());
        }

        public ICommand OpenAdvisorValuesCommand { get; }
        public ICommand OpenAdvisorStoryCommand { get; }
    }
}