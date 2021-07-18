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
    public partial class CameraChooserPage : ContentPage, IQueryAttributable
    {
        CameraChooserViewModel viewModel;
        public CameraChooserPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CameraChooserViewModel(this);
            ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        public CameraChooserPage(string type)
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((App)App.Current).GetAllPermissions();
            viewModel.OnAppearing();
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
            App.Current.MainPage = new CameraChooserPage();
        }

        private void Records_Clicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;
            App.Current.MainPage = new RecordingsPage();
        }
    }
}