using Octokit;
using System;
using Windows.UI.Xaml.Data;

namespace Octahedron.Converters
{
    public class EventIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var eventType = (EventInfoState)value;
            switch (eventType)
            {
                case EventInfoState.Assigned:
                case EventInfoState.Unassigned:
                    return "\uF018";
                case EventInfoState.Labeled:
                case EventInfoState.Unlabeled:
                    return "\uF015";
                case EventInfoState.Closed:
                    return "\uF028";
                case EventInfoState.Demilestoned:
                case EventInfoState.Milestoned:
                    return "\uF075";
                case EventInfoState.Locked:
                case EventInfoState.Unlocked:
                    return "\uF06A";
                case EventInfoState.Referenced:
                    return "\uF01F";
                case EventInfoState.Renamed:
                    return "\uF058";
                case EventInfoState.Reopened:
                    return "\uF027";
                default:
                    return "\uF068";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
