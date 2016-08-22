using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class CommitInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(parameter != null && parameter.ToString() == "pull")
            {
                var commit = value as PullRequestCommit;
                return $"{commit.Commit.Committer.Name} - {constants.shortDateFormatter.Format(commit.Commit.Committer.Date)}";
            }
            else
            {
                var commit = value as Commit;
                return $"{commit.Committer.Name} - {constants.shortDateFormatter.Format(commit.Committer.Date)}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
