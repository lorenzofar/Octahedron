using Windows.Security.Credentials;

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
            var credentials = vault.FindAllByResource(resource);
            if(credentials.Count > 0)
            {
                return credentials[0];
            }
            else
            {
                //THERE ARE NO CREDENTIALS, SO WE RETURN A NULL VALUE
                return null;
            }
        }

    }
}
