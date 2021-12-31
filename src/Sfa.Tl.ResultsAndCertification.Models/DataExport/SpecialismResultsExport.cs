using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class SpecialismResultsExport
    {
        [DisplayName(SpecialismResultsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(SpecialismResultsExportHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(SpecialismResultsExportHeader.SpecialismAssessmentEntry)]
        public string SpecialismAssessmentEntry { get; set; }


        [DisplayName(SpecialismResultsExportHeader.SpecialismGrade)]
        public string SpecialismGrade { get; set; }
    }
}
