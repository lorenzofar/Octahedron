using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Github.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private bool owner_profile;
        private DataTransferManager dataTransferManager;

        private User _user;

        public User user
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

        private IReadOnlyList<Repository> _repoList;

        public IReadOnlyList<Repository> repoList
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

        private IReadOnlyList<Organization> _orgsList;

        public IReadOnlyList<Organization> orgsList
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

        private IReadOnlyList<User> _followingList;

        public IReadOnlyList<User> followingList
        {
            get
            {
                return _followingList;
            }
            set
            {
                Set(ref _followingList, value);
            }
        }

        private IReadOnlyList<User> _followersList;

        public IReadOnlyList<User> followersList
        {
            get
            {
                return _followersList;
            }
            set
            {
                Set(ref _followersList, value);
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
                if (_OpenBlog == null)
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

        private RelayCommand<object> _OpenRepo;

        public RelayCommand<object> OpenRepo
        {
            get
            {
                if (_OpenRepo == null)
                {
                    _OpenRepo = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        if (args != null && args.ClickedItem != null)
                        {
                            var repo = args.ClickedItem as Repository;
                            App.Current.NavigationService.Navigate(typeof(Views.RepoDetailPage), repo.FullName);
                        }
                    });
                }
                return _OpenRepo;
            }
        }

        private RelayCommand<object> _OpenUser;

        public RelayCommand<object> OpenUser
        {
            get
            {
                if (_OpenUser == null)
                {
                    _OpenUser = new RelayCommand<object>((e) =>
                    {
                        var args = e as ItemClickEventArgs;
                        if (args != null && args.ClickedItem != null)
                        {
                            var user = args.ClickedItem as User;
                            App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), user.Login);
                        }
                    });
                }
                return _OpenUser;
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
                followersList = await constants.g_client.User.Followers.GetAll(user.Login);
                followingList = await constants.g_client.User.Followers.GetAllFollowing(user.Login);
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