using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Helper;
using Template10.Mvvm;

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
    }
}
