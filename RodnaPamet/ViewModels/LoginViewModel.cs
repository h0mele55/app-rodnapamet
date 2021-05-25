using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public Command RegisterCommand { get; }
        public event EventHandler LoginSuccess;
        public event EventHandler ErrorHandler;
        private bool showPassword = false;
        private bool showRegister = false;
        private string eMail;
        private string password;
        public bool TermsAccepted = false;

        public event EventHandler<string> Error;
        public LoginViewModel(IAnimatable cont) : base(cont)
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
        }
        public bool ShowPassword
        {
            get
            {
                return showPassword;
            }
            set
            {
                SetProperty(ref showPassword, value);
            }
        }
        public bool ShowRegister
        {
            get
            {
                return showRegister;
            }
            set
            {
                SetProperty(ref showRegister, value);
            }
        }
        public string EMail
        {
            get
            {
                return eMail;
            }
            set
            {
                SetProperty(ref eMail, value);
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                SetProperty(ref password, value);
            }
        }
        private async void OnLoginClicked(object obj)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = true;
            });

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            Task.Run(async () => {
            try
            {
                User user = new User();
                user.EMail = EMail;
                user.Password = Password;

                bool logged = await App.UserService.AuthorizeUserAsync(EMail, Password);
                if (logged) // Успешно влизане в приложението!
                {
                    App.UserService.Persist(); // Записваме потребителя за в бъдеще
                                               // Показваме приложението
                    LoginSuccess?.Invoke(this, null);
                }
                else
                {
                    // todo: fire event for no luck message
                    ErrorHandler?.Invoke(this, null);
                }
                
            }
            catch (System.OperationCanceledException ex)
            {
                    ErrorHandler?.Invoke(this, null);
                    Console.WriteLine($"Text load cancelled: {ex.Message}");
            }
            catch (Exception ex)
            {
                    ErrorHandler?.Invoke(this, null);
                    Console.WriteLine(ex.Message);
            }
            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = false;
            });
        }, ct);

            // Check user name
            // Show password
            // Check password
            // Register if new
            // Login to AboutPage if successfully registered

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void OnRegisterClicked(object obj)
        {
            if (!TermsAccepted)
            {     
                Error?.Invoke(this, "Трябва да приемете условията за употреба!");
                return;
            }

            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = true;
            });

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            Task.Run(async () => {
                try
                {
                    User user = new User();
                    user.EMail = EMail;

                    App.UserService.SubscribersList.Clear();
                    bool added = await App.UserService.AddItemAsync(user);
                    if (added) // Нямало е такъв потребител, вече има, изпратено е писмо за потвърждаване на пощата
                    {
                        showRegister = true;
                    }
                    else if (App.UserService.SubscribersList.Count > 0) // Има такъв потребител, получаваме id, искаме тайна дума 
                    {
                        ShowPassword = true;
                    }
                    else if (App.UserService.GetLastMessage() != "")
                    {
                        Error?.Invoke(this, App.UserService.GetLastMessage());
                    }
                }
                catch (System.OperationCanceledException ex)
                {
                    Console.WriteLine($"Text load cancelled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                await Device.InvokeOnMainThreadAsync(() =>
                {
                    IsBusy = false;
                });
            }, ct);

            // Check user name
            // Show password
            // Check password
            // Register if new
            // Login to AboutPage if successfully registered

            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            //            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }

        private async void EMailConfirmCommand(object obj)
        {
            IsBusy = true;

        }
    }
}
