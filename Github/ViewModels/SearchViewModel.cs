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

        private IReadOnlyList<SearchCodeResult> _codeResult;
        private IReadOnlyList<SearchUsersResult> _usersResult;
        private IReadOnlyList<SearchRepositoryResult> _reposResult;
        private IReadOnlyList<SearchIssuesResult> _issuesResult;
        public IReadOnlyList<SearchCodeResult> codeResult { get { return _codeResult; } set { Set(ref _codeResult, value); } }
        public IReadOnlyList<SearchUsersResult> usersResult { get { return _usersResult; } set { Set(ref _usersResult, value); } }
        public IReadOnlyList<SearchRepositoryResult> reposResult { get { return _reposResult; } set { Set(ref _reposResult, value); } }
        public IReadOnlyList<SearchIssuesResult> issuesResult { get { return _issuesResult; } set { Set(ref _issuesResult, value); } }

        private RelayCommand _Search;
        public RelayCommand Search
        {
            get
            {
                if (_Search == null)
                {
                    _Search = new RelayCommand(async () =>
                    {
                        //reposResult = await constants.g_client.Search
                    }, () => !string.IsNullOrWhiteSpace(query));
                }
                return _Search;
            }
        }
    }
}
