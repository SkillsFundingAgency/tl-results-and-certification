using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sfa.Tl.ResultsAndCertification.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName<T>(this T enumValue) where T : System.Enum
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            ?.First()
                            ?.GetCustomAttribute<DisplayAttribute>()
                            ?.GetName() ?? enumValue.ToString();
        }

        public static string GetDisplayName<T>(object value) where T : System.Enum
        {
            return IsValidValue<T>(value) ? ((T)(value.ToString().IsInt() ? value.ToString().ToInt() : value)).GetDisplayName() : null;
        }

        public static List<T> GetList<T>() where T : System.Enum
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool IsValidValue<T>(object value) where T : System.Enum
        {
            return value != null && System.Enum.IsDefined(typeof(T), value.ToString().IsInt() ? value.ToString().ToInt() : value);
        }

        public static bool IsValidDisplayName<T>(object value) where T : System.Enum
        {
            return GetList<T>().Any(x => x.GetDisplayName().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

        public static int? GetEnumValueByName<T>(object value) where T : System.Enum
        {
            return IsValidValue<T>(value) ? (int)System.Enum.Parse(typeof(T), value.ToString(), true) : (int?)null;
        }

        public static int? GetEnumValueByDisplayName<T>(string value) where T : System.Enum
        {
            var enumType = GetList<T>().FirstOrDefault(x => x.GetDisplayName().Equals(value, StringComparison.InvariantCultureIgnoreCase));
            return GetEnumValueByName<T>(enumType);
        }
    }
}
