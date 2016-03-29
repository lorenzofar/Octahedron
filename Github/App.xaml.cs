using Helper;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Github
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            this.InitializeComponent();
            SplashFactory = e => new Views.Splash(e);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            return Task.CompletedTask;
        }

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if ((Window.Current.Content as Views.Shell) == null)
            {
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                var credential = utilities.GetCredential("login");
                if (credential == null)
                {
                    Window.Current.Content = new Views.LoginPage(nav);
                }
                else
                {
                    Window.Current.Content = new Views.Shell(nav);
                    if (await utilities.LogIn(credential.UserName, credential.Password))
                    {
                        NavigationService.Navigate(typeof(Views.MainPage), null);
                    }
                    else
                    {
                        await Helper.communications.ShowDialog("login_error", "error");
                        App.Current.Exit();
                    }
                }
            }
            return;
        }
    }
}
