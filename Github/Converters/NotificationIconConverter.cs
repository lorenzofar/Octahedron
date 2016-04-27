using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class NotificationIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var n_type = value.ToString();
            string glyph = "";
            switch (n_type.ToLower())
            {
                case "issue":
                    glyph = "\uE783";
                    break;
                case "pullrequest":
                    glyph = "\uEC0A";
                    break;
                case "release":
                    glyph = "\uE8EC";
                    break;
                case "commit":
                    glyph = "\uE9A1";
                    break;
            }
            return glyph;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
