using Octokit;
using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Github.Converters
{
    class IssueStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ItemState state = (ItemState)value;
            return state == ItemState.Open ? new SolidColorBrush(Color.FromArgb(255, 108, 198, 68)) : new SolidColorBrush(Color.FromArgb(255, 189, 44, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
