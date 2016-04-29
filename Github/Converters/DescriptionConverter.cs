﻿using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    public class DescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? constants.r_loader.GetString("noDescription") : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
