using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class AssigneeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                User assignee = (User)value;
                return String.Format(constants.r_loader.GetString("assignedTo"), assignee.Login);
            }
            else
            {
                return constants.r_loader.GetString("notAssigned");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
