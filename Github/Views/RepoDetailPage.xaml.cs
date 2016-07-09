using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;

namespace Github.Views
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

        private void filter_issues_btn_Click(object sender, RoutedEventArgs e)
        {
            filter_issues_btn.Flyout.ShowAt(filter_issues_btn);
        }

        private void filter_pulls_btn_Click(object sender, RoutedEventArgs e)
        {
            filter_pulls_btn.Flyout.ShowAt(filter_pulls_btn);
        }

        private void filter_milestones_btn_Click(object sender, RoutedEventArgs e)
        {
            filter_milestones_btn.Flyout.ShowAt(filter_milestones_btn);
        }
    }
}
