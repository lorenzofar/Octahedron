using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Github.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public static Template10.Services.NavigationService.INavigationService nav_service;

        public LoginPage(Template10.Services.NavigationService.INavigationService nav)
        {
            this.InitializeComponent();
            nav_service = nav;
        }
    }
}