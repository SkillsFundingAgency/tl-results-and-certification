using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess
{
    public class ResultCsvRecordRequest : FileBaseModel
    {
        [Column(ResultFileHeader.Uln, Order = 0)]
        [Display(Name = ResultFluentHeader.Uln)]
        public string Uln { get; set; }

        [Column(ResultFileHeader.CoreCode, Order = 1)]
        [Display(Name = ResultFluentHeader.CoreCode)]
        public string CoreCode { get; set; }

        [Column(ResultFileHeader.CoreAssessmentSeries, Order = 2)]
        [Display(Name = ResultFluentHeader.CoreAssessmentSeries)]
        public string CoreAssessmentSeries { get; set; }

        [Column(ResultFileHeader.CoreGrade, Order = 3)]
        [Display(Name = ResultFluentHeader.CoreGrade)]
        public string CoreGrade { get; set; }

        [Column(ResultFileHeader.SpecialismCodes, Order = 4)]
        [Display(Name = ResultFluentHeader.SpecialismCodes)]
        public string SpecialismCodes { get; set; }

        [Column(ResultFileHeader.SpecialismSeries, Order = 5)]
        [Display(Name = ResultFluentHeader.SpecialismSeries)]
        public string SpecialismSeries { get; set; }

        [Column(ResultFileHeader.SpecialismGrades, Order = 6)]
        [Display(Name = ResultFluentHeader.SpecialismGrades)]
        public string SpecialismGrades { get; set; }
    }
}
