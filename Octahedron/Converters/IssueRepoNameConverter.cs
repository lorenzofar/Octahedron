using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class IssueRepoNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var issue = value as Issue;
            var url = issue.Url.AbsoluteUri;
            var splitUrl = url.Split('/');
            return splitUrl[5];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
