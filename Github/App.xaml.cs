﻿using Helper;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Github
{
    sealed partial class App : Template10.Common.BootStrapper
    {
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            var credential = utilities.GetCredential("login");
            if (credential == null)
            {
                NavigationService.Navigate(typeof(Views.LoginPage));
            }
            else
            {
                if (utilities.LogIn(credential.UserName, credential.Password))
                {
                    NavigationService.Navigate(typeof(Views.MainPage), null);
                }
                else
                {
                    Helpers.Communications.ShowDialog("login_error", "error");
                    App.Current.Exit();
                }
            }
            return Task.CompletedTask;
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if ((Window.Current.Content as Views.Shell) == null)
            {
                var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
                Window.Current.Content = new Views.Shell(nav);
            }
            return Task.CompletedTask;
        }
    }
}
