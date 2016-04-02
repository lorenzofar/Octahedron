using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class PivotBarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int pivotIndex = int.Parse(value.ToString());
            int barIndex = int.Parse(parameter.ToString());
            return pivotIndex == barIndex ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
