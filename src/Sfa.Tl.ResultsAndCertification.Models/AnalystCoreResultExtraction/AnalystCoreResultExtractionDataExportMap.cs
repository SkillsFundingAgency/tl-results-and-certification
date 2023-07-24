using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction
{
    public class AnalystCoreResultExtractionDataExportMap : ClassMap<AnalystCoreResultExtractionData>
    {
        public AnalystCoreResultExtractionDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}