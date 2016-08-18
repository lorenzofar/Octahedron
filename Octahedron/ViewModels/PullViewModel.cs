using Helper;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class PullViewModel : ViewModelBase
    {
        private string[] pullData { get; set; }

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

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                pullData = parameter.ToString().Split('/');
                LoadData();
            }
            return Task.CompletedTask;
        }

        private async void LoadData()
        {
            loading = true;
            owner = pullData[0] == App.user.Login;
            pull = await constants.g_client.PullRequest.Get(pullData[0], pullData[1], int.Parse(pullData[2]));
            commits = await constants.g_client.PullRequest.Commits(pullData[0], pullData[1], int.Parse(pullData[2]));
            comments = await constants.g_client.PullRequest.Comment.GetAll(pullData[0], pullData[1], int.Parse(pullData[2]));
            files = await constants.g_client.PullRequest.Files(pullData[0], pullData[1], int.Parse(pullData[2]));
            closeable = owner && pull.State != ItemState.Closed;
            loading = false;
        }
    }
}