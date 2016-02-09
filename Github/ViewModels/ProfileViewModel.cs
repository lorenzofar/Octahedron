using System.Collections.Generic;
using Template10.Mvvm;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private Octokit.User _user;
        public Octokit.User user
        {
            get
            {
                return _user;
            }
            set
            {
                Set(ref _user, value);
            }
        }

        private IReadOnlyList<Octokit.Repository> _reposList;
        public IReadOnlyList<Octokit.Repository> repoList
        {
            get
            {
                return _reposList;
            }
            set
            {
                Set(ref _reposList, value);
            }
        }

        private IReadOnlyList<Octokit.Organization> _orgsList;
        public IReadOnlyList<Octokit.Organization> orgsList
        {
            get
            {
                return _orgsList;
            }
            set
            {
                Set(ref _orgsList, value);
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (!string.IsNullOrEmpty(parameter.ToString()))
            {
                LoadData(parameter.ToString());
            }
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadData(string username)
        {
            try
            {
                user = await Helper.constants.g_client.User.Get(username);
                repoList = await Helper.constants.g_client.Repository.GetAllForUser(username);
                orgsList = await Helper.constants.g_client.Organization.GetAll(username);
            }
            catch
            {
                Helpers.Communications.ShowDialog("login_error", "error");
            }
        }

        
    }
}
