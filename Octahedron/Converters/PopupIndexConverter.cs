using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Octahedron.Converters
{
    class PopupIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = int.Parse(value.ToString());
            int par = int.Parse(parameter.ToString());
            SolidColorBrush brush = par == index ? (SolidColorBrush)App.Current.Resources["SystemControlBackgroundAccentBrush"] : new SolidColorBrush(Colors.Transparent);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
