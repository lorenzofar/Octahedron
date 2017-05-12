namespace Octahedron.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            Main = new MainViewModel();
            Login = new LoginViewModel();
            Profile = new ProfileViewModel();
            Repos = new ReposViewModel();
            RepoDetail = new RepoDetailViewModel();
            Search = new SearchViewModel();
            Issue = new IssueViewModel();
            Pull = new PullViewModel();
            Assign = new AssignViewModel();
            Settings = new SettingsViewModel();
            NoInternet = new NoInternetViewModel();
        }

        public MainViewModel Main { get; set; }

        public LoginViewModel Login { get; set; }

        public ProfileViewModel Profile { get; set; }

        public ReposViewModel Repos { get; set; }

        public RepoDetailViewModel RepoDetail { get; set; }

        public SearchViewModel Search { get; set; }

        public IssueViewModel Issue { get; set; }

        public NewIssueViewModel NewIssue => new NewIssueViewModel();

        public PullViewModel Pull { get; set; }

        public AssignViewModel Assign { get; set; }

        public EditIssueCommentViewModel EditIssueComment => new EditIssueCommentViewModel();

        public SettingsViewModel Settings { get; set; }

        public NoInternetViewModel NoInternet { get; set; }

        public NewMilestoneViewModel NewMilestone => new NewMilestoneViewModel();

        public AssignMilestoneViewModel AddMilestone => new AssignMilestoneViewModel();

        public ActivityViewModel Activity => new ActivityViewModel();
    }
}
