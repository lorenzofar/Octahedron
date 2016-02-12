using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {            
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            //App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), "lorenzofar");
            return base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
