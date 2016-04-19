using Octokit;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Helper
{
    public class utilities
    {
        /// <summary>
        /// Saves user's credentials in the PasswordVault
        /// </summary>
        /// <param name="resource">The resource name of the credential.</param>
        /// <param name="username">The user's username.</param>
        /// <param name="password">The user's password.</param>
        public static void SaveCredentials(string resource, string username, string password)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(resource, username, password));
        }

        /// <summary>
        /// Retrieves credentials for a particular resource
        /// </summary>
        /// <param name="resource">The resource name of the credential</param>
        public static PasswordCredential GetCredential(string resource)
        {
            var vault = new PasswordVault();
            try
            {
                var credentials = vault.FindAllByResource(resource);
                if (credentials.Count > 0)
                {
                    return vault.Retrieve(resource, credentials[0].UserName);
                }
                else
                {
                    //THERE ARE NO CREDENTIALS, SO WE RETURN A NULL VALUE
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public static async Task<bool> LogIn(string username, string password)
        {
            try
            {
                var g_credentials = new Credentials(username, password);
                var g_connection = new Connection(new ProductHeaderValue("GithubUWP"))
                {
                    Credentials = g_credentials
                };
                constants.g_client = new GitHubClient(g_connection);
                var user = await constants.g_client.User.Current();
                return true;
            }
            catch(AuthorizationException)
            {
                await communications.ShowDialog("credentials_error", "error");
                return false;
            }
            catch
            {
                return false;
            }
        }        

        public static SolidColorBrush ConvertHexToBrush(string hexColor)
        {
            byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        } 
    }
}
