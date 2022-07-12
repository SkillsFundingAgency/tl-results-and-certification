using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class DownloadOverallResultsExportMap : ClassMap<DownloadOverallResultsData>
    {
        public DownloadOverallResultsExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);            
            Map(m => m.DateOfBirth).Ignore();
            Map(m => m.AcademicYear).Ignore();
        }
    }
}
