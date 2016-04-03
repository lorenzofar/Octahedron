using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool state = bool.Parse(value.ToString());
            return parameter == null ? state : !state;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
