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
                    glyph = "\uF026";
                    break;
                case "pullrequest":
                    glyph = "\uF009";
                    break;
                case "release":
                    glyph = "\uF015";
                    break;
                case "commit":
                    glyph = "\uF01F";
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
