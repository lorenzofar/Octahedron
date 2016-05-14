using GalaSoft.MvvmLight.Command;
using Github.Views.Dialogs;
using Helper;
using Octokit;
using System.Collections.Generic;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;

namespace Github.ViewModels
{
    public class AssignViewModel : ViewModelBase
    {
        private string _assignSearch;
        public string assignSearch
        {
            get
            {
                return _assignSearch;
            }
            set
            {
                Set(ref _assignSearch, value);
                LoadUsers();
            }
        }

        private User _assignedUser;
        public User assignedUser
        {
            get
            {
                return _assignedUser;
            }
            set
            {
                Set(ref _assignedUser, value);
            }
        }

        private IReadOnlyList<User> _assignSuggestions;
        public IReadOnlyList<User> assignSuggestions
        {
            get
            {
                return _assignSuggestions;
            }
            set
            {
                Set(ref _assignSuggestions, value);
            }
        }

        private RelayCommand<object> _ChooseSuggestion;
        public RelayCommand<object> ChooseSuggestion
        {
            get
            {
                if (_ChooseSuggestion == null)
                {
                    _ChooseSuggestion = new RelayCommand<object>((e) =>
                    {
                        var args = e as AutoSuggestBoxSuggestionChosenEventArgs;
                        if (args != null && args.SelectedItem != null)
                        {
                            assignedUser = args.SelectedItem as User;
                            assignSearch = assignedUser.Login;
                        }

                    });
                }
                return _ChooseSuggestion;
            }
        }

        private RelayCommand _ConfirmAssignment;
        public RelayCommand ConfirmAssignment
        {
            get
            {
                if (_ConfirmAssignment == null)
                {
                    _ConfirmAssignment = new RelayCommand(() =>
                    {
                        var viewModel = AssignDialog.dataContext as IssueViewModel;
                        viewModel.AssignIssue.Execute(assignSearch);
                    });
                }
                return _ConfirmAssignment;
            }
        }

        private async void LoadUsers()
        {
            if (!string.IsNullOrWhiteSpace(assignSearch))
            {
                try
                {
                    var users = await constants.g_client.Search.SearchUsers(new SearchUsersRequest(assignSearch));
                    assignSuggestions = users.Items;
                }
                catch
                {
                }
            }
            else
            {
                assignSuggestions = null;
            }
        }
    }
}
