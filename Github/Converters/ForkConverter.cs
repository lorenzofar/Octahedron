using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class ForkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                Repository parent = value as Repository;
                return $"{constants.r_loader.GetString("forkedFrom")} {parent.FullName}";
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
