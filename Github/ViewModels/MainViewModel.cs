using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Helper;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Universal.UI.Xaml.Controls;

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

        private RelayCommand _RefreshFeed;
        public RelayCommand RefreshFeed
        {
            get
            {
                if(_RefreshFeed == null)
                {
                    _RefreshFeed = new RelayCommand(LoadNotifications);
                }
                return _RefreshFeed;
            }
        }

        private RelayCommand<object> _MarkNotificationRead;
        public RelayCommand<object> MarkNotificationRead
        {
            get
            {
                if(_MarkNotificationRead == null)
                {
                    _MarkNotificationRead = new RelayCommand<object>( async(e) =>
                    {
                        if (e != null)
                        {
                            var swipeArgs = e as ItemSwipeEventArgs;
                            Octokit.Notification readNotification = swipeArgs.SwipedItem as Octokit.Notification;
                            try
                            {
                                await constants.g_client.Notification.MarkAsRead(int.Parse(readNotification.Id));
                                LoadNotifications();
                            }
                            catch
                            {
                                await communications.ShowDialog("login_error", "error");
                            }

                        }
                    });
                }
                return _MarkNotificationRead;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadNotifications();
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadNotifications()
        {
            try
            {
                notifications = (await constants.g_client.Notification.GetAllForCurrent()).Where(x => x.Unread == true).ToList();
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
        }

        
    }
}
