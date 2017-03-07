using System;
using System.Globalization;

namespace Ingesup.Maze.Server.Web.Helper
{
    public static class DateTimeHelper
    {
        public static string ToString(DateTime value)
        {
            return value.ToString("dd/MM/yyyy HH:mm");
        }

        public static string ToString(TimeSpan value)
        {
            return value.ToString(@"d\.hh\:mm\:ss\.fff");
        }

        public static string ToUniversalDateTime(DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ss.fff");
        }

        public static DateTime FromUniversalDateTime(string value)
        {
            if (string.IsNullOrEmpty(value) || string.Equals(value, "null", StringComparison.InvariantCultureIgnoreCase))
            {
                return DateTime.MinValue;
            }

            return DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
        }
    }
}