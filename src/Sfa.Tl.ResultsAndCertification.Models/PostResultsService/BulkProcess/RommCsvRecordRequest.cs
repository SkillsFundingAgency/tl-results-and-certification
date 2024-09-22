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

        [Column(RommHeader.Core, Order = 6)]
        [Display(Name = RommFluentHeader.Core)]
        public string Core { get; set; }

        [Column(RommHeader.Specialisms, Order = 7)]
        [Display(Name = RommFluentHeader.Specialisms)]
        public string Specialisms { get; set; }
    }
}
