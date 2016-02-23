using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Helper;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using Universal.UI.Xaml.Controls;
using Github.Models;
using System.Collections.ObjectModel;

namespace Github.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {            
        }

        private List<Octokit.Notification> _notifications;
        public List<Octokit.Notification> notifications
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

        private ObservableCollection<GroupInfoList> _groups;
        public ObservableCollection<GroupInfoList> groups
        {
            get
            {
                return _groups;
            }
            set
            {
                Set(ref _groups, value);
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
                                var n_raw = notifications;
                                n_raw.Remove(readNotification);
                                notifications = null;
                                notifications = n_raw;
                                await constants.g_client.Notification.MarkAsRead(int.Parse(readNotification.Id));    
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
                groups = new ObservableCollection<GroupInfoList>();
                notifications = (await constants.g_client.Notification.GetAllForCurrent()).Where(x => x.Unread == true).ToList();
                var query = from item in notifications
                            group item by item.Repository.FullName.ToUpper() into r
                            orderby r.Key
                            select new { GroupName = r.Key, Items = r };
                foreach (var g in query)
                {
                    GroupInfoList info = new GroupInfoList();
                    info.Key = g.GroupName;
                    foreach (var item in g.Items)
                    {
                        info.Add(item);
                    }
                    groups.Add(info);
                }
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
        }

        
    }
}
