using RodnaPamet.Services;
using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RodnaPamet.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[QueryProperty(nameof(type), "type")]
	public partial class CameraPage : ContentPage, IQueryAttributable
	{
        RecorderViewModel viewModel;
		public CameraPage()
		{
			InitializeComponent();

            BindingContext = viewModel = new RecorderViewModel(this, CameraPreview);

			ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
			//QuestionsCarousel.ItemSelected += QuestionCarousel_ItemSelected;
			viewModel.ItemHeightChanged = QuestionCarouselItemSizeChanged;

			CameraPreview.VideoFinished += OnVideoFinished;
        }

		public CameraPage(string type)
		{
			InitializeComponent();

			BindingContext = viewModel = new RecorderViewModel(this, CameraPreview);

			if (type == "values")
			{
				viewModel.SelectedVideoType = true;
				viewModel.SelectedStoryType = false;
				viewModel.SelectedValuesType = true;
			}
			else if (type == "story")
			{
				viewModel.SelectedVideoType = true;
				viewModel.SelectedStoryType = true;
				viewModel.SelectedValuesType = false;
			}

			ContentStack.Margin = new Thickness(0, App.HeaderSize, 0, App.FooterSize);
			RecNotification.Padding = new Thickness(RecNotification.Padding.Left, App.HeaderSize + 5, RecNotification.Padding.Right, RecNotification.Padding.Bottom);
			viewModel.ItemHeightChanged = QuestionCarouselItemSizeChanged;

			CameraPreview.VideoFinished += OnVideoFinished;
		}

		public bool QuestionCarouselItemSizeChanged(double h1, double h2)
		{
			QuestionsCarousel.HeightRequest = h1 + h2 + 10;
			return true;
		}
		private async void OnVideoFinished(object sender, EventArgs e)
		{
			Guid newGuid = Guid.NewGuid();
			await DependencyService.Get<MockDataStore>().AddItemAsync(new Models.Item()
			{
				Id = newGuid.ToString(),
				Created = DateTime.Now,
				Description = viewModel.SelectedStoryType ? "История" : "Ценности",
				Filename = ((VideoEventArgs)e).Filename,
				Subject = "",
				Cameraman = "",
				Type = viewModel.SelectedStoryType ? 1 : 0,
				Uploading = false,
				Uploaded = false
			});
			App.Current.MainPage = new ItemDetailPage(newGuid);
			//((AppShell)App.Current.MainPage).OpenItemDetails(newGuid);
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
		}

		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
			viewModel.QuestionsCommand.Execute(null);
        }

		public void ApplyQueryAttributes(IDictionary<string, string> query)
		{
			if (query.ContainsKey("type"))
				type = query["type"];
			if (type == "values")
			{
				viewModel.SelectedVideoType = true;
				viewModel.SelectedStoryType = false;
				viewModel.SelectedValuesType = true;
			}
			else if (type == "story")
			{
				viewModel.SelectedVideoType = true;
				viewModel.SelectedStoryType = true;
				viewModel.SelectedValuesType = false;
			}
		}

		public string type { get; set; }
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
		private void Back_Clicked(object sender, EventArgs e)
		{
			App.Current.MainPage = new CameraChooserPage();
		}
	}

	public class VideoEventArgs : EventArgs
	{
		public string Filename { get; set; }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class BinaryInvertConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return true;
			if (value.GetType().IsAssignableFrom(typeof(bool)))
				return !(bool)value;
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return true;
			if (value.GetType().IsAssignableFrom(typeof(bool)))
				return (bool)value;
			if (value.GetType().IsAssignableFrom(typeof(string)))
				return (string)value == "" ? false : true;
			if (value.GetType().IsAssignableFrom(typeof(int)))
				return (int)value == 0 ? false : true;
			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public class FilenameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "";
			if (value.GetType().IsAssignableFrom(typeof(string)))
				return "ms-appdata://" + (string)value;//FileMediaSource.FromFile("ms-appdata://" + (string) value);
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}