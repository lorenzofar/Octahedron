using Octokit;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Octahedron.Templates
{
    public class UsersSearchResultsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate userTemplate { get; set; }
        public DataTemplate organizationTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            User user = (User)item;
            if(user.Type == AccountType.User)
            {
                return userTemplate;
            }
            else
            {
                return organizationTemplate;
            }
        }
    }
}
