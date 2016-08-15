using GalaSoft.MvvmLight.Command;
using Helper;
using Template10.Mvvm;
using Windows.UI.Xaml;
using static Template10.Common.BootStrapper;

namespace Octahedron.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private RelayCommand _LogOut;
        public RelayCommand LogOut
        {
            get
            {
                if(_LogOut == null)
                {
                    _LogOut = new RelayCommand(async() =>
                    {
                        await utilities.LogOut();
                        App.Current.NavigationService.ClearHistory();
                        Window.Current.Content = new Views.LoginPage(App.Current.NavigationService);
                    });
                }
                return _LogOut;
            }
        }

    }
}
