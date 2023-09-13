using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction
{
    public class SpecialRommExtractionDataExportMap : ClassMap<SpecialRommExtractionData>
    {
        public SpecialRommExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}