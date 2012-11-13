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

        public static int CreateInt(JToken token)
        {
            if (token != null)
                return Convert.ToInt32(token.ToString());
            else
                return 0;
        }

        public static long CreateLong(JToken token)
        {
            if (token != null)
                return Convert.ToInt64(token.ToString());
            else
                return 0;
        }

        public static DateTime CreateDateTime(JToken token)
        {
            return DateHelper.UnixToDateTime(CreateLong(token.ToString()));
        }

        public static bool CreateBoolean(JToken token)
        {
            return Convert.ToBoolean(token.ToString());
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
