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

        Octokit.WatchedClient watch;

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
            {
                string[] repoInfo = parameter.ToString().Split('/');
                repo = await constants.g_client.Repository.Get(repoInfo[0], repoInfo[1]);
                owner = repo.Owner.Login == (await constants.g_client.User.Current()).Login ? true : false;
                watched = await constants.g_client.Activity.Watching.CheckWatched(repo.Owner.Login, repo.Name);
                starred = await constants.g_client.Activity.Starring.CheckStarred(repo.Owner.Login, repo.Name);
                issues = await constants.g_client.Issue.GetAllForRepository(repo.Owner.Login, repo.Name);
            }
            return;
        }

        public void io()
        {
            
        }
    }
}
