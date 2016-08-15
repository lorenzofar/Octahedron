using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class JoinDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime joinDateTime = ((DateTimeOffset)value).DateTime;
            return String.Format(Helper.constants.r_loader.GetString("joinDate"), Helper.constants.shortDateFormatter.Format(joinDateTime), Helper.constants.shortTimeFormatter.Format(joinDateTime));

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
