using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Helper;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Github.ViewModels
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
                        var login_result = await utilities.LogIn(username, password);
                        if (login_result)
                        {
                            //SAVE CREDENTIALS ON SUCCESFULL LOGIN
                            utilities.SaveCredentials("login", username, password);
                            Window.Current.Content = new Views.Shell(Views.LoginPage.nav_service);
                            App.Current.NavigationService.Navigate(typeof(Views.MainPage));
                        }
                    },
                    () => !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !username.Contains(" ") && !password.Contains(" "));
                }
                return _login;
            }
        }
    }
}
