using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HRM.Core.Extensions
{
	public static class DateTimeExtension
    {
        public static string ToDisplayString(this DateTime date)
        {
            return date.ToLocalTime().ToString("hh:mm tt ddd, MMM dd, yyyy");
        }

        public static string ToISOString(this DateTime date)
        { 
            return date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public static bool IsNull(this DateTime value)
        {
            if (value != null && value > DateTime.MinValue && value < DateTime.MaxValue)
                return false;
            return true;
        }

        public static long ToTicks(this DateTime dt)
        {
            DateTime oldDate = new DateTime(1970, 1, 1);
            return dt.Ticks - oldDate.Ticks;
        }

        public static DateTime FromTicks(this DateTime dt, long ticks)
        {
            DateTime oldDate = new DateTime(1970, 1, 1);
            return new DateTime(ticks + oldDate.Ticks);
        }

        public static DateTime? FromTicks(this DateTime? dt, long? ticks)
        {
            if (ticks.HasValue)
            {
                DateTime oldDate = new DateTime(1970, 1, 1);
                return new DateTime?(new DateTime(ticks.Value + oldDate.Ticks));
            }
            else
            {
                return null;
            }
        }
    }
}