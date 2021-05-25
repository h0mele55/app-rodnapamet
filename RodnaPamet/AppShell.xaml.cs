using RodnaPamet.ViewModels;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : CustomShell
    {
        public AppShell()
        {
            InitializeComponent();

            MainTabBar.CurrentItem = AboutTab;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MainTabBar.IsVisible = true;
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                bool res = false;
                res = await DisplayAlert("Изход", "Сигурни ли сте, че желаете да излезете от Родна Паметъ?", "Изход", "Назад");
                if(res)
                {
                    System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                }
            });
            

            return true;
        }

        public void OpenItemDetails(Guid newGuid)
        {
            MainTabBar.CurrentItem = RecordingsTab;
            Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={newGuid}");
        }
    }
}