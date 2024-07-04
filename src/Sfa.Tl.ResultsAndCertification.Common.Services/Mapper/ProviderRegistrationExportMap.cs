using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class ProviderRegistrationExportMap : ClassMap<ProviderRegistrationExport>
    {
        public ProviderRegistrationExportMap() 
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}