using Octane.Xamarin.Forms.VideoPlayer;
using RodnaPamet.Models;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private DateTime created;
        private string filename;
        private string subject;
        private string cameraMan;
        private string description;
        private string village;
        private bool uploading;
        private bool uploaded;
        private Item item;
        public ICommand VillageTapped { get; set; }

        public event EventHandler<string> Error;

        public ItemDetailViewModel(IAnimatable cont) : base(cont)
        {
        }
        public string Id { get; set; }
        private int age;

        private bool isStory;
        public bool IsStory {
            get => isStory;
            set
            {
                SetProperty(ref isStory, value);
            }
        }
        public ICommand SaveCommand { get; set; }
        public Item Item { get => item; }

        private VideoSource videoSource;
        private string typeDescription = "";

        public string Filename
        {
            get => filename;
            set
            {
                SetProperty(ref filename, value);
            }
        }
        public string FilePath
        {
            get => filename;
        }
        public DateTime Created
        {
            get => created;
            set
            {
                SetProperty(ref created, value);
            }
        }
        public int Age
        {
            get => age;
            set
            {
                SetProperty(ref age, value);
            }
        }
        public string TypeDescription
        {
            get => typeDescription;
            set
            {
                SetProperty(ref typeDescription, value);
            }
        }
        public string CameraMan
        {
            get => cameraMan;
            set
            {
                SetProperty(ref cameraMan, value);
                (SaveCommand as Command)?.ChangeCanExecute();
            }
        }
        public string Subject
        {
            get => subject;
            set
            { 
                SetProperty(ref subject, value);
                (SaveCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);
                (SaveCommand as Command)?.ChangeCanExecute();
            }
        }

        public string Village
        {
            get => village;
            set
            {
                SetProperty(ref village, value);
                (SaveCommand as Command)?.ChangeCanExecute();
            }
        }

        public bool Uploading
        {
            get => uploading;
            set
            {
                SetProperty(ref uploading, value);
            }
        }

        public bool Uploaded
        {
            get => uploaded;
            set
            {
                SetProperty(ref uploaded, value);
            }
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value.ToString());
            }
        }

        public VideoSource VideoSource
        {
            get
            {
                return videoSource;
            }
            set
            {
                SetProperty(ref videoSource, value);
            }
        }

        public async void SaveItemToDB()
        {
            item.Id = Id;
            item.Age = Age;
            item.Created = Created;
            item.Subject = Subject;
            item.Filename = Filename;
            item.Cameraman = CameraMan;
            item.Description = Description;
            item.Village = Village;
            item.Type = IsStory ? 1 : 0;
            item.InfoComplete = true;
            item.TypeDescription = TypeDescription;

            await DataStore.UpdateItemAsync(item);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                item = await DataStore.GetItemAsync(itemId);
                Id = item.Id.ToString();
                Created = item.Created;
                Age = item.Age;
                Subject = item.Subject;
                Filename = item.Filename;
                VideoSource = VideoSource.FromFile(item.Filename);
                CameraMan = item.Cameraman;
                Created = item.Created;
                Description = item.Description;
                Village = item.Village;
                Uploaded = item.Uploaded;
                TypeDescription = item.TypeDescription;
                IsStory = item.Type == 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
