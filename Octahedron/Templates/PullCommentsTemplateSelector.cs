using Octokit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Octahedron.Templates
{
    public class PullCommentsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate sentTemplate { get; set; }
        public DataTemplate receivedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);   
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            PullRequestReviewComment comments = (PullRequestReviewComment)item;
            if (comments.User.Login == App.user)
            {
                return sentTemplate;
            }
            else { return receivedTemplate; }
        }
    }
}
