using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class ListSelectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool selecting = bool.Parse(value.ToString());
            return selecting == true ? ListViewSelectionMode.Multiple : ListViewSelectionMode.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
