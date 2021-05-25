using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel context;

        public ItemDetailPage(Guid newGuid)
        {
            InitializeComponent();

            VillageSelect.VillageSelected += VillageSelect_VillageSelected;

            context = new ItemDetailViewModel(this);
            context.ItemId = newGuid.ToString();
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
                    UploadHelper.AddFileToUpload(this, context.Item);
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, BottomNav.Height);
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
            //VillageSelect.FocusInput();
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