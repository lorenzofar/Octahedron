using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class CommitInfoConverter : IValueConverter
    {
        private string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(parameter != null && parameter.ToString() == "pull")
            {
                var commit = value as PullRequestCommit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Commit.Committer.Name, AnalyzeDate(commit.Commit.Committer.Date.DateTime));
            }
            else if(parameter != null && parameter.ToString() == "repo")
            {
                var commit = value as GitHubCommit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Commit.Committer.Name, AnalyzeDate(commit.Commit.Committer.Date.DateTime));
            }
            else
            {
                var commit = value as Commit;
                return String.Format(constants.r_loader.GetString("CommitInfo"), commit.Committer.Name, AnalyzeDate(commit.Committer.Date.DateTime));
            }
        }

        private string AnalyzeDate(DateTime date)
        {
            var currentDate = DateTime.Now;
            if (currentDate.Year == date.Year)
            {
                var daysSpan = currentDate.Subtract(date).TotalDays;
                if (daysSpan < 1)
                {
                    var hours = currentDate.Subtract(date).Hours;
                    if(hours == 0)
                    {
                        return "just now";
                    }
                    else
                    {
                        return $"{hours} hours ago";
                    }
                }
                else if(daysSpan <= 10)
                {
                    var days = Math.Round(daysSpan);
                    return $"{days} days ago";
                }
                else
                {
                    return $"on {months[date.Month - 1]} {date.Day}";
                }
            }
            else
            {
                return $"on {months[date.Month - 1]} {date.Day}, {date.Year}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
