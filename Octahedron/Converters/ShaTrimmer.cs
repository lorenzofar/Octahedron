using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class ShaTrimmer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string sha = value.ToString();
            return sha.Remove(7);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
