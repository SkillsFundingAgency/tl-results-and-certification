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
            //Map(m => m.AcademicYear).Ignore();
            //Map(m => m.Details.TlevelTitle).Ignore();
            //Map(m => m.Details.PathwayName).Ignore();
            //Map(m => m.Details.PathwayLarId).Ignore();
            //Map(m => m.Details.PathwayResult).Ignore();
            //Map(m => m.Details.SpecialismDetails).Ignore();
            //Map(m => m.Details.IndustryPlacementStatus).Ignore();
            //Map(m => m.Details.OverallResult).Ignore();
        }
    }
}
