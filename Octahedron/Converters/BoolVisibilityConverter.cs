using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool visible = parameter == null ? bool.Parse(value.ToString()) : !bool.Parse(value.ToString());
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
