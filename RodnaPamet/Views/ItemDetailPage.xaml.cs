using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel context;
        ICommand VillageTapped;

        public ItemDetailPage(Guid newGuid)
        {
            InitializeComponent();

            VillageSelect.VillageSelected += VillageSelect_VillageSelected;

            context = new ItemDetailViewModel(this);
            context.ItemId = newGuid.ToString();
            context.VillageTapped = new Command(() => TapGestureRecognizer_Tapped_1(this, null));
            context.SaveCommand = new Command(execute: async () => {
                if (context.CameraMan != null &&
                        context.Description != null &&
                        context.Age != null &&
                        context.Village != null &&
                        context.CameraMan.Trim() != "" &&
                        context.Description.Trim() != "" &&
                        context.Age > 0 &&
                        context.Village.Trim() != "" &&
                        (context.IsStory == false || context.TypeDescription.Trim() != ""))
                {
                    context.SaveItemToDB();
                    UploadHelper.DoFileToUpload(this, context.Item);
                    App.Current.MainPage = new RecordingsPage();
                }
                else
                    DisplayAlert("Непълни подробности", "Моля, попълнете всички полета", "Добре");
            }/*, canExecute: () => {
                return context.CameraMan != null &&
                        context.Description != null &&
                        context.Village != null &&
                        context.CameraMan.Trim() != "" &&
                        context.Description.Trim() != "" &&
                        context.Village.Trim() != "" &&
                        (context.IsStory == false || context.TypeDescription.Trim() != "");
            }*/);

            BindingContext = context;
            ContentStack.Padding = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void VillageSelect_VillageSelected(object sender, EventArgs e)
        {
            VillageField.Text = VillageSelect.SelectedVillage;
            VillageSelect.ClearInterface();
            VillageSelect.IsVisible = false;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            VillageSelect.IsVisible = false;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            VillageSelect.IsVisible = true;
            VillageSelect.FocusInput();
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