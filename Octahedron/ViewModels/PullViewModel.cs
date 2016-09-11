using Helper;
using Octahedron.Models;
using Octokit;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using System.Linq;

namespace Octahedron.ViewModels
{
    public class PullViewModel : ViewModelBase
    {
        private Dictionary<int, string> pullData { get; set; }

        private bool _loading;
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

        private bool _closeable;
        public bool closeable
        {
            get
            {
                return _closeable;
            }
            set
            {
                Set(ref _closeable, value);
            }
        }

        private PullRequest _pull;
        public PullRequest pull
        {
            get
            {
                return _pull;
            }
            set
            {
                Set(ref _pull, value);
            }
        }

        private IReadOnlyList<PullRequestCommit> _commits;
        public IReadOnlyList<PullRequestCommit> commits
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

        private IReadOnlyList<PullRequestReviewComment> _comments;
        public IReadOnlyList<PullRequestReviewComment> comments
        {
            get
            {
                return _comments;
            }
            set
            {
                Set(ref _comments, value);
            }
        }

        private IReadOnlyList<PullRequestFile> _files;
        public IReadOnlyList<PullRequestFile> files
        {
            get
            {
                return _files;
            }
            set
            {
                Set(ref _files, value);
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

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null && mode != NavigationMode.Back)
            {
                pullData = parameter as Dictionary<int, string>;
                LoadData();
            }
            return Task.CompletedTask;
        }

        private async void LoadData()
        {
            loading = true;
            loadingProgress = constants.r_loader.GetString("info_progress");
            owner = pullData[0] == App.user.Login;
            closeable = owner && pull.State != ItemState.Closed;
            pull = await constants.g_client.PullRequest.Get(pullData[0], pullData[1], int.Parse(pullData[2]));
            loadingProgress = constants.r_loader.GetString("commits_progress");
            commits = await constants.g_client.PullRequest.Commits(pullData[0], pullData[1], int.Parse(pullData[2]));
            GroupCommitsList();
            loadingProgress = constants.r_loader.GetString("comments_progress");
            comments = await constants.g_client.PullRequest.Comment.GetAll(pullData[0], pullData[1], int.Parse(pullData[2]));
            loadingProgress = constants.r_loader.GetString("changedFiles_progress");
            files = await constants.g_client.PullRequest.Files(pullData[0], pullData[1], int.Parse(pullData[2]));            
            loading = false;
        }

        private void GroupCommitsList()
        {
            groups.Clear();
            var query = from item in commits
                        group item by constants.shortDateFormatter.Format(item.Commit.Committer.Date) into r
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