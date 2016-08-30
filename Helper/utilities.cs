﻿using Octokit;
using System;
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
                var g_connection = new Connection(new ProductHeaderValue("Octahedron"))
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

        private static string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string FormatDate(DateTime date)
        {
            var currentDate = DateTime.Now.ToUniversalTime();
            if (currentDate.Year == date.Year)
            {
                var daysSpan = currentDate.Subtract(date).TotalDays;
                if (daysSpan < 1)
                {
                    var hours = currentDate.Subtract(date).Hours;
                    if (hours == 0)
                    {
                        return constants.r_loader.GetString("justNow");
                    }
                    else
                    {
                        return String.Format(constants.r_loader.GetString("timeAgo"), hours, constants.r_loader.GetString(hours == 1 ? "hour" : "hours"));
                    }
                }
                else if(daysSpan == 1)
                {
                    return constants.r_loader.GetString("yesterday");
                }
                else if (daysSpan < 30)
                {
                    var days = Math.Round(daysSpan);
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
    }
}
