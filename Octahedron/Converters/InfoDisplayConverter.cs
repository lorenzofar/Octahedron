﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class InfoDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null || value.ToString() == "" ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
