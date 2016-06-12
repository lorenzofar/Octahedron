using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    public class PullDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                PullRequest pr = value as PullRequest;
                var user = pr.User.Login;
                if(pr.State == ItemState.Open)
                {
                    return String.Format(Helper.constants.r_loader.GetString("openDate"), Helper.constants.shortDateFormatter.Format(pr.CreatedAt.DateTime), user);
                }
                else
                {
                    if (pr.Merged)
                    {
                        return String.Format(Helper.constants.r_loader.GetString("mergeDate"), Helper.constants.shortDateFormatter.Format(pr.MergedAt.Value));
                    }
                    else
                    {
                        return String.Format(Helper.constants.r_loader.GetString("closedDate"), Helper.constants.shortDateFormatter.Format(pr.ClosedAt.Value), string.Empty);
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
