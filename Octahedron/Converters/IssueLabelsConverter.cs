﻿using Helper;
using Octokit;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Octahedron.Converters
{
    class IssueLabelsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Issue issue = value as Issue;
            var labels = issue.Labels;
            var container = new StackPanel() { Orientation = Orientation.Horizontal };
            foreach (var label in labels)
            {
                var box = new Grid() { Background = utilities.ConvertHexToBrush(label.Color), Margin = new Thickness(0, 0, 6, 0) };
                box.Children.Add(new TextBlock()
                {
                    Text = label.Name,
                    Style = (Style)App.Current.Resources["CaptionTextBlockStyle"],
                    Margin = new Thickness(6, 3, 6, 3),
                    Foreground = new SolidColorBrush(utilities.CheckColorType(utilities.ConvertHexToColor(label.Color)) == utilities.colorType.Dark ? Colors.White : Colors.Black)
                });
                container.Children.Add(box);
            }
            return container;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
