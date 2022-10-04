using System;
using System.Globalization;

namespace GoKart.Converter
{
    internal static class Format
    {
        public static string LapTime(TimeSpan? time)
        {
            if (time == null)
            {
                return String.Empty;
            }

            return F("{0:00}:{1:00}.{2:000}", time?.Minutes, time?.Seconds, time?.Milliseconds);
        }

        public static string TimeLeft(TimeSpan? time)
        {
            if (time == null)
            {
                return String.Empty;
            }

            return F("{0:00}:{1:00}:{2:00}.{3:000}", time?.Hours, time?.Minutes, time?.Seconds, time?.Milliseconds);
        }

        public static string Speed(float? speed)
        {
            if (speed == null)
            {
                return String.Empty;
            }

            return F("{0:00.00}", speed);
        }

        private static string F(string format, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
    }
}