using System;
using Newtonsoft.Json.Linq;

namespace SteamWebAPI.Utility
{
    internal static class TypeHelper
    {
        public static string CreateString(JToken token)
        {
            if (token != null)
                return token.ToString();
            else
                return String.Empty;
        }

        public static int CreateInt(string value)
        {
            int tempInt = 0;
            Int32.TryParse(value, out tempInt);
            return tempInt;
        }

        public static int CreateInt(JToken token)
        {
            if (token != null)
                return CreateInt(token.ToString());
            else
                return 0;
        }

        public static long CreateLong(string value)
        {
            long tempLong = 0;
            Int64.TryParse(value, out tempLong);
            return tempLong;
        }

        public static long CreateLong(JToken token)
        {
            if (token != null)
                return CreateLong(token.ToString());
            else
                return 0;
        }


        public static double CreateDouble(string value)
        {
            double tempDouble = 0;
            double.TryParse(value, out tempDouble);
            return tempDouble;
        }

        public static double CreateDouble(JToken token)
        {
            if (token != null)
                return CreateDouble(token.ToString());
            else
                return 0;
        }

        public static DateTime CreateDateTime(JToken token)
        {
            if (token != null)
                return DateHelper.UnixToDateTime(CreateLong(token.ToString()));
            else
                return DateTime.MinValue;
        }

        public static bool CreateBoolean(string value)
        {
            bool tempBool = false;
            bool.TryParse(value, out tempBool);
            return tempBool;
        }

        public static bool CreateBoolean(JToken token)
        {
            if (token != null)
                return CreateBoolean(token.ToString());
            else
                return false;
        }

        public static Uri CreateUri(JToken token)
        {
            if (token != null)
                return new Uri(token.ToString());
            else
                return new Uri("null");
        }
    }
}
