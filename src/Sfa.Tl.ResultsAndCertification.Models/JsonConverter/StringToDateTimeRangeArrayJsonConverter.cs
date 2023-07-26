using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using System;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.JsonConverter
{
    public class StringToDateTimeRangeArrayJsonConverter : JsonConverter<DateTimeRange[]>
    {
        private const char DateSplitChar = ',';
        private const string DateFormat = "MM/dd/yyyy";

        public override bool CanWrite => false;

        public override bool CanRead => true;

        public override DateTimeRange[] ReadJson(JsonReader reader, Type objectType, DateTimeRange[] existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JArray array = JArray.Load(reader);
            var results = new DateTimeRange[array.Count];

            for (int i = 0; i < array.Count; i++)
            {
                JToken token = array[i];
                string[] split = ((string)token).Split(DateSplitChar);

                DateTime from = ParseStringToDate(split[0]);
                DateTime to = ParseStringToDate(split[1]);

                results[i] = new DateTimeRange { From = from, To = to };
            }

            return results;
        }

        public override void WriteJson(JsonWriter writer, DateTimeRange[] value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private DateTime ParseStringToDate(string dateAsString)
            => DateTime.ParseExact(dateAsString, DateFormat, CultureInfo.InvariantCulture);
    }
}