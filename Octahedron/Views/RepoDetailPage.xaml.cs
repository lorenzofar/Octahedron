using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Octahedron.Views
{
    public sealed partial class RepoDetailPage : Page
    {
        public RepoDetailPage()
        {
            this.InitializeComponent();
            Messenger.Default.Register<MvvmMessaging.ReadmeMessage>(this, message =>
            {
                readmeView.NavigateToString(message.html);
            });
        }
    }
}
