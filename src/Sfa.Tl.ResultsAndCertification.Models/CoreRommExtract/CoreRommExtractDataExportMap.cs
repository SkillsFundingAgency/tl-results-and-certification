using CsvHelper.Configuration;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract
{
    public class CoreRommExtractDataExportMap : ClassMap<CoreRommExtractData>
    {
        public CoreRommExtractDataExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}