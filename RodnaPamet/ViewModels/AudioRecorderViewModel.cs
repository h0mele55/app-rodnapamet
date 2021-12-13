using RodnaPamet.Models;
using RodnaPamet.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class AudioRecorderViewModel : BaseViewModel
    {
        AudioPreview audioPreview;
        public int RecordingTypeValues = 1;
        public int RecordingTypeStory = 2;
        public Command RecordCommand { get; }
        public Command PauseCommand { get; }
        public Command StopCommand { get; }
        public Command QuestionsCommand { get; }
        public Command SelectValuesCommand { get; }
        public Command SelectStoryCommand { get; }
        public Command BackCommand { get; }
        public string RecordControlLabel { get; set; }
        public string recLabel;
        public string RecLabel
        {
            get { return recLabel; }
            set { SetProperty(ref recLabel, value); }
        }
        public string RecordStopLabel { get; set; }
        private bool isRecording = false;
        private bool inTransition = false;
        private bool isPreviewing = false;
        private int question = -1;
        public string[] Questions { get; } = new string[] {
            "Тайната за здравото семейство е в...",
            "Тайната за истинската любовь е в...",
            "Тайната за силното приятелство е в...",
            "Тайната за личното щастие е в...",
            "Тайната за народното обѣдинение е в...",
            "Тайната за успеха въ работата е в...",
            "Тайната за дългия животъ е в...",
            "Любима билка...",
            "Любима мисъль/поговорка...",
            "Любимо място в България...",
            "Любимо Българско стихотворение...",
            "Запей любимата си Българска песен...",
            "Единъ съвѣтъ къмъ младото поколѣние...",
            "Какъ искашъ да бѫдешъ запоменъ?",
            "Каква е твоята тайна за..."
            };
        public bool IsRecording
        {
            get { return isRecording; }
            set { SetProperty(ref isRecording, value); }
        }
        public bool InTransition
        {
            get { return inTransition; }
            set { SetProperty(ref inTransition, value); }
        }
        public bool IsPreviewing
        {
            get { return isPreviewing; }
            set { SetProperty(ref isPreviewing, value); }
        }
        private bool selectedVideoType = false;
        public bool SelectedVideoType
        {
            get { return selectedVideoType; }
            set { SetProperty(ref selectedVideoType, value); }
        }

        private bool selectedValuesType = false;
        public bool SelectedValuesType
        {
            get { return selectedValuesType; }
            set { SetProperty(ref selectedValuesType, value); }
        }

        private bool selectedStoryType = false;
        public bool SelectedStoryType
        {
            get { return selectedStoryType; }
            set { SetProperty(ref selectedStoryType, value); }
        }

        public Func<double, double, bool> ItemHeightChanged;
        double itemHeight1;
        public double ItemHeight1
        {
            get { return itemHeight1; }
            set { itemHeight1 = value; if (ItemHeightChanged != null) { ItemHeightChanged(itemHeight1, ItemHeight2); } }
        }

        double itemHeight2;
        public double ItemHeight2
        {
            get { return itemHeight2; }
            set { itemHeight2 = value; if (ItemHeightChanged != null) { ItemHeightChanged(itemHeight1, ItemHeight2); } }
        }

        private int currentQuestion = 0;
        public int CurrentQuestion
        {
            get { return currentQuestion; }
            set { SetProperty(ref currentQuestion, value); }
        }

        private string question1 = "";
        public string Question1
        {
            get { return question1; }
            set { SetProperty(ref question1, value); }
        }
        private string question2 = "";
        public string Question2
        {
            get { return question2; }
            set { SetProperty(ref question2, value); }
        }
        private string question3 = "";
        public string Question3
        {
            get { return question3; }
            set { SetProperty(ref question3, value); }
        }
        private string question1number = "";
        public string Question1Number
        {
            get { return question1number; }
            set { SetProperty(ref question1number, value); }
        }
        private string question2number = "";
        public string Question2Number
        {
            get { return question2number; }
            set { SetProperty(ref question2number, value); }
        }
        private string question3number = "";
        public string Question3Number
        {
            get { return question3number; }
            set { SetProperty(ref question3number, value); }
        }
        private int questionnumbers = 0;
        public int QuestionNumbers
        {
            get { return questionnumbers; }
            set { SetProperty(ref questionnumbers, value); }
        }
        public AudioRecorderViewModel(IAnimatable cont, AudioPreview aPreview) : base(cont)
        {
            audioPreview = aPreview;

            Title = "Новъ Записъ";
            RecordControlLabel = "Запись";
            RecordStopLabel = "Спри";
            RecLabel = "";
            IsRecording = false;
            RecordCommand = new Command(async () => await ExecuteRecordCommand());
            StopCommand = new Command(async () => await ExecuteStopCommand());
            QuestionsCommand = new Command(async () => await ExecuteQuestionsCommand());
            SelectStoryCommand = new Command(async () => await ExecuteStoryCommand());
            SelectValuesCommand = new Command(async () => await ExecuteValuesCommand());
            BackCommand = new Command(async () => await ExecuteBackCommand());

            CurrentQuestion = 0;
            Question1 = "";
            Question2 = Questions[0];
            Question3 = Questions[1];
            Question1Number = "";
            Question2Number = "1";
            Question3Number = "2";
            QuestionNumbers = Questions.Length;
        }
        async Task ExecuteValuesCommand()
        {
            SelectedVideoType = true;
            SelectedValuesType = true;
            SelectedStoryType = false;
        }
        async Task ExecuteStoryCommand()
        {
            SelectedVideoType = true;
            SelectedValuesType = false;
            SelectedStoryType = true;
        }
        async Task ExecuteBackCommand()
        {
            App.Current.MainPage = new CameraChooserPage();
        }
        async Task ExecuteQuestionsCommand()
        {
            if(CurrentQuestion < Questions.Length - 1)
                CurrentQuestion++;
            return;
            question++;
            int start = question;
            if (start >= Questions.Length)
                start = 0;
//            for (int i = 0; i < 3; i++)
//            {
            Question1 = Questions[start];
            Question2 = Questions[start + 1];
            Question1Number = (start + 1).ToString();
            Question2Number = (start + 2).ToString();
            if (start + 2 >= Questions.Length)
            {
                Question3 = "";
                Question3Number = "";
                start--;
            }
            else
            {
                Question3Number = (start + 3).ToString();
                Question3 = Questions[start + 2];
            }
            //            }
            question = start;
        }
        async Task ExecuteRecordCommand()
        {
            IsPreviewing = true;
            MessagingCenter.Send<string>("StartRecording", "On");
            audioPreview.StartRecording?.Execute(null);
            StartLabelTimer();
            IsRecording = true;
        }

        async Task ExecuteStopCommand()
        {
            InTransition = true;
            IsRecording = false;
            IsPreviewing = false;
            MessagingCenter.Send<string>("StopRecording", "On");
            audioPreview.StopRecording?.Execute(null);
            StopLabelTimer();
            question = 0;
            SelectedVideoType = false;
            //SelectedValuesType = false;
            //SelectedStoryType = false;
        }
        public void OnAppearing()
        {
            IsBusy = true;
            IsRecording = false;
            SelectedVideoType = false;
            SelectedValuesType = false;
            SelectedStoryType = false;
            InTransition = false;
            RecLabel = "";
        }

        DateTime LabelTimerStart;
        private void StartLabelTimer()
        {
            LabelTimerStart = DateTime.Now;
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                bool stop = !IsRecording;
                TimeSpan newNow = DateTime.Now - LabelTimerStart;
                RecLabel = newNow.Minutes.ToString("00") + ":" + newNow.Seconds.ToString("00");
                return !stop;
            });
        }
        private void StopLabelTimer()
        {
            RecLabel = "";
        }

        public void OnDisappearing()
        {
            MessagingCenter.Send<string>("StopRecording", "On");
            IsBusy = false;
            IsRecording = false;
            SelectedVideoType = false;
        }

        private async void OnAddItem(object obj)
        {
            //await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            App.Current.MainPage = new ItemDetailPage(new Guid(item.Id));
        }
    }
}