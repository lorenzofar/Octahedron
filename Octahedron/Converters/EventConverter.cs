using Helper;
using Octokit;
using System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Octahedron.Converters
{
    public class EventConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var eventInfo = (EventInfo)value;
                var eventType = eventInfo.Event;
                var grayBrush = new SolidColorBrush(new Color {A = 255, R = 136, G = 136, B = 136 });
                switch (eventType)
                {
                    case EventInfoState.Assigned:
                    case EventInfoState.Unassigned:
                        return null;
                    case EventInfoState.Labeled:
                    case EventInfoState.Unlabeled:
                        StackPanel panel = new StackPanel { Orientation = Orientation.Horizontal };
                        panel.Children.Add(new TextBlock { Text = eventInfo.Actor.Login, FontWeight = FontWeights.Bold });
                        panel.Children.Add(new TextBlock { Text = constants.r_loader.GetString(eventType == EventInfoState.Labeled ? "added" : "removed"), Foreground = grayBrush, Margin = new Thickness(6, 0, 6, 0) });
                        var label = new Grid() { Background = utilities.ConvertHexToBrush(eventInfo.Label.Color) };
                        label.Children.Add(new TextBlock()
                        {
                            Text = eventInfo.Label.Name,
                            Style = (Style)App.Current.Resources["CaptionTextBlockStyle"],
                            Margin = new Thickness(6, 3, 6, 3),
                            Foreground = new SolidColorBrush(utilities.CheckColorType(utilities.ConvertHexToColor(eventInfo.Label.Color)) == utilities.colorType.Dark ? Colors.White : Colors.Black)
                        });
                        panel.Children.Add(label);
                        panel.Children.Add(new TextBlock { Text = utilities.FormatDate(eventInfo.CreatedAt.DateTime), Foreground = grayBrush, Margin = new Thickness(6, 0, 0, 0) });
                        return panel;
                    case EventInfoState.Closed:
                        return null;
                    case EventInfoState.Demilestoned:
                    case EventInfoState.Milestoned:
                        return null;
                    case EventInfoState.Locked:
                    case EventInfoState.Unlocked:
                        return null;
                    case EventInfoState.Referenced:
                        return null;
                    case EventInfoState.Renamed:
                        return null;
                    case EventInfoState.Reopened:
                        return null;
                    default:
                        return null;
                };
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
