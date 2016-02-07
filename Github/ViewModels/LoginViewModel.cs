using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
                    _login = new RelayCommand(() =>
                    {
                        //LOG IN TO GITHUB
                    },
                    () => !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !username.Contains(" ") && !password.Contains(" "));
                }
                return _login;
            }
        }
    }
}
