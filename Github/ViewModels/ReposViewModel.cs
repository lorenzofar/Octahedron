using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using GalaSoft.MvvmLight.Command;
using Github.Models;
using System.Collections.ObjectModel;
using Octokit;
using Windows.UI.Xaml.Navigation;
using Helper;

namespace Github.ViewModels
{
    public class ReposViewModel : ViewModelBase
    {
        private List<Repository> _repos;
        public List<Repository> repos
        {
            get
            {
                return _repos;
            }
            set
            {
                Set(ref _repos, value);
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

        private bool _loading = false;
        public bool loading
        {
            get
            {
                return _loading;
            }
            set
            {
                Set(ref _loading, value);
            }
        }

        private RelayCommand _ShowFilterMenu;
        public RelayCommand ShowFilterMenu
        {
            get
            {
                if(_ShowFilterMenu == null)
                {
                    _ShowFilterMenu = new RelayCommand(() =>
                    {

                    });
                }
                return _ShowFilterMenu;
            }
        }

        private RelayCommand _ShowSortMenu;
        public RelayCommand ShowSortMenu
        {
            get
            {
                if (_ShowSortMenu == null)
                {
                    _ShowSortMenu = new RelayCommand(() =>
                    {

                    });
                }
                return _ShowSortMenu;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadRepos();
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadRepos()
        {
            loading = true;
            var r_list = await constants.g_client.Repository.GetAllForCurrent();
            repos = null;
            repos = r_list.ToList();
            GroupList();
            loading = false;
        }

        private void GroupList()
        {
            groups.Clear();
            var query = from item in repos
                        group item by item.Name[0].ToString().ToUpper() into r
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
