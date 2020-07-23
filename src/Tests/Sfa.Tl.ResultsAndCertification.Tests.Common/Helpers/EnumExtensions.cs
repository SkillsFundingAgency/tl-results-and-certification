using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum val)
        {
            return val.GetType()
                      .GetMember(val.ToString())
                      .FirstOrDefault()
                      ?.GetCustomAttribute<DisplayAttribute>(false)
                      ?.Name
                      ?? val.ToString();
        }

        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            T val = ((T[])System.Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (var enumValue in from T enumValue in (T[])System.Enum.GetValues(typeof(T))
                                          where enumValue.ToString().ToUpper().Equals(str.ToUpper())
                                          select enumValue)
                {
                    val = enumValue;
                    break;
                }
            }

            return val;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            T val = ((T[])System.Enum.GetValues(typeof(T)))[0];
            foreach (var enumValue in from T enumValue in (T[])System.Enum.GetValues(typeof(T))
                                    where Convert.ToInt32(enumValue).Equals(intValue)
                                    select enumValue)
            {
                val = enumValue;
                break;
            }
            return val;
        }
    }
}
