using System;
using Windows.UI.Xaml.Data;
using Octokit;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Octahedron.Converters
{
    class PullStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                PullRequest pullReq = value as PullRequest;
                if (pullReq.State == ItemState.Open)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 108, 198, 68));
                }
                else
                {
                    if (pullReq.Merged)
                    {
                        return new SolidColorBrush(Color.FromArgb(255, 110, 84, 148));
                    }
                    else
                    {
                        return new SolidColorBrush(Color.FromArgb(255, 189, 44, 0));
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
