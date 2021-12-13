using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(type), "type")]
    public partial class AudioChooserPage : ContentPage, IQueryAttributable
    {
        AudioChooserViewModel viewModel;
        public AudioChooserPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AudioChooserViewModel(this);
            ContentStack.Padding = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        public AudioChooserPage(string type)
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).GetAllPermissions();
            viewModel.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new AboutPage();
            return true;
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.ContainsKey("type"))
                type = query["type"];
        }

        public string type { get; set; }
        private void Home_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            App.Current.MainPage = new AboutPage();
        }

        private void Record_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            App.Current.MainPage = new AudioChooserPage();
        }

        private void Records_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            App.Current.MainPage = new RecordingsPage();
        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }

    }
}