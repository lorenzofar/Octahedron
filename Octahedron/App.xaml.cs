using GalaSoft.MvvmLight.Messaging;
using Helper;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Octahedron
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public static Octokit.User user { get; set; }
        public static Views.Shell shell { get; set; }

        public App()
        {
            this.InitializeComponent();
            //SplashFactory = e => new Views.Splash(e);
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
                shell = new Views.Shell(nav);
                var credential = utilities.GetCredential("login");
                if (credential == null)
                {
                    Window.Current.Content = new Views.LoginPage(nav);
                }
                else
                {
                    Window.Current.Content = shell;
                    if (await utilities.LogIn(credential.UserName, credential.Password))
                    {
                        user = await constants.g_client.User.Current();
                        Messenger.Default.Send<MvvmMessaging.ProfileIconMessage>(new MvvmMessaging.ProfileIconMessage { url = user.AvatarUrl });
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
