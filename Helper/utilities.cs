using Octokit;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Networking.BackgroundTransfer;
using Windows.Security.Credentials;
using Windows.Storage;
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

        public enum LoginResult
        {
            success,
            wrongCredentials,
            connectionError
        }

        public static async Task<LoginResult> LogIn(string username, string password)
        {
            try
            {
                var g_credentials = new Credentials(username, password);
                var g_connection = new Connection(new ProductHeaderValue("Octahedron"))
                {
                    Credentials = g_credentials
                };
                constants.g_client = new GitHubClient(g_connection);
                var user = await constants.g_client.User.Current();
                return LoginResult.success;
            }
            catch (AuthorizationException)
            {
                return LoginResult.wrongCredentials;
            }
            catch
            {
                return LoginResult.connectionError;
            }
        }

        public static async Task LogOut()
        {
            var vault = new PasswordVault();
            vault.Remove(GetCredential("login"));
        }

        public static SolidColorBrush ConvertHexToBrush(string hexColor)
        {
            byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }

        public static Color ConvertHexToColor(string hexColor)
        {
            byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            return Color.FromArgb(255, r, g, b);
        }

        public static string ConvertColorToHex(Color color)
        {
            string r = color.R.ToString("X");
            string g = color.G.ToString("X");
            string b = color.B.ToString("X");
            r = r.Length == 1 ? $"0{r}" : r;
            g = g.Length == 1 ? $"0{g}" : g;
            b = b.Length == 1 ? $"0{b}" : b;
            return $"{r}{g}{b}";
        }

        public enum colorType
        {
            Dark,
            Light
        }

        public static colorType CheckColorType(Color color)
        {
            if (color.R * 0.2126 + color.G * 0.7152 + color.B * 0.0722 > 255 / 2)
            {
                return colorType.Light;
            }
            else
            {
                return colorType.Dark;
            }
        }

        private static string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string FormatDate(DateTime date)
        {
            var currentDate = DateTime.Now.ToUniversalTime();
            if (currentDate.Year == date.Year)
            {
                var days = currentDate.Subtract(date).Days;
                if (days < 1)
                {
                    var hours = Math.Round(currentDate.Subtract(date).TotalHours);
                    if (hours == 0)
                    {
                        var minutes = Math.Round(currentDate.Subtract(date).TotalMinutes);
                        if (minutes == 0)
                        {
                            return constants.r_loader.GetString("justNow");
                        }
                        else
                        {
                            return String.Format(constants.r_loader.GetString("timeAgo"), minutes, constants.r_loader.GetString(minutes == 1 ? "minute" : "minutes"));
                        }
                    }
                    else
                    {
                        return String.Format(constants.r_loader.GetString("timeAgo"), hours, constants.r_loader.GetString(hours == 1 ? "hour" : "hours"));
                    }
                }
                else if (days == 1)
                {
                    return constants.r_loader.GetString("yesterday");
                }
                else if (days < 30)
                {
                    return String.Format(constants.r_loader.GetString("timeAgo"), days, constants.r_loader.GetString(days == 1 ? "day" : "days"));
                }
                else
                {
                    return String.Format(constants.r_loader.GetString("onDate"), $"{months[date.Month - 1]} {date.Day}");
                }
            }
            else
            {
                return String.Format(constants.r_loader.GetString("onDate"), $"{months[date.Month - 1]} {date.Day}, {date.Year}");
            }
        }

        /// <summary>
        /// Download a file from the provided url
        /// </summary>
        /// <param name="url">The download url</param>
        /// <param name="file">The file to save the download in</param>
        /// <returns></returns>
        public static async Task<bool> DownloadFile(Uri url, StorageFile file)
        {
            try
            {
                var downloader = new BackgroundDownloader();
                var download_operation = downloader.CreateDownload(url, file);
                var download_result = await download_operation.StartAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<Color> GetDominantColor(string imagePath)
        {
            StorageFile image_file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("profilePic", CreationCollisionOption.GenerateUniqueName);
            var download = await DownloadFile(new Uri(imagePath), image_file);
            if (download)
            {
                var colorThief = new ColorThiefDotNet.ColorThief();
                var stream = await image_file.OpenReadAsync();
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                var color = await colorThief.GetColor(decoder);
                var brushColor = Color.FromArgb(color.Color.A, color.Color.R, color.Color.G, color.Color.B);
                await image_file.DeleteAsync();
                return brushColor;
            }
            else
            {
                return Colors.Transparent;
            }
        }
    }
}
