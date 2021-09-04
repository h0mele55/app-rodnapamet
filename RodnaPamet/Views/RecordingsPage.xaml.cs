using RodnaPamet.Models;
using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecordingsPage : ContentPage
    {
        ItemsViewModel _viewModel;
        public RecordingsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ItemsViewModel(this);
            NoRecordsLabel.IsVisible = false;

            ListContainer.Padding = new Thickness(0, App.HeaderSizeNoFix, 0, App.FooterSize);
        }

        protected override void OnAppearing()
        {
            if (BindingContext == null)
            {
                BindingContext = _viewModel = new ItemsViewModel(this);
            }
            base.OnAppearing();
            _viewModel.OnAppearing();
            NoRecordsLabel.IsVisible = _viewModel.Items.Count == 0;
            ItemsListView.IsVisible = _viewModel.Items.Count != 0;
        }
        protected override bool OnBackButtonPressed()
        {
            App.Current.MainPage = new AboutPage();
            return true;
        }

        private void SwipeItem_Invoked(object sender, EventArgs e)
        {
        }
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
        private void Back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }
    }
}