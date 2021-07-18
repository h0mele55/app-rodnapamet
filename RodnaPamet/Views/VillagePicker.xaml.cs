using RodnaPamet.Models;
using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VillagePicker : ContentView
    {
        public event EventHandler VillageSelected;
        VillagePickerViewModel _viewModel;
        ViewCell lastCell;
        public string SelectedVillage { get; set; } = "";
        public VillagePicker()
        {
            InitializeComponent();
            BindingContext = _viewModel = new VillagePickerViewModel(this);

            ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
        }

        private void SearchField_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (StackLayout)sender;

            Village selected = (Village)((StackLayout)sender).BindingContext;
            SelectedVillage = selected.Type + " " + selected.Name + ", " + selected.Obstina + ", " + selected.Oblast;
            VillageSelected?.Invoke(this, null);
            /*            if (viewCell.View != null)
                        {
                            object color = new Color();
                            Application.Current.Resources.TryGetValue("Primary", out color);
                            //viewCell.View.BackgroundColor = (Color) color;
                            viewCell.View.BackgroundColor = Color.Transparent; 
                            lastCell = viewCell;
                            Village selected = (Village) ((ViewCell)sender).BindingContext;
                            SelectedVillage = selected.Type + " " + selected.Name + ", " + selected.Obstina + ", " + selected.Oblast;
                            VillageSelected?.Invoke(this, null);
                        }
              */
        }

        public void ClearInterface()
        {
            SelectedVillage = "";
            SearchField.Text = "";
        }

        public void FocusInput()
        {
            SearchField.Focus();
        }
    }
}