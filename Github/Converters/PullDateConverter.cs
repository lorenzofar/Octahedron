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
                var Date = pr.State == ItemState.Open ? pr.CreatedAt : pr.ClosedAt;
                return String.Format(Helper.constants.r_loader.GetString(pr.State == ItemState.Open ? "openDate" : "closedDate"), Helper.constants.shortDateFormatter.Format(Date.Value), pr.State == ItemState.Open ? user : string.Empty);
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
