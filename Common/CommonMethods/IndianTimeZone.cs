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
			TimeZoneInfo indianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
			DateTime currentTimeInIndia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, indianTimeZone);
			return currentTimeInIndia;
		}
	}
}
