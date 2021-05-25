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

            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            if(File.Exists(((Item)((SwipeItem)sender).BindingContext).Filename))
                File.Delete(((Item)((SwipeItem)sender).BindingContext).Filename);
            _viewModel.Items.Remove((Item) ((SwipeItem)sender).BindingContext);
            _viewModel.DataStore.DeleteItemAsync(((Item)((SwipeItem)sender).BindingContext).Id);
            NoRecordsLabel.IsVisible = _viewModel.Items.Count == 0;
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
    }
}