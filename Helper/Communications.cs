using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Helper
{
    public class communications
    {
        public static async Task ShowDialog(string content_resource, string title_resource)
        {
            await new MessageDialog(constants.r_loader.GetString(content_resource), constants.r_loader.GetString(title_resource)).ShowAsync();
        }
    }
}
