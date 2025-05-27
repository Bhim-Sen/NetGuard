using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CommonMethods
{
	public static class IndianTimeZone
	{
		public static DateTime GetIndianTimeZone()
		{
			// Get the current time in India by converting UTC time to Indian Standard Time (IST)
			TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
			DateTime currentTimeInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianTimeZone);
			return currentTimeInIndia;
		}
	}
}
