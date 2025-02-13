using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvConverters
{
    public class BoleanToYesNoConverter : BooleanConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return value.GetType() == typeof(bool) ? (bool)value ? "Yes" : "No" : base.ConvertToString(value, row, memberMapData);
        }
    }
}
