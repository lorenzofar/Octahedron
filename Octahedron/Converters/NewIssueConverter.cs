using Helper;
using Octahedron.ViewModels;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class NewIssueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {            
            return $"{constants.r_loader.GetString(NewIssueViewModel.editing ? "editIssue" : "newIssue")} - {value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
