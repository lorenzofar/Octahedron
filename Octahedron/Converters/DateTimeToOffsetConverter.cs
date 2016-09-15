using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class DateTimeToOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTime date = (DateTime)value;
                return new DateTimeOffset(date);
            }
            catch
            {
                return new DateTimeOffset(DateTime.Now);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                DateTimeOffset dto = (DateTimeOffset)value;
                return dto.DateTime;
            }
            catch
            {
                return new DateTimeOffset(DateTime.Now);
            }
        }
    }
}
