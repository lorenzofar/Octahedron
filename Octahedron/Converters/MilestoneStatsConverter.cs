using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class MilestoneStatsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var milestone = (Milestone)value;
            int open = milestone.OpenIssues;
            int closed = milestone.ClosedIssues;
            int total = open + closed;
            int percentage = total != 0 ? (closed * 100) / total : 0;
            return String.Format(constants.r_loader.GetString("milestoneStats"), percentage, open, closed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
