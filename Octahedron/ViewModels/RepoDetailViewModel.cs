using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Helper;
using Octahedron.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using UniversalMarkdown;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class RepoDetailViewModel : ViewModelBase
    {
        private Dictionary<int, string> repoData { get; set; }

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

        private string _loadingProgress;
        public string loadingProgress
        {
            get
            {
                return _loadingProgress;
            }
            set
            {
                Set(ref _loadingProgress, value);
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

        #region PAGES
        private int _issuesPage = 1;
        public int issuesPage
        {
            get
            {
                return _issuesPage;
            }
            set
            {
                Set(ref _issuesPage, value);
                PreviousIssuesPage.RaiseCanExecuteChanged();
                NextIssuesPage.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _PreviousIssuesPage;
        public RelayCommand PreviousIssuesPage
        {
            get
            {
                if(_PreviousIssuesPage == null)
                {
                    _PreviousIssuesPage = new RelayCommand(async() =>
                    {
                        issuesPage--;
                        loading = true;
                        await LoadIssues();
                        loading = false;
                    }, () => issuesPage > 1);
                }
                return _PreviousIssuesPage;
            }
        }
        private RelayCommand _NextIssuesPage;
        public RelayCommand NextIssuesPage
        {
            get
            {
                if (_NextIssuesPage == null)
                {
                    _NextIssuesPage = new RelayCommand(async() =>
                    {
                        issuesPage++;
                        loading = true;
                        await LoadIssues();
                        loading = false;
                    }, () => 50*issuesPage < repo.OpenIssuesCount);
                }
                return _NextIssuesPage;
            }
        }
        #endregion

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

        private IReadOnlyList<GitHubCommit> _commits;
        public IReadOnlyList<GitHubCommit> commits
        {
            get
            {
                return _commits;
            }
            set
            {
                Set(ref _commits, value);
            }
        }

        private ObservableCollection<GroupInfoList> _commitGroups = new ObservableCollection<GroupInfoList>();
        public ObservableCollection<GroupInfoList> commitGroups
        {
            get
            {
                return _commitGroups;
            }
            set
            {
                Set(ref _commitGroups, value);
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
                //Messenger.Default.Send<MvvmMessaging.ReadmeMessage>(new MvvmMessaging.ReadmeMessage { html = _readme });
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
                GoUpContent.RaiseCanExecuteChanged();
            }
        }

        private IReadOnlyList<Branch> _branches;
        public IReadOnlyList<Branch> branches
        {
            get
            {
                return _branches;
            }
            set
            {
                Set(ref _branches, value);
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
                            LoadRepo();
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
                        var repoData = new Dictionary<int, string>();
                        repoData.Add(0, repo.Parent.Owner.Login);
                        repoData.Add(1, repo.Parent.Name);
                        App.Current.NavigationService.Navigate(typeof(Views.RepoDetailPage), repoData);
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
                    _FilterIssues = new RelayCommand<object>(async (index) =>
                    {
                        loading = true;
                        issuesIndex = int.Parse(index.ToString());
                        await LoadIssues();
                        loading = false;
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
                    _FilterIssuesUser = new RelayCommand<object>(async (index) =>
                    {
                        loading = true;
                        issuesFilterIndex = int.Parse(index.ToString());
                        await LoadIssues();
                        loading = false;
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
                    _FilterPulls = new RelayCommand<object>(async (index) =>
                    {
                        loading = true;
                        pullsIndex = int.Parse(index.ToString());
                        await LoadPulls();
                        loading = false;
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
                    _FilterMilestones = new RelayCommand<object>(async (index) =>
                    {
                        loading = true;
                        milestonesIndex = int.Parse(index.ToString());
                        await LoadMilestones();
                        loading = false;
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
                if (_OpenIssue == null)
                {
                    _OpenIssue = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        var issue = args.ClickedItem as Issue;
                        var issueData = new Dictionary<int, string>();
                        issueData.Add(0, repo.Owner.Login);
                        issueData.Add(1, repo.Name);
                        issueData.Add(2, issue.Number.ToString());
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
                        var pullData = new Dictionary<int, string>();
                        pullData.Add(0, repo.Owner.Login);
                        pullData.Add(1, repo.Name);
                        pullData.Add(2, pull.Number.ToString());
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
                if (_AddIssue == null)
                {
                    _AddIssue = new RelayCommand(() =>
                    {
                        var request = new Dictionary<string, string>();
                        request.Add("kind", "0"); //SET REQUEST KIND TO NEW
                        request.Add("owner", repo.Owner.Login);
                        request.Add("name", repo.Name);
                        request.Add("id", repo.Id.ToString());
                        request.Add("issueNumber", null);
                        App.Current.NavigationService.Navigate(typeof(Views.NewIssuePage), request);
                    });
                }
                return _AddIssue;
            }
        }

        private RelayCommand _AddMilestone;
        public RelayCommand AddMilestone
        {
            get
            {
                if (_AddMilestone == null)
                {
                    _AddMilestone = new RelayCommand(() =>
                    {
                        var request = new Dictionary<string, string>();
                        request.Add("kind", "0"); //SET REQUEST KIND TO NEW
                        request.Add("owner", repo.Owner.Login);
                        request.Add("name", repo.Name);
                        request.Add("id", repo.Id.ToString());
                        request.Add("milestoneNumber", null);
                        App.Current.NavigationService.Navigate(typeof(Views.NewMilestonePage), request);
                    });
                }
                return _AddMilestone;
            }
        }

        private RelayCommand<object> _OpenContent;
        public RelayCommand<object> OpenContent
        {
            get
            {
                if (_OpenContent == null)
                {
                    _OpenContent = new RelayCommand<object>(async (e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        var item = args.ClickedItem as RepositoryContent;
                        if (item.Type == ContentType.Dir)
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
                if (_GoUpContent == null)
                {
                    _GoUpContent = new RelayCommand(async () =>
                    {
                        var rawPath = content[0].Path;
                        var tempPath = $"./{rawPath.Replace($"/{content[0].Name}", "")}";
                        var path_split = tempPath.Split('/');
                        var path = "";
                        for (int i = 0; i < path_split.Length - 1; i++)
                        {
                            path += $"{path_split[i]}/";
                        }
                        content = await constants.g_client.Repository.Content.GetAllContents(repo.Owner.Login, repo.Name, path);
                    }, () =>
                    {
                        var path = content[0].Path;
                        return $"./{content[0].Name}" != $"./{content[0].Path}";
                    });
                }
                return _GoUpContent;
            }
        }

        private RelayCommand<object> _HandleReadmeClick;
        public RelayCommand<object> HandleReadmeClick
        {
            get
            {
                if (_HandleReadmeClick == null)
                {
                    _HandleReadmeClick = new RelayCommand<object>(async (e) =>
                    {
                        var args = e as OnMarkdownLinkTappedArgs;
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(args.Link));
                    });
                }
                return _HandleReadmeClick;
            }
        }

        private RelayCommand _Refresh;
        public RelayCommand Refresh
        {
            get
            {
                if(_Refresh == null)
                {
                    _Refresh = new RelayCommand(() =>
                    {
                        LoadRepo();
                    });
                }
                return _Refresh;
            }
        }
        #endregion

        #region LISTS LOADING
        private async Task LoadIssues()
        {
            loadingProgress = constants.r_loader.GetString("issues_progress");
            issues = await constants.g_client.Issue.GetAllForRepository(repo.Owner.Login, repo.Name, new RepositoryIssueRequest() { State = issuesState}, new ApiOptions { PageSize = 50, PageCount = 1, StartPage = issuesPage });
        }

        private async Task LoadPulls()
        {
            loadingProgress = constants.r_loader.GetString("pulls_progress");
            pulls = await constants.g_client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name, new PullRequestRequest { State = pullsState }, new ApiOptions { PageSize = 50, PageCount = 1 });
        }

        private async Task LoadMilestones()
        {
            loadingProgress = constants.r_loader.GetString("milestones_progress");
            milestonesList = await constants.g_client.Issue.Milestone.GetAllForRepository(repo.Owner.Login, repo.Name, new MilestoneRequest { State = milestonesState, SortProperty = MilestoneSort.Completeness }, new ApiOptions { PageSize = 50, PageCount = 1 });
        }

        private async Task LoadContributors()
        {
            loadingProgress = constants.r_loader.GetString("contributors_progress");
            contributorsList = await constants.g_client.Repository.GetAllContributors(repo.Owner.Login, repo.Name);
            if (owner)
            {
                collaborators = await constants.g_client.Repository.Collaborator.GetAll(repo.Owner.Login, repo.Name);
            }
        }

        private async Task LoadCommits()
        {
            loadingProgress = constants.r_loader.GetString("commits_progress");
            commits = await constants.g_client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name, new ApiOptions { PageSize = 50, PageCount = 1 });
            GroupCommitsList();
        }

        private async Task LoadContent()
        {
            loadingProgress = constants.r_loader.GetString("code_progress");
            content = await constants.g_client.Repository.Content.GetAllContents(repo.Owner.Login, repo.Name, "./");
        }
        #endregion

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                repoData = parameter as Dictionary<int, string>;
                if (repo == null || (repoData[0] != repo.Owner.Login || repoData[1] != repo.Name))
                {
                    LoadRepo();
                }
            }
            return Task.CompletedTask;
        }

        private async void LoadRepo()
        {
            loading = true;
            issuesIndex = 1;
            issuesFilterIndex = 0;
            pullsIndex = 1;
            milestonesIndex = 1;
            readme = string.Empty;
            try
            {
                loadingProgress = constants.r_loader.GetString("info_progress");
                repo = await constants.g_client.Repository.Get(repoData[0], repoData[1]);
                owner = repo.Owner.Login == (await constants.g_client.User.Current()).Login ? true : false;
                watched = await constants.g_client.Activity.Watching.CheckWatched(repo.Owner.Login, repo.Name);
                starred = await constants.g_client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);
                loadingProgress = constants.r_loader.GetString("branches_progress");
                branches = await constants.g_client.Repository.Branch.GetAll(repo.Id);
                await LoadIssues();
                await LoadPulls();
                await LoadMilestones();
                await LoadContributors();
                await LoadCommits();
                await LoadContent();
                readme = (await constants.g_client.Repository.Content.GetReadme(repo.Owner.Login, repo.Name)).Content;
            }
            catch (ApiException readMeException)
            {

            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
            loading = false;
        }

        private void GroupCommitsList()
        {
            commitGroups.Clear();
            var query = from item in commits
                        group item by constants.shortDateFormatter.Format(item.Commit.Committer.Date) into r
                        orderby r.Key descending
                        select new { GroupName = r.Key, Items = r };
            foreach (var g in query)
            {
                GroupInfoList info = new GroupInfoList();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                commitGroups.Add(info);
            }
        }
    }
}
