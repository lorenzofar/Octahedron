using Template10.Mvvm;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Helper;

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

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if(parameter != null)
            {
                try
                {
                    loading = true;
                    string[] issueData = parameter.ToString().Split('/');
                    issue = await constants.g_client.Issue.Get(issueData[0], issueData[1], int.Parse(issueData[2]));
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
