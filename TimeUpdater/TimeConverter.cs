using System;
using System.Collections.Generic;

namespace TimeUpdater
{
    public static class TimeConverter
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
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime = epoch.AddSeconds(unixDateTime).ToLocalTime();
            return dateTime;
        }

        public static double CalculateDailyTime(List<DateTime> dateTimes)
        {
            TimeSpan timeMorning = dateTimes[1] - dateTimes[0];
            TimeSpan timeAfternoon = dateTimes[3] - dateTimes[2];

            return Math.Round(timeMorning.TotalHours + timeAfternoon.TotalHours, 2);
        }

        /// <summary>
        /// Convert e.g.time to int araray with hours and minutes.
        /// E.g 07:43  =>  [7,43]
        /// </summary>
        /// <param name="time">e.g  '07:43'</param>
        /// <returns>array [0] => hours  array [1] => minutes</returns>
        public static int[] ConvertTimeToNumbers(string time)
        {
            var hoursMinutesStrings = time.Split(":");

            if(hoursMinutesStrings.Length == 2)
            {
                try
                {
                    var hoursMinutesIntegers = new int[2];
                    hoursMinutesIntegers[0] = Convert.ToInt32(hoursMinutesStrings[0]);
                    hoursMinutesIntegers[1] = Convert.ToInt32(hoursMinutesStrings[1]);
                    return hoursMinutesIntegers;
                }
                catch
                {
                    ;
                    throw;
                }
            }

            return null;
        }
    }
}
