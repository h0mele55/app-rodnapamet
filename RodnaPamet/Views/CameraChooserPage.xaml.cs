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
            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.ContainsKey("type"))
                type = query["type"];
        }

        public string type { get; set; }
        private void Home_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }

        private void Record_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new CameraChooserPage();
        }

        private void Records_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RecordingsPage();
        }
    }
}