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

        public static List<T> GetList<T>() where T : System.Enum
        {
            return System.Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static bool IsValidValue<T>(int value) where T : System.Enum
        {
            return System.Enum.IsDefined(typeof(T), value);
        }
    }
}
