using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Github.Views
{
    public sealed partial class IssuePage : Page
    {
        public IssuePage()
        {
            this.InitializeComponent();
        }

        private void pickColorBtn_Click(object sender, RoutedEventArgs e)
        {
            pickColorBtn.Flyout.ShowAt(pickColorBtn);
        }

        private void removeLabelBtn_Click(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            Octokit.Label label = (Octokit.Label)element.DataContext;
            if (label != null)
            {
                var viewmodel = DataContext as ViewModels.IssueViewModel;
                viewmodel?.RemoveLabel.Execute(label.Name);
            }
        }

        private void labelGrid_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var viewmodel = DataContext as ViewModels.IssueViewModel;
            if (viewmodel.owner)
            {
                var senderElement = sender as FrameworkElement;
                var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
                flyoutBase.ShowAt(senderElement);
            }
        }

        private void removeCommentBtn_Click(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            Octokit.IssueComment comment = (Octokit.IssueComment)element.DataContext;
            if (comment != null)
            {
                var viewmodel = DataContext as ViewModels.IssueViewModel;
                viewmodel?.RemoveComment.Execute(comment.Id);
            }
        }

        private void assignBtn_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.AssignDialog().ShowAsync();
        }
    }
}
