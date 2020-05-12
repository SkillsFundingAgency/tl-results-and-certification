using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Sfa.Tl.ResultsAndCertification.Web.Helpers
{
    public static class TempDataHelper
    {
        public static void Set<T>(this ITempDataDictionary tempData, string key, T value)
        {
            if (tempData != null)
            {
                tempData[key] = JsonConvert.SerializeObject(value);
            }
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key)
        {
            if (tempData == null) return default;
            tempData.TryGetValue(key, out object returnValue);
            return returnValue == null ? default : JsonConvert.DeserializeObject<T>(returnValue.ToString());
        }
    }
}
