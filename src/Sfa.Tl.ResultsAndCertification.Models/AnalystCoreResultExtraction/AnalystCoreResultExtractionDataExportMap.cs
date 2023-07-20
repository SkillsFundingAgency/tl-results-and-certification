using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction
{
    public class AnalystCoreResultExtractionDataExportMap : ClassMap<AnalystCoreResultExtractionData>
    {
        public AnalystCoreResultExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.TlevelTitle).TypeConverter<StringConverter>();
            Map(m => m.CoreComponent).TypeConverter<StringConverter>();
        }
    }

    public class StringConverter : DefaultTypeConverter
    {
        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return $"\"{value.ToString()}\"";
        }
    }
}