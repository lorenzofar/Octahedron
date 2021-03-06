﻿using Helper;
using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class DueDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                var milestone = (Milestone)value;
                switch (milestone.State)
                {
                    default:
                    case ItemState.Open:
                        if (milestone.DueOn == null)
                        {
                            return constants.r_loader.GetString("noDueDate");
                        }
                        else
                        {
                            return String.Format(constants.r_loader.GetString("dueDate"), constants.shortDateFormatter.Format(milestone.DueOn.Value));
                        }
                    case ItemState.Closed:
                        return String.Format(constants.r_loader.GetString("closedDate"), constants.shortDateFormatter.Format(milestone.ClosedAt.Value), string.Empty);
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
