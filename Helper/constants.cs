using Octokit;
using Windows.ApplicationModel.Resources;
using Windows.Globalization.DateTimeFormatting;

namespace Helper
{
    public class constants
    {
        public static ResourceLoader r_loader = new ResourceLoader();
        public static DateTimeFormatter shortDateFormatter = new DateTimeFormatter("shortdate");
        public static DateTimeFormatter longDateFormatter = new DateTimeFormatter("longdate");
        public static DateTimeFormatter shortTimeFormatter = new DateTimeFormatter("shorttime");
        public static DateTimeFormatter longTimeFormatter = new DateTimeFormatter("longtime");
        public static GitHubClient g_client { get; set; }
    }
}