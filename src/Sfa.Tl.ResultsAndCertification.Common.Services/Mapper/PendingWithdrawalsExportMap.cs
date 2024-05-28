using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class PendingWithdrawalsExportMap : ClassMap<PendingWithdrawalsExport>
    {
        public PendingWithdrawalsExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);            
            Map(m => m.DateOfBirth).Ignore();
            Map(m => m.AcademicYear).Ignore();
            Map(m => m.CreatedOn).Ignore();
            Map(m => m.SpecialismsList).Ignore();
            Map(m => m.Specialisms).TypeConverterOption.Format("N0");
        }
    }
}