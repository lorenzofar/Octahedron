using Octokit;
using Windows.ApplicationModel.Resources;

namespace Helper
{
    public class constants
    {
        public static ResourceLoader r_loader = new ResourceLoader();
        public static GitHubClient g_client { get; set; }
    }
}
