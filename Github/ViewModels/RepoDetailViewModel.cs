using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Helper;
using Windows.UI.Xaml.Navigation;
using Octokit;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;

namespace Github.ViewModels
{
    public class RepoDetailViewModel : ViewModelBase
    {
        private bool owner;

        private Repository _repo;
        public Repository repo
        {
            get
            {
                return _repo;
            }
            set
            {
                Set(ref _repo, value);
            }
        }

        private bool _starred;
        public bool starred
        {
            get
            {
                return _starred;
            }
            set
            {
                Set(ref _starred, value);
            }
        }

        private bool _watched;
        public bool watched
        {
            get
            {
                return _watched;
            }
            set
            {
                Set(ref _watched, value);
            }
        }

        private IReadOnlyList<Octokit.Issue> _issues;
        public IReadOnlyList<Octokit.Issue> issues
        {
            get
            {
                return _issues;
            }
            set
            {
                Set(ref _issues, value);
            }
        }

        private IReadOnlyList<RepositoryContributor> _contributorsList;
        public IReadOnlyList<RepositoryContributor> contributorsList
        {
            get
            {
                return _contributorsList;
            }
            set
            {
                Set(ref _contributorsList, value);
            }
        }

        #region COMMANDS
        private RelayCommand _WatchRepo;
        public RelayCommand WatchRepo
        {
            get
            {
                if (_WatchRepo == null)
                {
                    _WatchRepo = new RelayCommand(() =>
                    {
                        if (!watched)
                        {
                            constants.g_client.Activity.Watching.UnwatchRepo(repo.Owner.Login, repo.Name);
                        }
                        else
                        {
                            constants.g_client.Activity.Watching.WatchRepo(repo.Owner.Login, repo.Name, new NewSubscription { Subscribed = true });
                        }
                    });
                }
                return _WatchRepo;
            }
        }

        private RelayCommand _StarRepo;
        public RelayCommand StarRepo
        {
            get
            {
                if (_StarRepo == null)
                {
                    _StarRepo = new RelayCommand(async () =>
                    {
                        if (!starred)
                        {
                            await constants.g_client.Activity.Starring.RemoveStarFromRepo(repo.Owner.Login, repo.Name);
                        }
                        else
                        {
                            await constants.g_client.Activity.Starring.StarRepo(repo.Owner.Login, repo.Name);
                        }
                        LoadRepo(repo.FullName);
                    });
                }
                return _StarRepo;
            }
        }

        private RelayCommand _OpenProfile;
        public RelayCommand OpenProfile
        {
            get
            {
                if (_OpenProfile == null)
                {
                    _OpenProfile = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), repo.Owner.Login);
                    });
                }
                return _OpenProfile;
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
                            var user = args.ClickedItem as RepositoryContributor;
                            App.Current.NavigationService.Navigate(typeof(Views.ProfilePage), user.Login);
                        }
                    });
                }
                return _OpenUser;
            }
        }
        #endregion

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                LoadRepo(parameter);
            }
            return Task.CompletedTask;
        }

        private async void LoadRepo(object info)
        {
            string[] repoInfo = info.ToString().Split('/');
            repo = await constants.g_client.Repository.Get(repoInfo[0], repoInfo[1]);
            owner = repo.Owner.Login == (await constants.g_client.User.Current()).Login ? true : false;
            watched = await constants.g_client.Activity.Watching.CheckWatched(repo.Owner.Login, repo.Name);
            starred = await constants.g_client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);
            issues = await constants.g_client.Issue.GetAllForRepository(repo.Owner.Login, repo.Name);
            contributorsList = await constants.g_client.Repository.GetAllContributors(repo.Owner.Login, repo.Name);
        }
    }
}
