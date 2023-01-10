using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public class IndustryPlacementCsvRecordResponse : ValidationState<BulkProcessValidationError>
    {
        public IndustryPlacementCsvRecordResponse()
        {
            SpecialConsiderations = new List<string>();
        }

        public int RowNum { get; set; }
        public long Uln { get; set; }
        public string CoreCode { get; set; }
        public string IndustryPlacementStatus { get; set; }
        public string IndustryPlacementHours { get; set; }
        public IList<string> SpecialConsiderations { get; set; }        
    }
}
