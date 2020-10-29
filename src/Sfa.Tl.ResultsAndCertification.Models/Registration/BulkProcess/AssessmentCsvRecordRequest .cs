using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess
{
    public class AssessmentCsvRecordRequest : FileBaseModel
    {
        // TODO: FluentHeader to be created - [Display(Name)] attribute
        [Column(AssessmentFileHeader.Uln, Order = 0)]
        [Display(Name = AssessmentFileHeader.Uln)]
        public string Uln { get; set; }

        [Column(AssessmentFileHeader.CoreCode, Order = 1)]
        [Display(Name = AssessmentFileHeader.CoreCode)]
        public string CoreCode { get; set; }

        [Column(AssessmentFileHeader.CoreAssessmentEntry, Order = 2)]
        [Display(Name = AssessmentFileHeader.CoreAssessmentEntry)]
        public string CoreAssessmentEntry { get; set; }

        [Column(AssessmentFileHeader.SpecialismCode, Order = 3)]
        [Display(Name = AssessmentFileHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [Column(AssessmentFileHeader.SpecialismAssessmentEntry, Order = 4)]
        [Display(Name = AssessmentFileHeader.SpecialismAssessmentEntry)]
        public string SpecialismAssessmentEntry { get; set; }
    }
}
