﻿using Helper;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class UpdateDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime updateDateTime = parameter == null ? ((DateTimeOffset)value).DateTime : DateTime.Parse(value.ToString());
            return String.Format(constants.r_loader.GetString("updateDate"), utilities.FormatDate(updateDateTime));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
