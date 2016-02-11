using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    public class UpdateDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime updateDateTime = ((DateTimeOffset)value).DateTime;
            return String.Format(Helper.constants.r_loader.GetString("updateDate"), Helper.constants.shortDateFormatter.Format(updateDateTime), Helper.constants.shortTimeFormatter.Format(updateDateTime));

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
