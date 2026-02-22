using System;

namespace DellyBelly.Shared.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo IndianZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public static DateTime GetIndianTime()
        {
            var utcNow = DateTime.UtcNow;
            return TimeZoneInfo.ConvertTimeFromUtc(utcNow, IndianZone);
        }

        public static DateTime ToIndianTime(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(dateTime, IndianZone);
            }
            return TimeZoneInfo.ConvertTime(dateTime, IndianZone);
        }
    }
}
