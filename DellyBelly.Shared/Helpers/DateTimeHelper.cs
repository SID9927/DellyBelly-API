using System;

namespace DellyBelly.Shared.Helpers
{
    public static class DateTimeHelper
    {
        private static readonly TimeZoneInfo IndianZone = GetIndianTimeZone();

        private static TimeZoneInfo GetIndianTimeZone()
        {
            try
            {
                // Windows ID
                return TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                // Linux/macOS (IANA ID)
                return TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            }
        }

        public static DateTime GetIndianTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, IndianZone);
        }

        public static DateTime ToIndianTime(this DateTime dateTime)
        {
            DateTime utcDateTime = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, IndianZone);
        }
    }
}
