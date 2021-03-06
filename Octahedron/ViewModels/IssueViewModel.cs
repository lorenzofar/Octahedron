﻿using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using UniversalMarkdown;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class IssueViewModel : ViewModelBase
    {
        public Dictionary<int, string> issueData { get; set; }

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

        private bool _creator;
        public bool creator
        {
            get
            {
                return _creator;
            }
            set
            {
                Set(ref _creator, value);
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

        private bool _locked;
        public bool locked
        {
            get
            {
                return _locked;
            }
            set
            {
                Set(ref _locked, value);
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
                if (_CloseIssue == null)
                {
                    _CloseIssue = new RelayCommand(async () =>
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
                if (_AssignIssue == null)
                {
                    _AssignIssue = new RelayCommand<string>(async (string assignee) =>
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

        private RelayCommand<object> _AddToMilestone;
        public RelayCommand<object> AddToMilestone
        {
            get
            {
                if (_AddToMilestone == null)
                {
                    _AddToMilestone = new RelayCommand<object>(async (m) =>
                    {
                        var milestone = m as Milestone;
                        await constants.g_client.Issue.Update(issueData[0], issueData[1], int.Parse(issueData[2]), new IssueUpdate { Milestone = milestone.Number });
                        LoadData();
                    });
                }
                return _AddToMilestone;
            }
        }

        private RelayCommand _LockIssue;
        public RelayCommand LockIssue
        {
            get
            {
                if (_LockIssue == null)
                {
                    _LockIssue = new RelayCommand(async () =>
                    {
                        if (locked)
                        {
                            await constants.g_client.Issue.Lock(issueData[0], issueData[1], int.Parse(issueData[2]));
                        }
                        else
                        {
                            await constants.g_client.Issue.Unlock(issueData[0], issueData[1], int.Parse(issueData[2]));
                        }
                        LoadData();
                    });
                }
                return _LockIssue;
            }
        }

        private RelayCommand _EditIssue;
        public RelayCommand EditIssue
        {
            get
            {
                if (_EditIssue == null)
                {
                    _EditIssue = new RelayCommand(() =>
                    {
                        var request = new Dictionary<string, string>();
                        request.Add("kind", "1"); //SET REQUEST KIND TO EDIT
                        request.Add("owner", issueData[0]);
                        request.Add("name", issueData[1]);
                        request.Add("id", null);
                        request.Add("issueNumber", issue.Number.ToString());
                        App.Current.NavigationService.Navigate(typeof(Views.NewIssuePage), request);
                    });
                }
                return _EditIssue;
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

        private RelayCommand _OpenCreator;
        public RelayCommand OpenCreator
        {
            get
            {
                if (_OpenCreator == null)
                {
                    _OpenCreator = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), issue.User.Login);
                    });
                }
                return _OpenCreator;
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
                if (_RemoveComment == null)
                {
                    _RemoveComment = new RelayCommand<object>(async (object comment) =>
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
                if (_EditComment == null)
                {
                    _EditComment = new RelayCommand<object>(async (object commentInfo) =>
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
                UpdateSuggestions();
                AddLabel.RaiseCanExecuteChanged();
            }
        }

        private IReadOnlyList<Label> rawSuggestedLabels { get; set; }

        private IReadOnlyList<Label> _suggestedLabels;
        public IReadOnlyList<Label> suggestedLabels
        {
            get
            {
                return _suggestedLabels;
            }
            set
            {
                Set(ref _suggestedLabels, value);
            }
        }

        private void UpdateSuggestions()
        {
            suggestedLabels = rawSuggestedLabels.Where(x => x.Name.Contains(labelName)).ToList();
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
                if (_RemoveLabel == null)
                {
                    _RemoveLabel = new RelayCommand<object>(async (object label) =>
                    {
                        await constants.g_client.Issue.Labels.RemoveFromIssue(issueData[0], issueData[1], int.Parse(issueData[2]), label.ToString());
                        LoadData();
                    });
                }
                return _RemoveLabel;
            }
        }

        private RelayCommand<object> _ChooseLabelSuggestion;
        public RelayCommand<object> ChooseLabelSuggestion
        {
            get
            {
                if (_ChooseLabelSuggestion == null)
                {
                    _ChooseLabelSuggestion = new RelayCommand<object>((e) =>
                    {
                        var args = e as AutoSuggestBoxSuggestionChosenEventArgs;
                        var label = args.SelectedItem as Label;
                        labelName = label.Name;
                        labelColor = utilities.ConvertHexToColor(label.Color);
                    });
                }
                return _ChooseLabelSuggestion;
            }
        }
        #endregion;                

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null && mode != NavigationMode.Back)
            {
                issueData = parameter as Dictionary<int, string>;
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
                locked = issue.Locked;
                creator = issue.User.Login == App.user.Login;
                closeable = creator && issue.State != ItemState.Closed;
                rawSuggestedLabels = await constants.g_client.Issue.Labels.GetAllForRepository(issueData[0], issueData[1]);
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