using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess
{
    public class RommsCsvRecordRequest : FileBaseModel
    {
        [Column(RommHeader.Uln, Order = 0)]
        [Display(Name = RommFluentHeader.Uln)]
        public string Uln { get; set; }

        [Column(RommHeader.FirstName, Order = 1)]
        [Display(Name = RommFluentHeader.FirstName)]
        public string FirstName { get; set; }

        [Column(RommHeader.LastName, Order = 2)]
        [Display(Name = RommFluentHeader.LastName)]
        public string LastName { get; set; }

        [Column(RommHeader.DateOfBirth, Order = 3)]
        [Display(Name = RommFluentHeader.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Column(RommHeader.Ukprn, Order = 4)]
        [Display(Name = RommFluentHeader.Ukprn)]
        public string Ukprn { get; set; }

        [Column(RommHeader.AcademicYear, Order = 5)]
        [Display(Name = RommFluentHeader.AcademicYear)]
        public string AcademicYear { get; set; }

        [Column(RommHeader.AssessmentSeriesCore, Order = 6)]
        [Display(Name = RommFluentHeader.AssessmentSeriesCore)]
        public string AssessmentSeriesCore { get; set; }

        [Column(RommHeader.Core, Order = 7)]
        [Display(Name = RommFluentHeader.CoreRommOpen)]
        public string Core { get; set; }

        [Column(RommHeader.CoreRommOpen, Order = 8)]
        [Display(Name = RommFluentHeader.CoreRommOpen)]
        public string CoreRommOpen { get; set; }

        [Column(RommHeader.CoreRommOutcome, Order = 9)]
        [Display(Name = RommFluentHeader.CoreRommOutcome)]
        public string CoreRommOutcome { get; set; }

        [Column(RommHeader.AssessmentSeriesSpecialism, Order = 10)]
        [Display(Name = RommFluentHeader.AssessmentSeriesSpecialism)]
        public string AssessmentSeriesSpecialism { get; set; }

        [Column(RommHeader.Specialism, Order = 11)]
        [Display(Name = RommFluentHeader.Specialism)]
        public string Specialism { get; set; }

        [Column(RommHeader.SpecialismRommOpen, Order = 12)]
        [Display(Name = RommFluentHeader.SpecialismRommOpen)]
        public string SpecialismRommOpen { get; set; }

        [Column(RommHeader.SpecialismRommOutcome, Order = 13)]
        [Display(Name = RommFluentHeader.SpecialismRommOutcome)]
        public string SpecialismRommOutcome { get; set; }
    }
}
