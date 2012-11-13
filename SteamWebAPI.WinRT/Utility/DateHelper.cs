using System;

namespace SteamWebAPI.Utility
{
    internal static class DateHelper
    {
        public static DateTime UnixToDateTime(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>Takes a DateTime and converts it to the seconds since 1/1/1970 represented as a Int64 (long).
        /// The passed DateTime is preferably occurring after 1/1/1970, behavior is undefined otherwise.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnix(DateTime dateTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            TimeSpan timeSpanSinceOrigin = dateTime.Subtract(origin);

            return Convert.ToInt64(timeSpanSinceOrigin.TotalSeconds);
        }
    }
}
