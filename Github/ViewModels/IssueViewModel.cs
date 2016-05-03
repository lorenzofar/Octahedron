using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class IssueViewModel : ViewModelBase
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

        #region COMMENTS
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

        private string _comment;
        public string comment
        {
            get
            {
                return _comment;
            }
            set
            {
                Set(ref _comment, value);
                SendComment.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _SendComment;
        public RelayCommand SendComment
        {
            get
            {
                if(_SendComment == null)
                {
                    _SendComment = new RelayCommand(async() =>
                    {
                        await constants.g_client.Issue.Comment.Create(issueData[0], issueData[1], int.Parse(issueData[2]), comment);
                        comment = "";
                        LoadComments();
                    }, () => !string.IsNullOrWhiteSpace(comment));
                }
                return _SendComment;
            }
        }
        #endregion

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                try
                {
                    loading = true;
                    issueData = parameter.ToString().Split('/');
                    issue = await constants.g_client.Issue.Get(issueData[0], issueData[1], int.Parse(issueData[2]));
                    LoadComments();
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

        private async void LoadComments()
        {
            comments = await constants.g_client.Issue.Comment.GetAllForIssue(issueData[0], issueData[1], int.Parse(issueData[2]));
        }
    }
}