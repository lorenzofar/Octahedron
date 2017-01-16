using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class DescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value ?? constants.r_loader.GetString("noDescription");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
