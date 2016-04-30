using Octokit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Github.Templates
{
    public class IssueCommentsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate sentTemplate { get; set; }
        public DataTemplate receivedTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);   
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            IssueComment comments = (IssueComment)item;
            if (comments.User.Login == App.user)
            {
                return sentTemplate;
            }
            else { return receivedTemplate; }
        }
    }
}
