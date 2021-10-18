using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.Mapper
{
    public class CsvMapper : ClassMap<UcasDataRecord>
    {
        public CsvMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.UcasDataComponents).Ignore();
        }
    }
}
