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
    }
}
