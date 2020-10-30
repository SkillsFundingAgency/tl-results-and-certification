using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess
{
    public class AssessmentCsvRecordRequest : FileBaseModel
    {
        [Column(AssessmentFileHeader.Uln, Order = 0)]
        [Display(Name = AsessmentFluentHeader.Uln)]
        public string Uln { get; set; }

        [Column(AssessmentFileHeader.CoreCode, Order = 1)]
        [Display(Name = AsessmentFluentHeader.CoreCode)]
        public string CoreCode { get; set; }

        [Column(AssessmentFileHeader.CoreAssessmentEntry, Order = 2)]
        [Display(Name = AsessmentFluentHeader.CoreAssessmentEntry)]
        public string CoreAssessmentEntry { get; set; }

        [Column(AssessmentFileHeader.SpecialismCode, Order = 3)]
        [Display(Name = AsessmentFluentHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [Column(AssessmentFileHeader.SpecialismAssessmentEntry, Order = 4)]
        [Display(Name = AsessmentFluentHeader.SpecialismAssessmentEntry)]
        public string SpecialismAssessmentEntry { get; set; }
    }
}
