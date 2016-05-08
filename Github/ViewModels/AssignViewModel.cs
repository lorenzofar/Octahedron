using Helper;
using Octokit;
using System.Collections.Generic;
using Template10.Mvvm;

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
        }
    }
}
