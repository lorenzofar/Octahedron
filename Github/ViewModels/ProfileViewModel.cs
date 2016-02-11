﻿using System.Collections.Generic;
using Template10.Mvvm;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;
using Helper;
using Windows.ApplicationModel.DataTransfer;

namespace Github.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private bool owner_profile;
        private DataTransferManager dataTransferManager;

        private int _list_width;
        public int list_width
        {
            get
            {
                return _list_width;
            }
            set
            {
                Set(ref _list_width, value);
            }
        }

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

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (!string.IsNullOrEmpty(parameter.ToString()))
            {
                LoadData(parameter.ToString());
                this.dataTransferManager = DataTransferManager.GetForCurrentView();
                this.dataTransferManager.DataRequested += OnDataRequested;
            }
            return base.OnNavigatedToAsync(parameter, mode, state);
        }

        private async void LoadData(string username)
        {
            try
            {
                user = await constants.g_client.User.Get(username);
                repoList = await constants.g_client.Repository.GetAllForUser(username);
                orgsList = await constants.g_client.Organization.GetAll(username);
                following = await constants.g_client.User.Followers.IsFollowingForCurrent(user.Login);
                owner_profile = user.Login == (await constants.g_client.User.Current()).Login ? true : false;
                FollowUser.RaiseCanExecuteChanged();
            }
            catch
            {
                Helpers.Communications.ShowDialog("login_error", "error");
            }
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataPackage userData = e.Request.Data;
            userData.Properties.Title = $"{user.Name} - Github";
            userData.SetWebLink(new System.Uri(user.HtmlUrl, System.UriKind.RelativeOrAbsolute));
        }
    }
}
