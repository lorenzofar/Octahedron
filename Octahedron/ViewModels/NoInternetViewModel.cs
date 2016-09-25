using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Helper;
using Template10.Mvvm;
using Windows.UI.Xaml;

namespace Octahedron.ViewModels
{
    public class NoInternetViewModel : ViewModelBase
    {
        private bool _loggingIn;
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

        private RelayCommand _Refresh;
        public RelayCommand Refresh
        {
            get
            {
                if(_Refresh == null)
                {
                    _Refresh = new RelayCommand(async() =>
                    {
                        loggingIn = true;
                        var credential = utilities.GetCredential("login");
                        var loginResult = await utilities.LogIn(credential.UserName, credential.Password);
                        if (loginResult == utilities.LoginResult.success)
                        {
                            App.user = await constants.g_client.User.Current();
                            Messenger.Default.Send<MvvmMessaging.ProfileIconMessage>(new MvvmMessaging.ProfileIconMessage { url = App.user.AvatarUrl });
                            App.Current.NavigationService.ClearHistory();
                            Window.Current.Content = App.shell;
                            App.Current.NavigationService.Navigate(typeof(Views.MainPage));
                        }
                        else
                        {
                            loggingIn = false;
                        }
                    });
                }
                return _Refresh;
               
            }
        }
    }
}
