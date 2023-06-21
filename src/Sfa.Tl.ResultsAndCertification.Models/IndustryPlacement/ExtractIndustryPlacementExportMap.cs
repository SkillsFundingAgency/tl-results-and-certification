using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.ExtractIndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement
{
    public class ExtractIndustryPlacementExportMap : ClassMap<ExtractData>
    {
        public ExtractIndustryPlacementExportMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
