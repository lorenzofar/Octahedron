using Helper;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class PullViewModel : ViewModelBase
    {
        private string[] issueData { get; set; }

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

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                issueData = parameter.ToString().Split('/');
                LoadData();
            }
            return Task.CompletedTask;
        }

        private async void LoadData()
        {
            loading = true;
            owner = issueData[0] == App.user;
            pull = await constants.g_client.PullRequest.Get(issueData[0], issueData[1], int.Parse(issueData[2]));
            closeable = owner && pull.State != ItemState.Closed;
            loading = false;
        }
    }
}
