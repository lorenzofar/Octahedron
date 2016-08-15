using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime date = DateTime.Parse(value.ToString());
            return constants.shortDateFormatter.Format(date);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
