using RodnaPamet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace RodnaPamet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public event EventHandler LoginSuccess;
        LoginViewModel viewModel;
        private static Type invalidationEventArgsType;
        private static PropertyInfo triggerProperty;
        public LoginPage()
        {
            InitializeComponent();

            Shell.SetTabBarIsVisible(this, false);
        }

        protected override void OnAppearing()
        {
            this.BindingContext = viewModel = new LoginViewModel(this);
            ContentStack.Margin = new Thickness(0, StatusBar.Height, 0, 0);
            viewModel.IsBusy = true;
            viewModel.LoginSuccess += ViewModel_LoginSuccess;
            viewModel.Error += ViewModel_Error;
        }

        private void ViewModel_Error(object sender, string e)
        {
            Device.BeginInvokeOnMainThread(() => {
                DisplayAlert("Грешка", e, "Добре");
            });
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            MessagingCenter.Send<LoginPage>(this, "login_success");
            LoginSuccess?.Invoke(this, e);
        }

        void Switch_Toggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            viewModel.TermsAccepted = e.Value;
        }

        private void Image_MeasureInvalidated(object sender, EventArgs e)
        {
            var trigger = TryGetInvalidationTrigger(e);
            if (trigger.HasValue && trigger.Value.HasFlag(InvalidationTrigger.RendererReady))
            {
                MessagingCenter.Send<LoginPage>(this, "login_success");
                //viewModel.IsBusy = false;
            }
            //    if(((InvalidationEventArgs)e).)
            //            if (App.UserService.SubscribersList.Count == 1)
            //                MessagingCenter.Send<LoginPage>(this, "login_success");
            //            else
            //                viewModel.IsBusy = false;
        }

        internal static InvalidationTrigger? TryGetInvalidationTrigger(EventArgs e)
        {
            Type type = e.GetType();
            if (invalidationEventArgsType == null)
            {
                if (type.FullName == "Xamarin.Forms.InvalidationEventArgs")
                {
                    invalidationEventArgsType = type;
                    triggerProperty = type.GetRuntimeProperty("Trigger");
                }
            }

            if (type != invalidationEventArgsType)
            {
                return null;
            }

            object propertyValue = triggerProperty.GetValue(e);
            InvalidationTrigger actualTrigger = (InvalidationTrigger)propertyValue;
            return actualTrigger;
        }
    }
}
