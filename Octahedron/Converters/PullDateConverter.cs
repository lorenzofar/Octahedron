using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class PullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                PullRequest pr = value as PullRequest;
                if (pr.State == ItemState.Open)
                {
                    return String.Format(constants.r_loader.GetString("openDate"), pr.User.Login, utilities.FormatDate(pr.CreatedAt.DateTime));
                }
                else
                {
                    if (pr.Merged)
                    {
                        return String.Format(constants.r_loader.GetString("mergeDate"), pr.MergedBy.Login, utilities.FormatDate(pr.MergedAt.Value.DateTime));
                    }
                    else
                    {
                        return String.Format(constants.r_loader.GetString("closedDate"), string.Empty, utilities.FormatDate(pr.ClosedAt.Value.DateTime));
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
