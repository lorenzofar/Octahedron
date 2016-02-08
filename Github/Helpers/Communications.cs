using Helper;
using System;
using Windows.UI.Popups;

namespace Github.Helpers
{
    public class Communications
    {
        public static async void ShowDialog(string content_resource, string title_resource)
        {
            await new MessageDialog(constants.r_loader.GetString(content_resource), constants.r_loader.GetString(title_resource)).ShowAsync();
        }
    }
}
