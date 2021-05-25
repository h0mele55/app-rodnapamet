using RodnaPamet.Models;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private DateTime created;
        private string filename;
        private string subject;
        private string cameraMan;
        private string description;
        private string village;
        public NewItemViewModel(IAnimatable cont) : base(cont)
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(subject)
                && !String.IsNullOrWhiteSpace(description);
        }

        public string Filename
        {
            get => filename;
            set => SetProperty(ref filename, value);
        }
        public DateTime Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
        public string CameraMan
        {
            get => cameraMan;
            set => SetProperty(ref cameraMan, value);
        }
        public string Subject
        {
            get => subject;
            set => SetProperty(ref subject, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public string Village
        {
            get => village;
            set => SetProperty(ref village, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            App.Current.MainPage = new RecordingsPage();
            //await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Created = created,
                Subject = subject,
                Filename = filename,
                Cameraman = cameraMan,
                Description = description,
                Village = village
            };

            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            //await Shell.Current.GoToAsync("..");
            App.Current.MainPage = new RecordingsPage();
        }
    }
}
