using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    class ActivityIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            switch (value)
            {
                case "PushEvent":
                    return "\uF01F";
                case "WatchEvent":
                    return "\uF02A";
                case "CreateEvent":
                    return "\uF001";
                case "PullRequestEvent":
                    return "\uF009";
                case "CommitCommentEvent":
                    return null;
                case "ForkEvent":
                    return "\uF020";
                case "PullRequestReviewCommentEvent":
                case "IssueCommentEvent":
                    return "\uF04F";
                case "ReleaseEvent":
                    return "\uF015";
                case "IssuesEvent":
                    return "\uF026";
                case "DeleteEvent":
                case "DeploymentEvent":
                case "DeploymentStatusEvent":
                case "DownloadEvent":
                case "FollowEvent":
                case "ForkApplyEvent":
                case "GistEvent":
                case "GollumEvent":
                case "LabelEvent":
                case "MemberEvent":
                case "MembershipEvent":
                case "MilestoneEvent":
                case "OrganizationEvent":
                case "OrgBlockEvent":
                case "PageBuildEvent":
                case "ProjectCardEvent":
                case "ProjectColumnEvent":
                case "ProjectEvent":
                case "PublicEvent":
                case "PullRequestReviewEvent":
                case "RepositoryEvent":
                case "StatusEvent":
                case "TeamEvent":
                case "TeamAddEvent":
                default:
                    return null;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
