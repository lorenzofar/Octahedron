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
using GalaSoft.MvvmLight.Messaging;

namespace Octahedron.ViewModels
{
    public class RepoDetailViewModel : ViewModelBase
    {
        private bool _owner;
        public bool owner
        {
            get
            {
                return _owner;
            }
            set
            {
                Set(ref _owner, value);
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

        private int _milestonesIndex = 1;
        public int milestonesIndex
        {
            get
            {
                return _milestonesIndex;
            }
            set
            {
                Set(ref _milestonesIndex, value);
            }
        }

        private ItemStateFilter issuesState
        {
            get
            {
                switch (issuesIndex)
                {
                    default:
                    case 0:
                        return ItemStateFilter.All;
                    case 1:
                        return ItemStateFilter.Open;
                    case 2:
                        return ItemStateFilter.Closed;
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

        private ItemStateFilter pullsState
        {
            get
            {
                switch (pullsIndex)
                {
                    default:
                    case 0:
                        return ItemStateFilter.All;
                    case 1:
                        return ItemStateFilter.Open;
                    case 2:
                        return ItemStateFilter.Closed;                        
                }
            }
        }

        private ItemStateFilter milestonesState
        {
            get
            {
                switch (milestonesIndex)
                {
                    default:
                    case 0:
                        return ItemStateFilter.All;
                    case 1:
                        return ItemStateFilter.Open;
                    case 2:
                        return ItemStateFilter.Closed;
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

        private IReadOnlyList<Milestone> _milestonesList;
        public IReadOnlyList<Milestone> milestonesList
        {
            get
            {
                return _milestonesList;
            }
            set
            {
                Set(ref _milestonesList, value);
            }
        }

        private IReadOnlyList<RepositoryLanguage> _languages;
        public IReadOnlyList<RepositoryLanguage> languages
        {
            get
            {
                return _languages;
            }
            set
            {
                Set(ref _languages, value);
            }
        }

        private IReadOnlyList<User> _collaborators;
        public IReadOnlyList<User> collaborators
        {
            get
            {
                return _collaborators;
            }
            set
            {
                Set(ref _collaborators, value);
            }
        }

        private string _readme;
        public string readme
        {
            get
            {
                return _readme;
            }
            set
            {
                Set(ref _readme, value);
                Messenger.Default.Send<MvvmMessaging.ReadmeMessage>(new MvvmMessaging.ReadmeMessage { html = _readme });
            }
        }

        private IReadOnlyList<RepositoryContent> _content;
        public IReadOnlyList<RepositoryContent> content
        {
            get
            {
                return _content;
            }
            set
            {
                Set(ref _content, value);
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
                            if (args.ClickedItem.GetType() == typeof(User))
                            {
                                App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), ((User)args.ClickedItem).Login);
                            }
                            else
                            {
                                App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), ((RepositoryContributor)args.ClickedItem).Login);
                            }
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

        private RelayCommand<object> _FilterMilestones;
        public RelayCommand<object> FilterMilestones
        {
            get
            {
                if (_FilterMilestones == null)
                {
                    _FilterMilestones = new RelayCommand<object>((index) =>
                    {
                        milestonesIndex = int.Parse(index.ToString());
                        LoadRepo(repo.FullName);
                    });
                }
                return _FilterMilestones;
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

        private RelayCommand<object> _OpenPull;
        public RelayCommand<object> OpenPull
        {
            get
            {
                if (_OpenPull == null)
                {
                    _OpenPull = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        var pull = args.ClickedItem as PullRequest;
                        string pullData = $"{repo.Owner.Login}/{repo.Name}/{pull.Number}";
                        App.Current.NavigationService.Navigate(typeof(Views.PullPage), pullData);
                    });
                }
                return _OpenPull;
            }
        }

        private RelayCommand _AddIssue;
        public RelayCommand AddIssue
        {
            get
            {
                if(_AddIssue == null)
                {
                    _AddIssue = new RelayCommand(() =>
                    {
                        string repoData = $"{repo.Owner.Login}/{repo.Name}/{repo.Id}";
                        App.Current.NavigationService.Navigate(typeof(Views.NewIssuePage), repoData);
                    });
                }
                return _AddIssue;
            }
        }

        private RelayCommand<object> _OpenContent;
        public RelayCommand<object> OpenContent
        {
            get
            {
                if(_OpenContent == null)
                {
                    _OpenContent = new RelayCommand<object>(async(e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        var item = args.ClickedItem as RepositoryContent;
                        if(item.Type == ContentType.Dir)
                        {
                            content = await constants.g_client.Repository.Content.GetAllContents(repo.Owner.Login, repo.Name, item.Path);
                        }
                    });
                }
                return _OpenContent;
            }
        }

        private RelayCommand _GoUpContent;
        public RelayCommand GoUpContent
        {
            get
            {
                if(_GoUpContent == null)
                {
                    _GoUpContent = new RelayCommand(async() =>
                    {
                        var rawPath = content[0].Path;                        
                        var tempPath = $"./{rawPath.Replace($"/{content[0].Name}", "")}";
                        var path_split = tempPath.Split('/');
                        var path = "";
                        for(int i = 0; i < path_split.Length - 1; i++)
                        {
                            path += $"{path_split[i]}/";
                        }
                        content = await constants.g_client.Repository.Content.GetAllContents(repo.Owner.Login, repo.Name, path);
                    });
                }
                return _GoUpContent;
            }
        }
        #endregion

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                if (repo == null || parameter.ToString() != $"{repo.Owner.Login}/{repo.Name}")
                {
                    System.Diagnostics.Debug.WriteLine(App.Current.NavigationService.CurrentPageType);
                    LoadRepo(parameter);
                }
            }
            return Task.CompletedTask;
        }

        private async void LoadRepo(object info)
        {
            loading = true;
            try
            {
                string[] repoInfo = info.ToString().Split('/');
                repo = await constants.g_client.Repository.Get(repoInfo[0], repoInfo[1]);
                owner = repo.Owner.Login == (await constants.g_client.User.Current()).Login ? true : false;
                watched = await constants.g_client.Activity.Watching.CheckWatched(repo.Owner.Login, repo.Name);
                starred = await constants.g_client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);
                issues = await constants.g_client.Issue.GetAllForRepository(repo.Owner.Login, repo.Name, new RepositoryIssueRequest() { State = issuesState, Filter = issuesFilter });
                pulls = await constants.g_client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name, new PullRequestRequest { State = pullsState });
                milestonesList = await constants.g_client.Issue.Milestone.GetAllForRepository(repo.Owner.Login, repo.Name, new MilestoneRequest { State = milestonesState, SortProperty = MilestoneSort.Completeness });
                contributorsList = await constants.g_client.Repository.GetAllContributors(repo.Owner.Login, repo.Name);
                content = await constants.g_client.Repository.Content.GetAllContents(repo.Owner.Login,repo.Name, "./");
                if (owner)
                {
                    collaborators = await constants.g_client.Repository.Collaborator.GetAll(repo.Owner.Login, repo.Name);
                }
                var rawReadme = await constants.g_client.Repository.Content.GetReadme(repo.Owner.Login, repo.Name);
                readme = await constants.g_client.Miscellaneous.RenderRawMarkdown(rawReadme.Content.ToString());
            }
            catch(ApiException readMeException)
            {

            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
            loading = false;
        }
    }
}
