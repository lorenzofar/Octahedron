namespace Octahedron.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
        }

        public MainViewModel Main
        {
            get
            {
                return new MainViewModel();
            }
        }

        public LoginViewModel Login
        {
            get
            {
                return new LoginViewModel();
            }
        }

        public ProfileViewModel Profile
        {
            get
            {
                return new ProfileViewModel();
            }
        }

        public ReposViewModel Repos
        {
            get
            {
                return new ReposViewModel();
            }
        }

        public RepoDetailViewModel RepoDetail
        {
            get
            {
                return new RepoDetailViewModel();
            }
        }

        public SearchViewModel Search
        {
            get
            {
                return new SearchViewModel();
            }
        }

        public IssueViewModel Issue
        {
            get
            {
                return new IssueViewModel();
            }
        }

        public NewIssueViewModel NewIssue
        {
            get
            {
                return new NewIssueViewModel();
            }
        }

        public PullViewModel Pull
        {
            get
            {
                return new PullViewModel();
            }
        }

        public AssignViewModel Assign
        {
            get
            {
                return new AssignViewModel();
            }
        }

        public EditIssueCommentViewModel EditIssueComment
        {
            get
            {
                return new EditIssueCommentViewModel();
            }
        }

        public SettingsViewModel Settings
        {
            get
            {
                return new SettingsViewModel();
            }
        }
    }
}
