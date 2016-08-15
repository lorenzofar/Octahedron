using GalaSoft.MvvmLight.Command;
using Helper;
using Octahedron.Views.Dialogs;
using Octokit;
using System.Collections.Generic;
using Template10.Mvvm;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Octahedron.ViewModels
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
                suggestionsOpen = !string.IsNullOrWhiteSpace(_assignSearch);                
            }
        }

        private bool _suggestionsOpen = true;
        public bool suggestionsOpen
        {
            get
            {
                return _suggestionsOpen;
            }
            set
            {
                Set(ref _suggestionsOpen, value);
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

        private RelayCommand _SubmitQuery;
        public RelayCommand SubmitQuery
        {
            get
            {
                if (_SubmitQuery == null)
                {
                    _SubmitQuery = new RelayCommand(() =>
                    {
                        LoadUsers();
                    });
                }
                return _SubmitQuery;
            }
        }

        private RelayCommand<object> _KeyDown;
        public RelayCommand<object> KeyDown
        {
            get
            {
                if (_KeyDown == null)
                {
                    _KeyDown = new RelayCommand<object>((e) =>
                    {
                        var args = e as KeyRoutedEventArgs;
                        if (args.Key == Windows.System.VirtualKey.Enter)
                        {
                            LoadUsers();
                        }
                    });
                }
                return _KeyDown;
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

        private RelayCommand _HideSuggestions;
        public RelayCommand HideSuggestions
        {
            get
            {
                if (_HideSuggestions == null)
                {
                    _HideSuggestions = new RelayCommand(() =>
                    {
                        suggestionsOpen = false;
                    });
                }
                return _HideSuggestions;
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
