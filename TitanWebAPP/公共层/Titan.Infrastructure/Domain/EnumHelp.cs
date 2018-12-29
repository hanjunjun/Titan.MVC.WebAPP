using System;

namespace Titan.Infrastructure.Domain
{
    public static class EnumHelp
    {
        public static T ToEnum<T>(this string value, T defaultValue = default(T))
        {
            if (value == null)
                return defaultValue;

            bool found = false;
            foreach (string name in Enum.GetNames(typeof(T)))
            {
                if (value == name)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return defaultValue;

            T result = (T)Enum.Parse(typeof(T), value);
            if (result == null)
                return defaultValue;
            return result;
        }

        public static T ToIntToEnum<T>(this string value, T defaultValue = default(T))
        {
            int n;
            int.TryParse(value,out n);
            return n.ToEnum<T>();
        }

        public static T ToEnum<T>(this int value, T defaultValue = default(T))
        {
            bool found = false;
            foreach (int eValue in Enum.GetValues(typeof(T)))
            {
                if (value == eValue)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
                return defaultValue;

            T result = (T)Enum.ToObject(typeof(T), value);
            if (result == null)
                return defaultValue;
            return result;
        }

        public static int ToInt<T>(this T value)
        {
            return Convert.ToInt32(value);
        }

        public static string ToValueString<T>(this T value)
        {
            return Convert.ToInt32(value).ToString();
        }
    }
}
