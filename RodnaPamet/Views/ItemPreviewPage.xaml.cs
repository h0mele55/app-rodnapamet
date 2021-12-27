using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RodnaPamet.Views
{
    public partial class ItemPreviewPage : ContentPage
    {
        ItemDetailViewModel context;
        public ItemPreviewPage(Guid newGuid)
        {
            InitializeComponent();
            context = new ItemDetailViewModel(this);
            context.ItemId = newGuid.ToString();
            context.SetAudioFile(context.Filename);
            BindingContext = context;
            ContentStack.Padding = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            context.StopCommand.Execute(null);
            base.OnDisappearing();
        }
        private void Home_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AboutPage();
        }

        private void Record_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AudioChooserPage();
        }

        private void Records_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RecordingsPage();
        }
        private void Back_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new RecordingsPage();
        }

        private async void Remove_Tapped(object sender, EventArgs e)
        {
            if(!await DisplayAlert("Потвърждение", "Сигурни ли сте, че желаете да изтриете този записъ?\r\nТова дѣйствие е необратимо!", "Да", "Не"))
            {
                return;
            }
            if (File.Exists(context.Filename))
                File.Delete(context.Filename);
            await UploadHelper.RemoveFile(context.Item);
            await context.DataStore.DeleteItemAsync(context.Id);
            App.Current.MainPage = new RecordingsPage();
        }

        void Play_Clicked(System.Object sender, System.EventArgs e)
        {
            context.PlayCommand.Execute(sender);
        }
        void Pause_Clicked(System.Object sender, System.EventArgs e)
        {
            context.PauseCommand.Execute(sender);
        }
        void Stop_Clicked(System.Object sender, System.EventArgs e)
        {
            context.StopCommand.Execute(sender);
        }
        void Slider_DragCompleted(System.Object sender, System.EventArgs e)
        {
            context.PlayAtPositionCommand.Execute(((Slider)sender).Value);
        }
    }

    class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nullable = value as double?;
            var result = string.Empty;

            if (nullable.HasValue)
            {
                result = ((int)nullable.Value).ToString();
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            int intValue;
            int? result = null;

            if (int.TryParse(stringValue, out intValue))
            {
                result = new Nullable<int>(intValue);
            }

            return result;
        }
    }
}