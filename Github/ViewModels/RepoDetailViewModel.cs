using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Helper;
using Windows.UI.Xaml.Navigation;
using Octokit;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;

namespace Github.ViewModels
{
    public class RepoDetailViewModel : ViewModelBase
    {
        private bool owner;

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

        private Repository _repo;
        public Repository repo
        {
            get
            {
                return _repo;
            }
            set
            {
                Set(ref _repo, value);
            }
        }

        private bool _starred;
        public bool starred
        {
            get
            {
                return _starred;
            }
            set
            {
                Set(ref _starred, value);
            }
        }

        private bool _watched;
        public bool watched
        {
            get
            {
                return _watched;
            }
            set
            {
                Set(ref _watched, value);
            }
        }

        private int _pivotIndex = 0;
        public int pivotIndex
        {
            get
            {
                return _pivotIndex;
            }
            set
            {
                Set(ref _pivotIndex, value);
            }
        }

        private int _issuesIndex = 1;
        public int issuesIndex
        {
            get
            {
                return _issuesIndex;
            }
            set
            {
                Set(ref _issuesIndex, value);
            }
        }

        private int _issuesFilterIndex = 0;
        public int issuesFilterIndex
        {
            get
            {
                return _issuesFilterIndex;
            }
            set
            {
                Set(ref _issuesFilterIndex, value);
            }
        }

        private int _pullsIndex = 1;
        public int pullsIndex
        {
            get
            {
                return _pullsIndex;
            }
            set
            {
                Set(ref _pullsIndex, value);
            }
        }

        private ItemState issuesState
        {
            get
            {
                switch (issuesIndex)
                {
                    default:
                    case 0:
                        return ItemState.All;
                    case 1:
                        return ItemState.Open;
                    case 2:
                        return ItemState.Closed;
                }
            }
        }

        private IssueFilter issuesFilter
        {
            get
            {
                switch (issuesFilterIndex)
                {
                    default:
                    case 0:
                        return IssueFilter.All;
                    case 1:
                        return IssueFilter.Assigned;
                    case 2:
                        return IssueFilter.Created;
                    case 3:
                        return IssueFilter.Mentioned;
                    case 4:
                        return IssueFilter.Subscribed;
                }
            }
        }

        private ItemState pullsState
        {
            get
            {
                switch (pullsIndex)
                {
                    default:
                    case 0:
                        return ItemState.All;
                    case 1:
                        return ItemState.Open;
                    case 2:
                        return ItemState.Closed;                        
                }
            }
        }

        private IReadOnlyList<Issue> _issues;
        public IReadOnlyList<Issue> issues
        {
            get
            {
                return _issues;
            }
            set
            {
                Set(ref _issues, value);
            }
        }

        private IReadOnlyList<PullRequest> _pulls;
        public IReadOnlyList<PullRequest> pulls
        {
            get
            {
                return _pulls;
            }
            set
            {
                Set(ref _pulls, value);
            }
        }

        private IReadOnlyList<RepositoryContributor> _contributorsList;
        public IReadOnlyList<RepositoryContributor> contributorsList
        {
            get
            {
                return _contributorsList;
            }
            set
            {
                Set(ref _contributorsList, value);
            }
        }

        #region COMMANDS
        private RelayCommand _WatchRepo;
        public RelayCommand WatchRepo
        {
            get
            {
                if (_WatchRepo == null)
                {
                    _WatchRepo = new RelayCommand(async () =>
                    {
                        try
                        {

                            if (!watched)
                            {
                                await constants.g_client.Activity.Watching.UnwatchRepo(repo.Owner.Login, repo.Name);
                            }
                            else
                            {
                                await constants.g_client.Activity.Watching.WatchRepo(repo.Owner.Login, repo.Name, new NewSubscription { Subscribed = true });
                            }
                        }
                        catch
                        {
                            await communications.ShowDialog("login_error", "error");
                        }
                    });
                }
                return _WatchRepo;
            }
        }

        private RelayCommand _StarRepo;
        public RelayCommand StarRepo
        {
            get
            {
                if (_StarRepo == null)
                {
                    _StarRepo = new RelayCommand(async () =>
                    {
                        try
                        {
                            if (!starred)
                            {
                                await constants.g_client.Activity.Starring.RemoveStarFromRepo(repo.Owner.Login, repo.Name);
                            }
                            else
                            {
                                await constants.g_client.Activity.Starring.StarRepo(repo.Owner.Login, repo.Name);
                            }
                            LoadRepo(repo.FullName);
                        }
                        catch
                        {
                            await communications.ShowDialog("login_error", "error");
                        }
                    });
                }
                return _StarRepo;
            }
        }

        private RelayCommand _OpenProfile;
        public RelayCommand OpenProfile
        {
            get
            {
                if (_OpenProfile == null)
                {
                    _OpenProfile = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), repo.Owner.Login);
                    });
                }
                return _OpenProfile;
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
                            var user = args.ClickedItem as RepositoryContributor;
                            App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), user.Login);
                        }
                    });
                }
                return _OpenUser;
            }
        }

        private RelayCommand<object> _OpenParent;
        public RelayCommand<object> OpenParent
        {
            get
            {
                if (_OpenParent == null)
                {
                    _OpenParent = new RelayCommand<object>((e) =>
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.RepoDetailPage), repo.Parent.FullName);
                    });
                }
                return _OpenParent;
            }
        }

        private RelayCommand<object> _FilterIssues;
        public RelayCommand<object> FilterIssues
        {
            get
            {
                if (_FilterIssues == null)
                {
                    _FilterIssues = new RelayCommand<object>((index) =>
                    {
                        issuesIndex = int.Parse(index.ToString());
                        LoadRepo(repo.FullName);
                    });
                }
                return _FilterIssues;
            }
        }

        private RelayCommand<object> _FilterIssuesUser;
        public RelayCommand<object> FilterIssuesUser
        {
            get
            {
                if (_FilterIssuesUser == null)
                {
                    _FilterIssuesUser = new RelayCommand<object>((index) =>
                    {
                        issuesFilterIndex = int.Parse(index.ToString());
                        LoadRepo(repo.FullName);
                    });
                }
                return _FilterIssuesUser;
            }
        }

        private RelayCommand<object> _FilterPulls;
        public RelayCommand<object> FilterPulls
        {
            get
            {
                if (_FilterPulls == null)
                {
                    _FilterPulls = new RelayCommand<object>((index) =>
                    {
                        pullsIndex = int.Parse(index.ToString());
                        LoadRepo(repo.FullName);
                    });
                }
                return _FilterPulls;
            }
        }

        private RelayCommand<object> _OpenIssue;
        public RelayCommand<object> OpenIssue
        {
            get
            {
                if(_OpenIssue == null)
                {
                    _OpenIssue = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        var issue = args.ClickedItem as Issue;
                        string issueData = $"{repo.Owner.Login}/{repo.Name}/{issue.Number}";
                        App.Current.NavigationService.Navigate(typeof(Views.IssuePage), issueData);
                    });
                }
                return _OpenIssue;
            }
        }
        #endregion

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                LoadRepo(parameter);
            }
            return Task.CompletedTask;
        }

        private async void LoadRepo(object info)
        {
            try
            {
                loading = true;
                string[] repoInfo = info.ToString().Split('/');
                repo = await constants.g_client.Repository.Get(repoInfo[0], repoInfo[1]);
                owner = repo.Owner.Login == (await constants.g_client.User.Current()).Login ? true : false;
                watched = await constants.g_client.Activity.Watching.CheckWatched(repo.Owner.Login, repo.Name);
                starred = await constants.g_client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);
                issues = await constants.g_client.Issue.GetAllForRepository(repo.Owner.Login, repo.Name, new RepositoryIssueRequest() { State = issuesState, Filter = issuesFilter });
                pulls = await constants.g_client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name, new PullRequestRequest { State = pullsState });
                contributorsList = await constants.g_client.Repository.GetAllContributors(repo.Owner.Login, repo.Name);
                loading = false;
            }
            catch
            {
                loading = false;
                await communications.ShowDialog("login_error", "error");
            }
        }
    }
}
