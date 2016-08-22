using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class RepoIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var repo = value as Repository;
            return repo.Private ? "\uF06A" : "\uF001";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
