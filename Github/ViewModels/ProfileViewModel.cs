using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Helper;
using Windows.ApplicationModel.DataTransfer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Github.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private bool owner_profile;
        private DataTransferManager dataTransferManager;

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

        private IReadOnlyList<Octokit.Repository> _repoList;
        public IReadOnlyList<Octokit.Repository> repoList
        {
            get
            {
                return _repoList;
            }
            set
            {
                Set(ref _repoList, value);
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

        private int _starredRepos;
        public int starredRepos
        {
            get
            {
                return _starredRepos;
            }
            set
            {
                Set(ref _starredRepos, value);
            }
        }

        private bool _following;
        public bool following
        {
            get
            {
                return _following;
            }
            set
            {
                Set(ref _following, value);
            }
        }

        private RelayCommand _ShareUser;
        public RelayCommand ShareUser
        {
            get
            {
                if (_ShareUser == null)
                {
                    _ShareUser = new RelayCommand(() =>
                    {
                        DataTransferManager.ShowShareUI();
                    });
                }
                return _ShareUser;
            }
        }

        private RelayCommand _FollowUser;
        public RelayCommand FollowUser
        {

            get
            {
                if (_FollowUser == null)
                {
                    _FollowUser = new RelayCommand(async () =>
                    {
                        var follower = await constants.g_client.User.Followers.IsFollowingForCurrent(user.Login);
                        if (follower)
                        {
                            await constants.g_client.User.Followers.Unfollow(user.Login);
                        }
                        else
                        {
                            await constants.g_client.User.Followers.Follow(user.Login);
                        }
                    }, () => !owner_profile);
                }
                return _FollowUser;
            }
        }

        private RelayCommand _OpenBlog;
        public RelayCommand OpenBlog
        {
            get
            {
                if(_OpenBlog == null)
                {
                    _OpenBlog = new RelayCommand(async () =>
                    {
                       await Windows.System.Launcher.LaunchUriAsync(new Uri(user.Blog, UriKind.RelativeOrAbsolute));
                    });
                }
                return _OpenBlog;
            }
        }

        private RelayCommand _SendMail;
        public RelayCommand SendMail
        {
            get
            {
                if (_SendMail == null)
                {
                    _SendMail = new RelayCommand(async () =>
                    {
                        await Windows.System.Launcher.LaunchUriAsync(new Uri(String.Format("mailto:{0}", user.Email), UriKind.RelativeOrAbsolute));
                    });
                }
                return _SendMail;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            LoadData(parameter);
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += OnDataRequested;
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadData(object username)
        {
            try
            {
                user = username == null ? await constants.g_client.User.Current() : await constants.g_client.User.Get(username.ToString());
                owner_profile = user.Login == (await constants.g_client.User.Current()).Login ? true : false;
                FollowUser.RaiseCanExecuteChanged();
                following = await constants.g_client.User.Followers.IsFollowingForCurrent(user.Login);
                var repos = await constants.g_client.Repository.GetAllForUser(user.Login);
                repoList = repos.OrderByDescending(x => x.UpdatedAt).ToList();
                orgsList = await constants.g_client.Organization.GetAll(user.Login);
                starredRepos = (await constants.g_client.Activity.Starring.GetAllForUser(user.Login)).Count;
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
            }
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataPackage userData = e.Request.Data;
            userData.Properties.Title = $"{user.Name} - Github";
            userData.SetWebLink(new Uri(user.HtmlUrl, UriKind.RelativeOrAbsolute));
        }
    }
}
