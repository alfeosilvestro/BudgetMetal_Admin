﻿using System;
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

        public static string EncodeString(this string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static string DecodeString(this string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
    }
}
