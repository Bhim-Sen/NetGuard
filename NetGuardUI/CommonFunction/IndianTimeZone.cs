﻿namespace NetGuardUI.CommonFunction
{
    public static class IndianTimeZone
    {
        public static DateTime GetIndianTimeZone()
        {    // Get the Indian Standard Time zone
            TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            // Get the current time in the Indian Standard Time zone
            DateTime currentTimeInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianTimeZone);
            return currentTimeInIndia;
        }
    }
}
