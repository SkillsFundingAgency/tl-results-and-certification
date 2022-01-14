using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class CoreResultsExport
    {
        [DisplayName(CoreResultsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(CoreResultsExportHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(CoreResultsExportHeader.CoreAssessmentEntry)]
        public string CoreAssessmentEntry { get; set; }

        [DisplayName(CoreResultsExportHeader.CoreGrade)]
        public string CoreGrade { get; set; }
    }
}
