using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class ListPageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return String.Format(constants.r_loader.GetString("listPage"), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
