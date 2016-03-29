using Helper;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Github.Converters
{
    public class UserNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var task = Task.Run(async () =>
            {
                var user = await constants.g_client.User.Get(value.ToString());
                return user != null ? user.Name : "";
            });
            return task.Result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}