using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class ChangesNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var changes = int.Parse(value.ToString());
            if(parameter.ToString() == "add")
            {
                return $"+{changes}";
            }
            else
            {
                return $"-{changes}";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
