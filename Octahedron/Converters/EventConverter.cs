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
                var grayBrush = new SolidColorBrush(new Color { A = 255, R = 136, G = 136, B = 136 });
                switch (eventType)
                {
                    case EventInfoState.Assigned:
                    case EventInfoState.Unassigned:
                        StackPanel assignPanel = new StackPanel();
                        StackPanel assignTextPanel = new StackPanel { Orientation = Orientation.Horizontal };
                        assignTextPanel.Children.Add(new TextBlock { Text = eventInfo.Actor.Login, FontWeight = FontWeights.Bold });
                        assignTextPanel.Children.Add(new TextBlock { Text = constants.r_loader.GetString(eventType == EventInfoState.Assigned ? eventInfo.Assignee.Login == eventInfo.Actor.Login ? "eventSelfAssigned" : "eventAssigned" : "eventUnassigned"), Margin = new Thickness(6, 0, 6, 0) });
                        assignTextPanel.Children.Add(new TextBlock { Text = eventInfo.Assignee.Login, FontWeight = FontWeights.Bold, Visibility = eventInfo.Assignee.Login == eventInfo.Actor.Login ? Visibility.Collapsed : Visibility.Visible });
                        assignPanel.Children.Add(assignTextPanel);
                        assignPanel.Children.Add(new TextBlock { Text = utilities.FormatDate(eventInfo.CreatedAt.DateTime), Foreground = grayBrush, Style = (Style)App.Current.Resources["CaptionTextBlockStyle"] });
                        return assignPanel;
                    case EventInfoState.Labeled:
                    case EventInfoState.Unlabeled:
                        StackPanel panel = new StackPanel();
                        StackPanel textPanel = new StackPanel { Orientation = Orientation.Horizontal };
                        textPanel.Children.Add(new TextBlock { Text = eventInfo.Actor.Login, FontWeight = FontWeights.Bold });
                        textPanel.Children.Add(new TextBlock { Text = constants.r_loader.GetString(eventType == EventInfoState.Labeled ? "added" : "removed"), Margin = new Thickness(6, 0, 6, 0) });
                        var label = new Grid() { Background = utilities.ConvertHexToBrush(eventInfo.Label.Color) };
                        label.Children.Add(new TextBlock()
                        {
                            Text = eventInfo.Label.Name,
                            Style = (Style)App.Current.Resources["CaptionTextBlockStyle"],
                            Margin = new Thickness(6, 3, 6, 3),
                            Foreground = new SolidColorBrush(utilities.CheckColorType(utilities.ConvertHexToColor(eventInfo.Label.Color)) == utilities.colorType.Dark ? Colors.White : Colors.Black)
                        });
                        textPanel.Children.Add(label);
                        panel.Children.Add(textPanel);
                        panel.Children.Add(new TextBlock { Text = utilities.FormatDate(eventInfo.CreatedAt.DateTime), Foreground = grayBrush, Style = (Style)App.Current.Resources["CaptionTextBlockStyle"] });
                        return panel;
                    case EventInfoState.Closed:
                        StackPanel closedPanel = new StackPanel();
                        StackPanel closedTextPanel = new StackPanel() { Orientation = Orientation.Horizontal };
                        closedTextPanel.Children.Add(new TextBlock { Text = eventInfo.Actor.Login, FontWeight = FontWeights.Bold });
                        closedTextPanel.Children.Add(new TextBlock { Text = constants.r_loader.GetString( eventInfo.CommitId == null ? "eventClosed" : "eventClosedCommit"), Margin = new Thickness(6, 0, 6, 0) });
                        if(eventInfo.CommitId != null)
                        {
                            string sha = eventInfo.CommitId;
                            closedTextPanel.Children.Add(new TextBlock { Text = sha.Remove(7), FontWeight = FontWeights.Bold });
                        }
                        closedPanel.Children.Add(closedTextPanel);
                        closedPanel.Children.Add(new TextBlock { Text = utilities.FormatDate(eventInfo.CreatedAt.DateTime), Foreground = grayBrush, Style = (Style)App.Current.Resources["CaptionTextBlockStyle"] });
                        return closedPanel;
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
