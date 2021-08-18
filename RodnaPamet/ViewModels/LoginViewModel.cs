using RodnaPamet.Models;
using RodnaPamet.Services;
using RodnaPamet.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RodnaPamet.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; } // Initial step
        public Command BackCommand { get; }
        public Command ConfirmCommand { get; } // Confirmation code check
        public Command PasswordCommand { get; } // User found, check password
        public Command RegisterCommand { get; } // User creating, filled fields
        public Command TermsCommand { get; }
        public event EventHandler LoginSuccess;
        public event EventHandler ErrorHandler;
        private bool showInitial = true;
        private bool showPassword = false;
        private bool showDetails = false;
        private bool showRegistered = false;
        private string name;
        private string eMail;
        private string password;
        private string password2;
        private string verificationCode;
        public bool TermsAccepted = false;

        public event EventHandler<string> Error;
        public LoginViewModel(IAnimatable cont) : base(cont)
        {
            LoginCommand = new Command(OnLoginClicked);
            RegisterCommand = new Command(OnRegisterClicked);
            PasswordCommand = new Command(OnPasswordClicked);
            TermsCommand = new Command(OnTermsClicked);
            ConfirmCommand = new Command(OnConfirmClicked);
            BackCommand = new Command(OnBackClicked);
        }
        public bool ShowInitial
        {
            get
            {
                return showInitial;
            }
            set
            {
                SetProperty(ref showInitial, value);
            }
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
        public bool ShowDetails
        {
            get
            {
                return showDetails;
            }
            set
            {
                SetProperty(ref showDetails, value);
            }
        }
        public bool ShowRegistered
        {
            get
            {
                return showRegistered;
            }
            set
            {
                SetProperty(ref showRegistered, value);
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
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
        public string Password2
        {
            get
            {
                return password2;
            }
            set
            {
                SetProperty(ref password2, value);
            }
        }
        public string VerificationCode
        {
            get
            {
                return verificationCode;
            }
            set
            {
                SetProperty(ref verificationCode, value);
            }
        }
        private async void OnPasswordClicked(object obj)
        {
            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = true;
            });

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            _ = Task.Run(async () =>
            {
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
                        Error?.Invoke(this, "Грѣшно потрѣбителско име или тайна дума!");
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

        private async void OnLoginClicked(object obj)
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

            _ = Task.Run(async () =>
            {
                try
                {
                    User user = new User();
                    user.EMail = EMail;

                    App.UserService.SubscribersList.Clear();
                    bool existing = await App.UserService.CheckUserAsync(user.EMail);
                    if (!existing) // Нямало е такъв потребител, вече има, изпратено е писмо за потвърждаване на пощата
                    {
                        ShowInitial = false;
                        ShowDetails = true;
                    }
                    else if (App.UserService.SubscribersList[0].EMailVerified == "1") // Има такъв потребител, получаваме id, искаме тайна дума 
                    {
                        ShowInitial = false;
                        ShowPassword = true;
                    }
                    else if (App.UserService.SubscribersList[0].EMailVerified == "0")
                    {
                        ShowInitial = false;
                        ShowRegistered = true;
                    }
                    else if (App.UserService.GetLastMessage() != "")
                    {
                        Error?.Invoke(this, App.UserService.GetLastMessage());
                    }
                }
                catch (System.OperationCanceledException ex)
                {
                    Error?.Invoke(this, ex.Message);
                }
                catch (Exception ex)
                {
                    Error?.Invoke(this, ex.Message);
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
            if (Password.Trim() == "" ||
                Password.IndexOf(" ") > -1 ||
                Password.Length < 6 ||
                Password.Any(char.IsUpper) == false ||
                Password.Any(char.IsLower) == false ||
                Password.Any(char.IsNumber) == false)
            {
                Error?.Invoke(this, "Тайната дума трѣбва да съдържа главни, малки букви, поне една цифра и да е съ дължина по-голѣма отъ 6 знака!");
                return;
            }
            if (Password != Password2)
            {
                Error?.Invoke(this, "Тайната дума и потвърждението не съвпадатъ!");
                return;
            }
            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = true;
            });

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            _ = Task.Run(async () =>
            {
                try
                {
                    User user = new User();
                    user.EMail = EMail;
                    user.Name = Name;
                    user.Password = Password;

                    bool added = await App.UserService.AddItemAsync(user);
                    if (added) // Успешно влизане в приложението!
                    {
                        App.UserService.Persist(); // Записваме потребителя за в бъдеще
                                                   // Показваме приложението
                        ShowDetails = false;
                        ShowRegistered = true;
                        //LoginSuccess?.Invoke(this, null);
                    }
                    else
                    {
                        // todo: fire event for no luck message
                        Error?.Invoke(this, "Неуспѣшно добавѣне на потрѣбитель! Натиснете назадъ и опитайте отново.");
                    }

                }
                catch (System.OperationCanceledException ex)
                {
                    Error?.Invoke(this, ex.Message);
                    Console.WriteLine($"Text load cancelled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Error?.Invoke(this, ex.Message);
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

        private async void OnTermsClicked(object obj)
        {
            App.Current.MainPage = new TermsPage();
        }

        private async void OnBackClicked(object obj)
        {
            ShowInitial = true;
            ShowDetails = false;
            ShowPassword = false;
            ShowRegistered = false;
        }

        private async void OnConfirmClicked(object obj)
        {
            if (VerificationCode.Trim() == "" ||
    VerificationCode.IndexOf(" ") > -1 ||
    VerificationCode.Length != 4)
            {
                Error?.Invoke(this, "Числото за потвърждение трѣбва да съдържа 4 цифри!");
                return;
            }
            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = true;
            });

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            _ = Task.Run(async () =>
            {
                try
                {
                    User user = App.UserService.SubscribersList[0];

                    bool added = await App.UserService.ConfirmEMailAsync(user.EMail, VerificationCode);
                    if (added) // Успешно влизане в приложението!
                    {
                        App.UserService.SubscribersList[0].EMailVerified = "1";
                        App.UserService.Persist(); // Записваме потребителя за в бъдеще
                                                   // Показваме приложението
                        LoginSuccess?.Invoke(this, null);
                    }
                    else
                    {
                        // todo: fire event for no luck message
                        Error?.Invoke(this, "Грѣшно число за потвърждение!");
                    }

                }
                catch (System.OperationCanceledException ex)
                {
                    Error?.Invoke(this, ex.Message);
                    Console.WriteLine($"Text load cancelled: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Error?.Invoke(this, ex.Message);
                    Console.WriteLine(ex.Message);
                }
                await Device.InvokeOnMainThreadAsync(() =>
                {
                    IsBusy = false;
                });
            }, ct);
            IsBusy = false;
        }
    }
}
