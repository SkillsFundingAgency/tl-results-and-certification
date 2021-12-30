using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class CoreResultsExport
    {
        [DisplayName(CoreAssessmentsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(CoreAssessmentsExportHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(CoreAssessmentsExportHeader.CoreAssessmentEntry)]
        public string CoreAssessmentEntry { get; set; }
    }
}
