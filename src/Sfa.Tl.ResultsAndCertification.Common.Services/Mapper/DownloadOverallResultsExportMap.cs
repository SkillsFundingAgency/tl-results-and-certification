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
            Map(m => m.Details.TlevelTitle).Ignore();
            Map(m => m.Details.PathwayName).Ignore();
            Map(m => m.Details.PathwayLarId).Ignore();
            Map(m => m.Details.PathwayResult).Ignore();
            Map(m => m.Details.SpecialismDetails).Ignore();
            Map(m => m.Details.IndustryPlacementStatus).Ignore();
            Map(m => m.Details.OverallResult).Ignore();
        }
    }
}
