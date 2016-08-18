using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Helper;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Octahedron.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        public string username
        {
            get
            {
                return _username;
            }
            set
            {
                Set(ref _username, value);
                login.RaiseCanExecuteChanged();
            }
        }

        private string _password;
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                Set(ref _password, value);
                login.RaiseCanExecuteChanged();
            }
        }

        private bool _loggingIn = false;
        public bool loggingIn
        {
            get
            {
                return _loggingIn;
            }
            set
            {
                Set(ref _loggingIn, value);
            }
        }

        private RelayCommand<object> _KeyPressed;
        public RelayCommand<object> KeyPressed
        {
            get
            {
                if (_KeyPressed == null)
                {
                    _KeyPressed = new RelayCommand<object>((e) =>
                    {
                        var args = e as KeyRoutedEventArgs;
                        if (args.Key == Windows.System.VirtualKey.Enter)
                        {
                            login.Execute(null);
                        }
                    });
                }
                return _KeyPressed;
            }
        }

        private RelayCommand _login;
        public RelayCommand login
        {
            get
            {
                if (_login == null)
                {
                    _login = new RelayCommand(async() =>
                    {
                        //LOG IN TO GITHUB
                        loggingIn = true;
                        var login_result = await utilities.LogIn(username, password);
                        if (login_result)
                        {
                            //SAVE CREDENTIALS ON SUCCESFULL LOGIN
                            utilities.SaveCredentials("login", username, password);
                            App.user = await constants.g_client.User.Current();
                            Messenger.Default.Send<MvvmMessaging.ProfileIconMessage>(new MvvmMessaging.ProfileIconMessage { url = App.user.AvatarUrl });
                            App.Current.NavigationService.ClearHistory();
                            Window.Current.Content = App.shell;
                            App.Current.NavigationService.Navigate(typeof(Views.MainPage));
                        }
                        else
                        {
                            loggingIn = false;
                            login.RaiseCanExecuteChanged();
                        }
                    },
                    () => !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !username.Contains(" ") && !password.Contains(" ") && !loggingIn);
                }
                return _login;
            }
        }
    }
}
