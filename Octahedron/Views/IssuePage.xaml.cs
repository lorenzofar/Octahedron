using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace Octahedron.Views
{
    public sealed partial class IssuePage : Page
    {
        public IssuePage()
        {
            this.InitializeComponent();
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
            new Dialogs.AssignDialog(this.DataContext).ShowAsync();
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            Octokit.IssueComment comment = (Octokit.IssueComment)element.DataContext;
            new Dialogs.EditIssueCommentDialog(this.DataContext, comment).ShowAsync();
        }
        
        private void milestoneBtn_Click(object sender, RoutedEventArgs e)
        {
            new Dialogs.AssignMilestoneDialog(this.DataContext).ShowAsync();
        }
    }
}
