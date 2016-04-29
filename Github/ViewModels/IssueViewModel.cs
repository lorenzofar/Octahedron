using Helper;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class IssueViewModel : ViewModelBase
    {
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

        private Issue _issue;
        public Issue issue
        {
            get
            {
                return _issue;
            }
            set
            {
                Set(ref _issue, value);
            }
        }

        private IReadOnlyList<IssueComment> _comments;
        public IReadOnlyList<IssueComment> comments
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

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if(parameter != null)
            {
                try
                {
                    loading = true;
                    string[] issueData = parameter.ToString().Split('/');
                    issue = await constants.g_client.Issue.Get(issueData[0], issueData[1], int.Parse(issueData[2]));
                    comments = await constants.g_client.Issue.Comment.GetAllForIssue(issueData[0], issueData[1], int.Parse(issueData[2]));
                    loading = false;
                }
                catch
                {
                   await communications.ShowDialog("login_error", "error");
                    loading = false;
                }
            }
            return;
        }
    }
}