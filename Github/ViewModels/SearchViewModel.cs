using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Helper;
using Template10.Mvvm;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace Github.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private bool _loading;
        public bool loading { get { return _loading; } set { Set(ref _loading, value); } }
        
        private string _query;
        public string query
        {
            get
            {
                return _query;
            }
            set
            {
                Set(ref _query, value);
                Search.RaiseCanExecuteChanged();
            }
        }

        private IReadOnlyList<SearchCode> _codeResult;
        private IReadOnlyList<User> _usersResult;
        private IReadOnlyList<Repository> _reposResult;
        private IReadOnlyList<Issue> _issuesResult;
        public IReadOnlyList<SearchCode> codeResult { get { return _codeResult; } set { Set(ref _codeResult, value); } }
        public IReadOnlyList<User> usersResult { get { return _usersResult; } set { Set(ref _usersResult, value); } }
        public IReadOnlyList<Repository> reposResult { get { return _reposResult; } set { Set(ref _reposResult, value); } }
        public IReadOnlyList<Issue> issuesResult { get { return _issuesResult; } set { Set(ref _issuesResult, value); } }

        private RelayCommand<object> _KeyPressed;
        public RelayCommand<object> KeyPressed
        {
            get
            {
                if (_KeyPressed == null)
                {
                    _KeyPressed = new RelayCommand<object>((e) =>
                    {
                        var args = e as KeyRoutedEventArgs;
                        if (args.Key == Windows.System.VirtualKey.Enter)
                        {
                            Search.Execute(null);
                        }
                    });
                }
                return _KeyPressed;
            }
        }

        private RelayCommand _Search;
        public RelayCommand Search
        {
            get
            {
                if (_Search == null)
                {
                    _Search = new RelayCommand(async () =>
                    {
                        loading = true;
                        reposResult = (await constants.g_client.Search.SearchRepo(new SearchRepositoriesRequest(query))).Items;
                        //codeResult = (await constants.g_client.Search.SearchCode(new SearchCodeRequest(query))).Items;
                        issuesResult = (await constants.g_client.Search.SearchIssues(new SearchIssuesRequest(query))).Items;
                        usersResult = (await constants.g_client.Search.SearchUsers(new SearchUsersRequest(query))).Items;
                        loading = false;
                    }, () => !string.IsNullOrWhiteSpace(query));
                }
                return _Search;
            }
        }

        private RelayCommand<object> _OpenRepo;
        public RelayCommand<object> OpenRepo
        {
            get
            {
                if (_OpenRepo == null)
                {
                    _OpenRepo = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        if (args != null && args.ClickedItem != null)
                        {
                            var repo = args.ClickedItem as Repository;
                            App.Current.NavigationService.Navigate(typeof(Views.RepoDetailPage), repo.FullName);
                        }
                    });
                }
                return _OpenRepo;
            }
        }

        private RelayCommand<object> _OpenIssue;
        public RelayCommand<object> OpenIssue
        {
            get
            {
                if (_OpenIssue == null)
                {
                    _OpenIssue = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        if (args != null && args.ClickedItem != null)
                        {
                            var issue = args.ClickedItem as Issue;
                            string url = issue.Url.ToString();
                            url = url.Replace("https://api.github.com/repos/", "");
                            url = url.Replace("/issues", "");
                            var data = url.Split('/');
                            string issueData = $"{data[0]}/{data[1]}/{issue.Number}";
                            App.Current.NavigationService.Navigate(typeof(Views.IssuePage), issueData);
                        }
                    });
                }
                return _OpenIssue;
            }
        }

        private RelayCommand<object> _OpenUser;
        public RelayCommand<object> OpenUser
        {
            get
            {
                if (_OpenUser == null)
                {
                    _OpenUser = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        if (args != null && args.ClickedItem != null)
                        {
                            var user = args.ClickedItem as User;
                            App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), user.Login);
                        }
                    });
                }
                return _OpenUser;
            }
        }
    }


}
