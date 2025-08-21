using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class SpecialismResultsExport
    {
        [DisplayName(SpecialismResultsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(SpecialismResultsExportHeader.AcademicYear)]
        public string? AcademicYear { get; set; }

        [DisplayName(SpecialismResultsExportHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(SpecialismResultsExportHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(SpecialismResultsExportHeader.SpecialismAssessmentEntry)]
        public string SpecialismAssessmentEntry { get; set; }

        [DisplayName(SpecialismResultsExportHeader.SpecialismGrade)]
        public string SpecialismGrade { get; set; }
    }
}