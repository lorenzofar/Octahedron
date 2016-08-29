using Helper;
using Octahedron.Helpers;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class CommitInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter != null && parameter.ToString() == "pull")
            {
                var commit = value as PullRequestCommit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Commit.Committer.Name, DateFormatter.AnalyzeDate(commit.Commit.Committer.Date.DateTime));
            }
            else if (parameter != null && parameter.ToString() == "repo")
            {
                var commit = value as GitHubCommit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Commit.Committer.Name, DateFormatter.AnalyzeDate(commit.Commit.Committer.Date.DateTime));
            }
            else
            {
                var commit = value as Commit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Committer.Name, DateFormatter.AnalyzeDate(commit.Committer.Date.DateTime));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
