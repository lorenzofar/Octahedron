using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    public class MilestoneStatsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var milestone = (Milestone)value;
            int open = milestone.OpenIssues;
            int closed = milestone.ClosedIssues;
            int total = open + closed;
            int percentage = (closed * 100) / total;
            return String.Format(constants.r_loader.GetString("milestoneStats"), percentage, open, closed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
