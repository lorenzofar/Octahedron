using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

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

        private List<Octokit.Repository> _reposList;
        public List<Octokit.Repository> repoList
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

        private List<Octokit.Organization> _orgsList;
        public List<Octokit.Organization> orgsList
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

        private RelayCommand _GetData;
        public RelayCommand GetData {
            get
            {
                if (_GetData == null)
                {
                    _GetData = new RelayCommand(() =>
                    {
                        LoadData();
                    });
                }
                return _GetData;
            }
        }

        private async void LoadData()
        {
            user = await Helper.constants.g_client.User.Current();
        }
    }
}
