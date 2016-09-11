using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class NewIssueViewModel : ViewModelBase
    {
        public static bool editing { get; set; }
        private Issue editingIssue { get; set; }

        private string[] _repoData;
        public string[] repoData
        {
            get
            {
                return _repoData;
            }
            set
            {
                Set(ref _repoData, value);
            }
        }

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

        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
                AddIssue.RaiseCanExecuteChanged();
            }
        }

        private string _body;
        public string body
        {
            get
            {
                return _body;
            }
            set
            {
                Set(ref _body, value);
            }
        }

        private RelayCommand _AddIssue;
        public RelayCommand AddIssue
        {
            get
            {
                if (_AddIssue == null)
                {
                    _AddIssue = new RelayCommand(async () =>
                    {
                        loading = true;
                        loadingProgress = constants.r_loader.GetString(editing ? "editIssue_progress" : "addIssue_progress");
                        Issue issue = new Issue();
                        if (!editing)
                        {
                            NewIssue newIssue = new NewIssue(title) { Body = body };
                            issue = await constants.g_client.Issue.Create(int.Parse(repoData[2]), newIssue);
                        }
                        else
                        {
                            var update = editingIssue.ToUpdate();
                            update.Title = title;
                            update.Body = body;
                            issue = await constants.g_client.Issue.Update(repoData[0], repoData[1], editingIssue.Number, update);
                        }
                        var issueData = new Dictionary<int, string>();
                        issueData.Add(0, repoData[0]);
                        issueData.Add(1, repoData[1]);
                        issueData.Add(2, issue.Number.ToString());
                        App.Current.NavigationService.Navigate(typeof(Views.IssuePage), issueData);
                        loading = false;
                    }, () => !string.IsNullOrWhiteSpace(title));
                }
                return _AddIssue;
            }
        }

        private RelayCommand _Cancel;
        public RelayCommand Cancel
        {
            get
            {
                if (_Cancel == null)
                {
                    _Cancel = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.GoBack();
                    });
                }
                return _Cancel;
            }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                var request = parameter as Dictionary<string, string>;
                editing = int.Parse(request["kind"].ToString()) != 0;
                repoData = new string[] { request["owner"], request["name"], request["id"] };
                if (editing)
                {
                    var issueNumber = int.Parse(request["issueNumber"]);
                    editingIssue = await constants.g_client.Issue.Get(repoData[0], repoData[1], issueNumber);
                    title = editingIssue.Title;
                    body = editingIssue.Body;
                }
            }
        }
    }
}
