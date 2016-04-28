﻿using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    class IssueDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                Issue issue = value as Issue;
                var user = issue.User.Login;
                var openDate = issue.CreatedAt;
                return String.Format(Helper.constants.r_loader.GetString(issue.State == ItemState.Open ? "openDate" : "closedDate"), Helper.constants.shortDateFormatter.Format(openDate), user);
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}