using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class DateSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime updateDateTime = parameter == null ? ((DateTimeOffset)value).DateTime : DateTime.Parse(value.ToString());
            return utilities.FormatDate(updateDateTime);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
