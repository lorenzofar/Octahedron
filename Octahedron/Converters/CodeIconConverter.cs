using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class CodeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var content = value as RepositoryContent;
            switch (content.Type)
            {
                case ContentType.File:
                    return "\uF011";
                case ContentType.Dir:
                    return "\uF016";
                default:
                    return "\uF010";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
