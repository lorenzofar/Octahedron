﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool selecting = parameter == null ? bool.Parse(value.ToString()) : !bool.Parse(value.ToString());
            return selecting == true ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
