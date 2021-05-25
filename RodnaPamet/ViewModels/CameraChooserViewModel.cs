using RodnaPamet.Models;
using RodnaPamet.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class CameraChooserViewModel : BaseViewModel
    {
        public Command SelectValuesCommand { get; }
        public Command SelectStoryCommand { get; }
        public CameraChooserViewModel(IAnimatable cont) : base(cont)
        {

            Title = "Записъ";
            SelectStoryCommand = new Command(async () => await ExecuteStoryCommand());
            SelectValuesCommand = new Command(async () => await ExecuteValuesCommand());
        }
        async Task ExecuteValuesCommand()
        {
            Device.BeginInvokeOnMainThread(() => {
                IsBusy = true;
                App.Current.MainPage = new CameraPage("values");
            });
            //await Shell.Current.GoToAsync($"CameraPage?type=values");
        }
        async Task ExecuteStoryCommand()
        {
            Device.BeginInvokeOnMainThread(() => {
                IsBusy = true;
                App.Current.MainPage = new CameraPage("story");
            });
            //await Shell.Current.GoToAsync($"CameraPage?type=story");
        }
        public void OnAppearing()
        {
            IsBusy = false;

        }
        public void OnDisappearing()
        {
            IsBusy = false;
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
            App.Current.MainPage = new ItemDetailPage(new Guid(item.Id));
        }
    }
}
