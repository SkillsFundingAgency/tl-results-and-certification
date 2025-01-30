using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class RommsExportMap : ClassMap<RommsExport>
    {
        public RommsExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.DateOfBirth).Ignore();
            Map(m => m.AcademicYear).Ignore();
        }
    }
}
