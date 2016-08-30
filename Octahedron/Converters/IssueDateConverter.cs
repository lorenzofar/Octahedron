using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class IssueDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                Issue issue = value as Issue;
                var user = issue.User.Login;
                var Date = issue.State == ItemState.Open ? issue.CreatedAt : issue.ClosedAt;
                return String.Format(constants.r_loader.GetString(issue.State == ItemState.Open ? "openDate" : "closedDate"), issue.State == ItemState.Open ? user : string.Empty, utilities.FormatDate(Date.Value.DateTime));
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
