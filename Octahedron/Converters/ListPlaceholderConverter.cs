using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class ListPlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                if(parameter.ToString() == "main")
                {
                    var cvs = value as List<Octokit.Notification>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "issues")
                {
                    var cvs = value as IReadOnlyList<Octokit.Issue>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "pulls")
                {
                    var cvs = value as IReadOnlyList<Octokit.PullRequest>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "milestones")
                {
                    var cvs = value as IReadOnlyList<Octokit.Milestone>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "issueComments")
                {
                    var cvs = value as IReadOnlyList<Octokit.IssueComment>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "labels")
                {
                    var cvs = value as IReadOnlyList<Octokit.Label>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "contributors")
                {
                    var cvs = value as IReadOnlyList<Octokit.RepositoryContributor>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else if(parameter.ToString() == "users")
                {
                    var cvs = value as IReadOnlyList<Octokit.User>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
                else
                {
                    var cvs = value as List<Octokit.Repository>;
                    return cvs.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
