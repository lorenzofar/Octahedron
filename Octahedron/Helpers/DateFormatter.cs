using System;

namespace Octahedron.Helpers
{
    public class DateFormatter
    {
        private static string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public static string AnalyzeDate(DateTime date)
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
                        return "just now";
                    }
                    else
                    {
                        return $"{hours} hours ago";
                    }
                }
                else if (daysSpan < 30)
                {
                    var days = Math.Round(daysSpan);
                    return $"{days} days ago";
                }
                else
                {
                    return $"on {months[date.Month - 1]} {date.Day}";
                }
            }
            else
            {
                return $"on {months[date.Month - 1]} {date.Day}, {date.Year}";
            }
        }
    }
}
