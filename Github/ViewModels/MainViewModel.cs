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
using Windows.UI.Xaml.Controls;
using System;

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

        private ObservableCollection<GroupInfoList> _groups = new ObservableCollection<GroupInfoList>();
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

        private bool _selecting = false;
        public bool selecting
        {
            get
            {
                return _selecting;
            }
            set
            {
                Set(ref _selecting, value);
            }
        }

        private List<object> _selectedItems;
        public List<object> selectedItems
        {
            get
            {
                if (_selectedItems == null)
                {
                    _selectedItems = new List<object>();
                }
                return _selectedItems;
            }
            set
            {
                Set(ref _selectedItems, value);
            }
        }

        private RelayCommand<object> _SelectionChanged;
        public RelayCommand<object> SelectionChanged
        {
            get
            {
                if (_SelectionChanged == null)
                {
                    _SelectionChanged = new RelayCommand<object>((e) =>
                    {
                        var args = e as SelectionChangedEventArgs;
                        foreach (var item in args.AddedItems)
                        {
                            selectedItems.Add(item);
                        }
                        foreach (var item in args.RemovedItems)
                        {
                            selectedItems.Remove(item);
                        }
                    });
                }

                return _SelectionChanged;
            }
        }

        private RelayCommand _RefreshFeed;
        public RelayCommand RefreshFeed
        {
            get
            {
                if (_RefreshFeed == null)
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
                if (_MarkNotificationRead == null)
                {
                    _MarkNotificationRead = new RelayCommand<object>(async (e) =>
                   {
                       if (e != null)
                       {
                           var swipeArgs = e as ItemSwipeEventArgs;
                           Octokit.Notification readNotification = swipeArgs.SwipedItem as Octokit.Notification;
                           try
                           {
                               constants.g_client.Activity.Notifications.MarkAsRead(int.Parse(readNotification.Id));
                               var n_raw = notifications;
                               n_raw.Remove(readNotification);
                               notifications = null;
                               notifications = n_raw;
                               GroupList();
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

        private RelayCommand _SelectItems;
        public RelayCommand SelectItems
        {
            get
            {
                if (_SelectItems == null)
                {
                    _SelectItems = new RelayCommand(() =>
                    {
                        selecting = true;
                    });
                }
                return _SelectItems;
            }
        }

        private RelayCommand _DeleteItems;
        public RelayCommand DeleteItems
        {
            get
            {
                if (_DeleteItems == null)
                {
                    _DeleteItems = new RelayCommand(async () =>
                    {
                        foreach (var item in selectedItems)
                        {
                            if (item != null)
                            {
                                var notification = item as Octokit.Notification;
                                notifications.Remove(notification);                                
                                constants.g_client.Activity.Notifications.MarkAsRead(int.Parse(notification.Id));
                            }
                        }
                        var n_raw = notifications;
                        notifications = null;
                        notifications = n_raw;
                        selectedItems = null;
                        selecting = false;
                        GroupList();
                    });
                }
                return _DeleteItems;
            }
        }

        private RelayCommand _CancelSelection;
        public RelayCommand CancelSelection
        {
            get
            {
                if (_CancelSelection == null)
                {
                    _CancelSelection = new RelayCommand(() => {
                        selectedItems = null;
                        selecting = false;
                    });
                }
                return _CancelSelection;
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
                var n_list = await constants.g_client.Activity.Notifications.GetAllForCurrent(new Octokit.NotificationsRequest { Before = DateTime.Now });
                notifications = null;
                notifications = n_list.ToList();
                GroupList();
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
        }

        private void GroupList()
        {
            groups.Clear();
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
    } 
}
