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

            this.BindingContext = viewModel = new LoginViewModel(this);

            ContentStack.Padding = new Thickness(0, App.HeaderSize, 0, App.FooterSize);

            //            ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.PageTitle"])[0].Setters[1].Value = (int) App.HeaderFontSize;
            //            ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.BigButton"])[0].Setters[1].Value = App.BigButtonSize;
            //            ((List<Style>)Application.Current.Resources["Xamarin.Forms.StyleClass.PageTitle"])[0].Setters[1].Value = App.HeaderFontSize;
        }

        protected override void OnAppearing()
        {
            //ContentStack.Padding = new Thickness(0, StatusBar.Height + StatusBarFix.Height, 0, NavBarFix.Height);
            base.OnAppearing();
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
                if (App.UserService.SubscribersList.Count == 1)
                    MessagingCenter.Send<LoginPage>(this, "login_success");
                else
                    viewModel.IsBusy = false;
            }
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
