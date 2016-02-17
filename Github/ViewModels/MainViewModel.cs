using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Helper;
using System.Linq;

namespace Github.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {            
        }

        private IReadOnlyList<Octokit.Notification> _notifications;
        public IReadOnlyList<Octokit.Notification> notifications
        {
            get
            {
                return _notifications;
            }
            set
            {
                Set(ref _notifications, value);
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadNotifications();
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadNotifications()
        {
            notifications = (await constants.g_client.Notification.GetAllForCurrent()).Where(x => x.Unread == true).ToList();
        }
    }
}
