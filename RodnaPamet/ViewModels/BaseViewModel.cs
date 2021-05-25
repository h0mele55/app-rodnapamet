using RodnaPamet.Models;
using RodnaPamet.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Item> DataStore => DependencyService.Get<MockDataStore>();
        protected Animation busyAnim = new Animation();
        protected IAnimatable container;
        bool isBusy = false;

        public BaseViewModel(IAnimatable cont)
        {
            container = cont;
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { 
                SetProperty(ref isBusy, value);
                if (isBusy)
                {
                    StartLoaderAnim();
                }
                else
                {
                    StopLoaderAnim();
                }
            }
        }

        protected void StartLoaderAnim()
        {
            if (busyAnim == null)
                new Animation();
            busyAnim.Add(0, 0.5, new Animation(scale => BusyScale = scale, 1, 1.5, Easing.CubicOut, null));
            busyAnim.Add(0.5, 1, new Animation(scale => BusyScale = scale, 1.5, 1, Easing.CubicOut, null));

            busyAnim.Add(0, 0.5, new Animation(opacity => BusyOpacity = opacity, 1, 0.6, Easing.CubicOut, null));
            busyAnim.Add(0.5, 1, new Animation(opacity => BusyOpacity = opacity, 0.6, 1, Easing.CubicOut, null));

            busyAnim.Commit(container, "loadingIndicatorPulseAnimation", 5, 1500, null, null, () => true);
        }

        protected void StopLoaderAnim()
        {
            container.AbortAnimation("loadingIndicatorPulseAnimation");
//            busyAnim = null;
        }

        double busyOpacity = 1;
        public double BusyOpacity
        {
            get { return busyOpacity; }
            set { SetProperty(ref busyOpacity, value); }
        }
        double busyScale = 1;
        public double BusyScale
        {
            get { return busyScale; }
            set { SetProperty(ref busyScale, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
