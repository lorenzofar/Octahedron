using GalaSoft.MvvmLight.Messaging;
using Helper;
using Microsoft.HockeyApp;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Template10.Common;

namespace Octahedron
{
    sealed partial class App : BootStrapper
    {
        public static Octokit.User user { get; set; }
        public static Views.Shell shell { get; set; }

        public App()
        {
            this.InitializeComponent();
            new ViewModels.ViewModelLocator();
            HockeyClient.Current.Configure("d15374afd2ee4377851f1dc8e26e2d69");
            //HideStatusBar();
            SplashFactory = e => new Views.Splash(e);
        }

        private async void HideStatusBar()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }
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
                    Window.Current.Content = new Views.LoginPage();
                }
                else
                {
                    Window.Current.Content = shell;
                    var loginResult = await utilities.LogIn(credential.UserName, credential.Password);
                    if (loginResult == utilities.LoginResult.success)
                    {
                        user = await constants.g_client.User.Current();
                        Messenger.Default.Send(new MvvmMessaging.ProfileIconMessage { url = user.AvatarUrl });
                        NavigationService.Navigate(typeof(Views.MainPage));
                    }
                    else if (loginResult == utilities.LoginResult.wrongCredentials)
                    {
                        await communications.ShowDialog("credentials_error", "error");
                        await utilities.LogOut();
                        Window.Current.Content = new Views.LoginPage();
                    }
                    else
                    {
                        Window.Current.Content = new Views.NoInternetPage();
                    }
                }
            }
            return;
        }
    }
}
