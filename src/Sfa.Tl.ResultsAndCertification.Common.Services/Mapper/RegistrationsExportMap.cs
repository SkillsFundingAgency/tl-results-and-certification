using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class RegistrationsExportMap : ClassMap<RegistrationsExport>
    {
        public RegistrationsExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);            
            Map(m => m.DateOfBirth).Ignore();
            Map(m => m.AcademicYear).Ignore();
            Map(m => m.CreatedOn).Ignore();
            Map(m => m.SpecialismsList).Ignore();
        }
    }
}
