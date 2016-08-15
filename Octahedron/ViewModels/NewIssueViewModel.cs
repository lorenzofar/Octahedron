using GalaSoft.MvvmLight.Command;
using Helper;
using Octokit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml.Navigation;

namespace Octahedron.ViewModels
{
    public class NewIssueViewModel : ViewModelBase
    {
        private string[] _repoData;
        public string[] repoData
        {
            get
            {
                return _repoData;
            }
            set
            {
                Set(ref _repoData, value);
            }
        }

        private bool _loading;
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

        private string _title;
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
                AddIssue.RaiseCanExecuteChanged();
            }
        }

        private string _body;
        public string body
        {
            get
            {
                return _body;
            }
            set
            {
                Set(ref _body, value);
            }
        }

        private RelayCommand _AddIssue;
        public RelayCommand AddIssue
        {
            get
            {
                if(_AddIssue == null)
                {
                    _AddIssue = new RelayCommand(async() =>
                    {
                        loading = true;
                        NewIssue newIssue = new NewIssue(title) { Body = body };
                        var issue = await constants.g_client.Issue.Create(int.Parse(repoData[2]), newIssue);
                        string issueData = $"{repoData[0]}/{repoData[1]}/{issue.Number}";
                        App.Current.NavigationService.Navigate(typeof(Views.IssuePage), issueData);
                        loading = false;
                    }, () => !string.IsNullOrWhiteSpace(title));
                }
                return _AddIssue;
            }
        }

        private RelayCommand _Cancel;
        public RelayCommand Cancel
        {
            get
            {
                if(_Cancel == null)
                {
                    _Cancel = new RelayCommand(() =>
                    {
                        App.Current.NavigationService.GoBack();
                    });
                }
                return _Cancel;
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if(parameter != null)
            {
                repoData = parameter.ToString().Split('/');
            }
            return base.OnNavigatedToAsync(parameter, mode, state);
        }
    }
}
