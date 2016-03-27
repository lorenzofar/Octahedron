using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class ListPlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if(parameter.ToString() == "main")
                {
                    var cvs = value as List<Octokit.Notification>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    var cvs = value as List<Octokit.Repository>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
