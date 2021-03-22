using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace TimeUpdater
{
    public class TimeConverter
    {
        public static List<double> ConvertToUnixDateTime(List<DateTime> dateTimes)
        {
            var retUnixDateTimes = new List<double>();

            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            foreach(var dateTime in dateTimes)
            {
                retUnixDateTimes.Add((dateTime.ToUniversalTime() - epoch).TotalSeconds);
            }

            return retUnixDateTimes;
        }

        public static DateTime ConvertUnixTimeToDateTime(double unixDateTime)
        {
            var timespan = TimeSpan.FromSeconds(unixDateTime);
            var localDateTime = new DateTime(timespan.Ticks).ToLocalTime();

            return localDateTime;
        }

        public static double CalculateDailyTime(List<DateTime> dateTimes)
        {
            TimeSpan timeMorning = dateTimes[1] - dateTimes[0];
            TimeSpan timeAfternoon = dateTimes[3] - dateTimes[2];

            return Math.Round(timeMorning.TotalHours + timeAfternoon.TotalHours, 2);
        }
    }
}
