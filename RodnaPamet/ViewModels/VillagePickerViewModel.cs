using Newtonsoft.Json;
using RodnaPamet.Models;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class VillagePickerViewModel : BaseViewModel
    {
        private Item _selectedItem;

        public List<Village> Villages { get; }
        public Dictionary<string, string> Oblasti { get; }
        public Dictionary<string, string> Obshtini { get; }
        public ObservableCollection<Village> ShownVillages { get; }

        public ObservableCollection<Item> Items { get; }
        public Command TextChangedCommand { get; }
        public string SearchText { get; set; }
        public VillagePickerViewModel(IAnimatable cont) : base(cont)
        {
            ShownVillages = new ObservableCollection<Village>();
            Villages = new List<Village>();

            string jsonFileName = "contacts.json";

            var assembly = typeof(App).GetTypeInfo().Assembly;

            Stream stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Oblasti.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                Oblasti = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }

            stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Obstini.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                Obshtini = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            }

            stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Mesta.json");
            using (var reader = new System.IO.StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();

                //Converting JSON Array Objects into generic list    
                Villages = JsonConvert.DeserializeObject<List<Village>>(jsonString);
                foreach (Village village in Villages)
                {
                    village.Obstina = Obshtini[village.Obstina];
                    village.Oblast = Oblasti[village.Oblast];
                }
            }

            TextChangedCommand = new Command(ExecuteTextChangedCommand);
        }

        async void ExecuteTextChangedCommand()
        {
            IsBusy = true;

            //try
            {
                ShownVillages.Clear();
                if (SearchText.Length > 2)
                {
                    List<Village> temp = new List<Village>();
                    foreach (Village village in Villages)
                    {
                        if (village.Name.ToLower().Contains(SearchText.ToLower()))
                        {
                            ShownVillages.Add(village);
                        }
                    }
                }
            }
            //catch (Exception ex)
            {
            //    Debug.WriteLine(ex);
            }
            //finally
            {
            //    IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                //OnItemSelected(value);
            }
        }
    }
}