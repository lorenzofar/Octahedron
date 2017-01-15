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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private bool owner_profile;
        private DataTransferManager dataTransferManager;

        private bool _loading = false;
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

        private SolidColorBrush _background;
        public SolidColorBrush background
        {
            get
            {
                return _background;
            }
            set
            {
                Set(ref _background, value);
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

        private IReadOnlyList<Organization> _organizations;
        public IReadOnlyList<Organization> organizations
        {
            get
            {
                return _organizations;
            }
            set
            {
                Set(ref _organizations, value);
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

        #region ORGANIZATION
        private bool _member = false;
        public bool member
        {
            get
            {
                return _member;
            }
            set
            {
                Set(ref _member, value);
            }
        }

        private IReadOnlyList<Team> _teams;
        public IReadOnlyList<Team> teams
        {
            get
            {
                return _teams;
            }
            set
            {
                Set(ref _teams, value);
            }
        }

        private IReadOnlyList<User> _members;
        public IReadOnlyList<User> members
        {
            get
            {
                return _members;
            }
            set
            {
                Set(ref _members, value);
            }
        }
        #endregion

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
                        if (e is ItemClickEventArgs args && args.ClickedItem != null)
                        {
                            var repo = args.ClickedItem as Repository;
                            var repoData = new Dictionary<int, string>
                            {
                                { 0, repo.Owner.Login },
                                { 1, repo.Name }
                            };
                            App.Current.NavigationService.Navigate(typeof(Views.RepoDetailPage), repoData);
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
                        if (e is ItemClickEventArgs args && args.ClickedItem != null)
                        {
                            var user = args.ClickedItem as User;
                            App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), user.Login);
                        }
                    });
                }
                return _OpenUser;
            }
        }

        private RelayCommand<object> _OpenOrganization;
        public RelayCommand<object> OpenOrganization
        {
            get
            {
                if (_OpenOrganization == null)
                {
                    _OpenOrganization = new RelayCommand<object>((e) =>
                    {
                        if (e is ItemClickEventArgs args && args.ClickedItem != null)
                        {
                            var org = args.ClickedItem as Organization;
                            App.Current.NavigationService.Navigate(typeof(Views.OrganizationPage), org.Login);
                        }
                    });
                }
                return _OpenOrganization;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (mode != NavigationMode.Back || parameter == null || parameter.ToString() != user.Login)
            {
                LoadData(parameter);
            }
            this.dataTransferManager = DataTransferManager.GetForCurrentView();
            this.dataTransferManager.DataRequested += OnDataRequested;
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadData(object username)
        {
            try
            {
                loading = true;
                loadingProgress = constants.r_loader.GetString("info_progress");
                user = username == null ? await constants.g_client.User.Current() : await constants.g_client.User.Get(username.ToString());
                loadingProgress = constants.r_loader.GetString("repositories_progress");
                var repos = await constants.g_client.Repository.GetAllForUser(user.Login);
                repoList = repos.OrderByDescending(x => x.UpdatedAt).ToList();
                if (user.Type == AccountType.Organization)
                {
                    member = await constants.g_client.Organization.Member.CheckMember(username.ToString(), App.user.Login);
                    loadingProgress = constants.r_loader.GetString("members_progress");
                    members = await constants.g_client.Organization.Member.GetAll(username.ToString());
                    if (member)
                    {
                        loadingProgress = constants.r_loader.GetString("teams_progress");
                        teams = await constants.g_client.Organization.Team.GetAll(username.ToString());
                    }
                }
                else
                {
                    owner_profile = user.Login == (await constants.g_client.User.Current()).Login ? true : false;
                    FollowUser.RaiseCanExecuteChanged();
                    following = await constants.g_client.User.Followers.IsFollowingForCurrent(user.Login);
                    loadingProgress = constants.r_loader.GetString("organizations_progress");
                    organizations = username == null ? await constants.g_client.Organization.GetAllForCurrent() : await constants.g_client.Organization.GetAll(username.ToString());
                    loadingProgress = constants.r_loader.GetString("followers_progress");
                    followersList = await constants.g_client.User.Followers.GetAll(user.Login);
                    loadingProgress = constants.r_loader.GetString("following_progress");
                    followingList = await constants.g_client.User.Followers.GetAllFollowing(user.Login);
                }
                background = new SolidColorBrush(await utilities.GetDominantColor(user.AvatarUrl));
                loading = false;
                starredRepos = (await constants.g_client.Activity.Starring.GetAllForUser(user.Login)).Count;
            }
            catch
            {
                await communications.ShowDialog("login_error", "error");
                loading = false;
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
