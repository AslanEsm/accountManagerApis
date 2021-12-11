using MD.PersianDateTime.Standard;
using System;
using System.Globalization;

namespace common.Utilities
{
    public static class ConvertDate
    {
        public static DateTime ToMiladiDateTime(this DateTime date)
        {
            if (date == DateTime.MinValue)
                return default;

            var stringResult = date.ToString("u");
            PersianDateTime persianDateTime = PersianDateTime.Parse(stringResult);
            return persianDateTime.ToDateTime();
        }

        public static DateTime ToMiladiDateTime(this string date)
        {
            if (string.IsNullOrEmpty(date))
                return default;

            PersianDateTime persianDateTime = PersianDateTime.Parse(date);
            return persianDateTime.ToDateTime();
        }
        //==========================================================================================//
        public static DateTime ToShamsiDateTime(this DateTime date)
        {
            if (date == DateTime.MinValue)
                return default;

            PersianDateTime persianDateTime = new PersianDateTime(date);
            return persianDateTime;
        }

        public static string ToShamsiDateTime(this DateTime date, string format)
        {
            if (date == DateTime.MinValue)
                return default;

            PersianDateTime persianDateTime = new PersianDateTime(date);
            return persianDateTime.ToString(format);
        }

        public static DateTime NullableToShamsiDateTime(this DateTime? date)
        {
            try
            {
                if (date != DateTime.MinValue)
                {
                    PersianDateTime persianDateTime = new PersianDateTime(date);
                    return persianDateTime;
                }
            }
            catch (Exception)
            {
                return default;
            }

            return default;
        }

        public static string NullableToShamsiDateTime(this DateTime? date, string format)
        {
            try
            {
                if (date != DateTime.MinValue)
                {
                    PersianDateTime persianDateTime = new PersianDateTime(date);
                    return persianDateTime.ToString(format);
                }
            }
            catch (Exception)
            {
                return default;
            }

            return default;
        }

        public static string FormatShamsi(this DateTime date, string format)
        {
            if (date == DateTime.MinValue)
                return default;

            var stringResult = date.ToString(format);
            return stringResult;
        }

        //=================================================================================================//
        public static DateTime Now(this DateTime date)
        {
            DateTime dateTime = DateTime.Now;
            return dateTime;
        }
        public static DateTime Now(this string date)
        {
            DateTime dateTime = DateTime.Now;
            return dateTime;
        }
    }
}