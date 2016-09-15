using Helper;
using Octahedron.ViewModels;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class NewMIlestoneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {            
            return $"{constants.r_loader.GetString(NewMilestoneViewModel.editing ? "editMilestone" : "newMilestone")} - {value}".ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
