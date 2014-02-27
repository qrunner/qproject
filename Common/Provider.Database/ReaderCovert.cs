using System;

namespace Provider.Database
{
    public static class ReaderConvert
    {
        public static byte ToByte(object value)
        {
            return Convert.ToByte(value);
        }

        public static int ToInteger(object value)
        {
            return Convert.ToInt32(value);
        }

        public static int? ToIntegerNullable(object value)
        {
            if (value == null || value == Convert.DBNull)            
                return null;

            return Convert.ToInt32(value);
        }

        public static string ToString(object value)
        {
            return Convert.ToString(value);
        }

        public static bool ToBool(object value)
        {
            return Convert.ToBoolean(value);
        }

        public static long ToLong(object value)
        {
            return Convert.ToInt64(value);
        }

        public static long? ToLongNullable(object value)
        {
            if (value == null || value == Convert.DBNull)
                return null;
            
            return Convert.ToInt64(value);
        }

        public static decimal ToDecimal(object value)
        {
            return Convert.ToDecimal(value);
        }

        public static decimal? ToDecimalNullable(object value)
        {
            if (value == null || value == Convert.DBNull)
                return null;
            
            return Convert.ToDecimal(value);
        }

        public static DateTime ToDateTime(object value)
        {
            return Convert.ToDateTime(value);
        }

        public static DateTime? ToDateTimeNullable(object value)
        {
            if (value == null || value == Convert.DBNull)
                return null;
            
            return Convert.ToDateTime(value);
        }
    }
}