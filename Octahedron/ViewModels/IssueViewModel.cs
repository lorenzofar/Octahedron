using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
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

        private IReadOnlyList<EventInfo> _events;
        public IReadOnlyList<EventInfo> events
        {
            get
            {
                return _events;
            }
            set
            {
                Set(ref _events, value);
            }
        }

        private RelayCommand _CloseIssue;
        public RelayCommand CloseIssue
        {
            get
            {
                if(_CloseIssue == null)
                {
                    _CloseIssue = new RelayCommand(async() =>
                    {
                        await constants.g_client.Issue.Update(issueData[0], issueData[1], int.Parse(issueData[2]), new IssueUpdate { State = ItemState.Closed });
                        LoadData();
                    });
                }
                return _CloseIssue;
            }
        }

        private RelayCommand<string> _AssignIssue;
        public RelayCommand<string> AssignIssue
        {
            get
            {
                if(_AssignIssue == null)
                {
                    _AssignIssue = new RelayCommand<string>(async(string assignee) =>
                    {
                        try
                        {
                            await constants.g_client.Issue.Update(issueData[0], issueData[1], int.Parse(issueData[2]), new IssueUpdate() { Assignee = assignee });
                            LoadData();
                        }
                        catch (ApiValidationException)
                        {
                            await communications.ShowDialog("invalid_user", "error");
                        }
                        catch
                        {
                            await communications.ShowDialog("login_error", "error");
                        }
                    });
                }
                return _AssignIssue;
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
                if (_SendComment == null)
                {
                    _SendComment = new RelayCommand(async () =>
                    {
                        await constants.g_client.Issue.Comment.Create(issueData[0], issueData[1], int.Parse(issueData[2]), comment);
                        comment = "";
                        LoadData();
                    }, () => !string.IsNullOrWhiteSpace(comment));
                }
                return _SendComment;
            }
        }

        private RelayCommand<object> _RemoveComment;
        public RelayCommand<object> RemoveComment
        {
            get
            {
                if(_RemoveComment == null)
                {
                    _RemoveComment = new RelayCommand<object>(async(object comment) =>
                    {
                        await constants.g_client.Issue.Comment.Delete(issueData[0], issueData[1], int.Parse(comment.ToString()));
                        LoadData();
                    });
                }
                return _RemoveComment;
            }
        }

        private RelayCommand<object> _EditComment;
        public RelayCommand<object> EditComment
        {
            get
            {
                if(_EditComment == null)
                {
                    _EditComment = new RelayCommand<object>(async(object commentInfo) =>
                    {
                        string[] info = commentInfo as string[];
                        await constants.g_client.Issue.Comment.Update(issueData[0], issueData[1], int.Parse(info[0]), info[1]);
                        LoadData();
                    });
                }
                return _EditComment;
            }
        }
        #endregion

        #region LABELS
        private Color _labelColor = Colors.Red;
        public Color labelColor
        {
            get
            {
                return _labelColor;
            }
            set
            {
                Set(ref _labelColor, value);
            }
        }

        private string _labelName;
        public string labelName
        {
            get
            {
                return _labelName;
            }
            set
            {
                Set(ref _labelName, value);
                AddLabel.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _AddLabel;
        public RelayCommand AddLabel
        {
            get
            {
                if (_AddLabel == null)
                {
                    _AddLabel = new RelayCommand(async () =>
                    {
                        var label = new NewLabel(labelName, utilities.ConvertColorToHex(labelColor)) { Name = labelName, Color = utilities.ConvertColorToHex(labelColor) };
                        try
                        {
                            await constants.g_client.Issue.Labels.Create(issueData[0], issueData[1], label);
                        }
                        catch //LABEL ALREADY EXISTS
                        {
                            var labelOld = await constants.g_client.Issue.Labels.Get(issueData[0], issueData[1], label.Name);
                            labelColor = utilities.ConvertHexToColor(labelOld.Color);
                        }
                        string[] labels = { label.Name };
                        await constants.g_client.Issue.Labels.AddToIssue(issueData[0], issueData[1], int.Parse(issueData[2]), labels);
                        labelName = "";
                        LoadData();
                    }, () => !string.IsNullOrWhiteSpace(labelName));
                }
                return _AddLabel;
            }
        }

        private RelayCommand<object> _RemoveLabel;
        public RelayCommand<object> RemoveLabel
        {
            get
            {
                if(_RemoveLabel == null)
                {
                    _RemoveLabel = new RelayCommand<object>(async(object label) =>
                    {
                        await constants.g_client.Issue.Labels.RemoveFromIssue(issueData[0], issueData[1], int.Parse(issueData[2]), label.ToString());
                        LoadData();
                    });
                }
                return _RemoveLabel;
            }
        }


        #endregion;                

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null && mode != NavigationMode.Back)
            {
                issueData = parameter.ToString().Split('/');
                LoadData();
            }
            return Task.CompletedTask;
        }

        private async void LoadData()
        {
            try
            {
                loading = true;
                loadingProgress = constants.r_loader.GetString("info_progress");
                owner = issueData[0] == App.user.Login;
                issue = await constants.g_client.Issue.Get(issueData[0], issueData[1], int.Parse(issueData[2]));
                loadingProgress = constants.r_loader.GetString("comments_progress");
                comments = await constants.g_client.Issue.Comment.GetAllForIssue(issueData[0], issueData[1], int.Parse(issueData[2]));
                loadingProgress = constants.r_loader.GetString("events_progress");
                events = (await constants.g_client.Issue.Events.GetAllForIssue(issueData[0], issueData[1], int.Parse(issueData[2])))
                    .Where(x => x.Event == EventInfoState.Assigned ||
                                x.Event == EventInfoState.Closed ||
                                x.Event == EventInfoState.Demilestoned ||
                                x.Event == EventInfoState.Labeled ||
                                x.Event == EventInfoState.Locked ||
                                x.Event == EventInfoState.Milestoned ||
                                x.Event == EventInfoState.Referenced ||
                                x.Event == EventInfoState.Renamed ||
                                x.Event == EventInfoState.Reopened ||
                                x.Event == EventInfoState.Unassigned ||
                                x.Event == EventInfoState.Unlabeled ||
                                x.Event == EventInfoState.Unlocked).ToList();
                closeable = owner && issue.State != ItemState.Closed;
                loading = false;
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
                loading = false;
            }
        }
    }
}