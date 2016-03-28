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

        private int _sortIndex = 0;
        public int sortIndex
        {
            get
            {
                return _sortIndex;
            }
            set
            {
                Set(ref _sortIndex, value);
            }
        }

        private int _filterIndex = 0;
        public int filterIndex
        {
            get
            {
                return _filterIndex;
            }
            set
            {
                Set(ref _filterIndex, value);
            }
        }

        private RelayCommand<object> _SelectFilter;
        public RelayCommand<object> SelectFilter
        {
            get
            {
                if (_SelectFilter == null)
                {
                    _SelectFilter = new RelayCommand<object>((parameter) =>
                    {
                        int index = int.Parse(parameter.ToString());
                        filterIndex = index;
                        LoadRepos();
                    });
                }
                return _SelectFilter;
            }
        }

        private RelayCommand<object> _SelectSort;
        public RelayCommand<object> SelectSort
        {
            get
            {
                if (_SelectSort == null)
                {
                    _SelectSort = new RelayCommand<object>((parameter) =>
                    {
                        int index = int.Parse(parameter.ToString());
                        sortIndex = index;
                        LoadRepos();
                    });
                }
                return _SelectSort;
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
            var r_list = await constants.g_client.Repository.GetAllForCurrent(new RepositoryRequest { Type = repoType });
            repos = null;
            repos = r_list.ToList();
            GroupList();
            loading = false;
        }

        private RepositoryType repoType
        {
            get
            {
                switch (filterIndex)
                {
                    default:
                    case 0:
                        return RepositoryType.All;
                    case 1:
                        return RepositoryType.Public;
                    case 2:
                        return RepositoryType.Private;
                    case 3:
                        return RepositoryType.Member;
                    case 4:
                        return RepositoryType.Owner;
                }
            }
        }

        private void GroupList()
        {
            groups.Clear();
            var query = from item in repos
                        group item by groupRule(item) into r
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

        private object groupRule(Repository repo)
        {
            switch (_sortIndex)
            {
                default:
                case 0: //SORT BY NAME
                    return repo.Name[0].ToString().ToUpper();
                case 1: //SORT BY LANGUAGE
                    return repo.Language;
            }
        }
    }
}
