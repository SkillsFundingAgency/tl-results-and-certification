using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class SpecialismResultsExport
    {
        [DisplayName(SpecialismAssessmentsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(SpecialismAssessmentsExportHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(SpecialismAssessmentsExportHeader.SpecialismAssessmentEntry)]
        public string SpecialismAssessmentEntry { get; set; }
    }
}
