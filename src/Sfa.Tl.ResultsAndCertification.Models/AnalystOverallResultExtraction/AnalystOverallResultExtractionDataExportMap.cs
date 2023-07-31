using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction
{
    public class AnalystOverallResultExtractionDataExportMap : ClassMap<AnalystOverallResultExtractionData>
    {
        public AnalystOverallResultExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}