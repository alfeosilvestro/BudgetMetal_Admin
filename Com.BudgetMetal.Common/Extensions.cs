using System;
using System.Collections.Generic;
using System.Text;

namespace Com.BudgetMetal.Common
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrEmpty(this DateTime value)
        {
            return (value == null);
        }

        public static string ToApplicationShortDateTimeFormat(this DateTime value)
        {
            if (value.IsNullOrEmpty()) return null;

            return value.ToString("dd/MM/yyyy");
        }

        public static string ToShortString(this Guid value)
        {
            return Convert.ToBase64String(value.ToByteArray());
        }

        public static string ToApplicationLongDateTimeFormat(this DateTime value)
        {
            if (value.IsNullOrEmpty()) return null;

            return value.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
