﻿using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
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
                if(_AddLabel == null)
                {
                    _AddLabel = new RelayCommand(async() =>
                    {
                        var label = new NewLabel(labelName, utilities.ConvertColorToHex(labelColor)) { Name = labelName, Color = utilities.ConvertColorToHex(labelColor) };
                        try
                        {
                            await constants.g_client.Issue.Labels.Create(issueData[0], issueData[1], label);
                        }
                        catch { } //LABEL ALREADY EXISTS
                        string[] labels = { label.Name };
                        await constants.g_client.Issue.Labels.AddToIssue(issueData[0], issueData[1],int.Parse(issueData[2]),labels);
                        labelName = "";
                        LoadData();
                    }, () => !string.IsNullOrWhiteSpace(labelName));
                }
                return _AddLabel;
            }
        }


        #endregion;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                issueData = parameter.ToString().Split('/');
                LoadData();
            }
            return;
        }

        private async void LoadData()
        {
            try
            {
                loading = true;
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
    }
}