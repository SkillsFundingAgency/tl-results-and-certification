using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess
{
    public class RegistrationCsvRecordRequest : FileBaseModel
    {
        [Column(RegistrationHeader.Uln, Order = 0)]
        [Display(Name = RegistrationFluentHeader.Uln)]
        public string Uln { get; set; }

        [Column(RegistrationHeader.FirstName, Order = 1)]
        [Display(Name = RegistrationFluentHeader.FirstName)]
        public string FirstName { get; set; }

        [Column(RegistrationHeader.LastName, Order = 2)]
        [Display(Name = RegistrationFluentHeader.LastName)]
        public string LastName { get; set; }

        [Column(RegistrationHeader.DateOfBirth, Order = 3)]
        [Display(Name = RegistrationFluentHeader.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Column(RegistrationHeader.Ukprn, Order = 4)]
        [Display(Name = RegistrationFluentHeader.Ukprn)]
        public string Ukprn { get; set; }

        [Column(RegistrationHeader.AcademicYear, Order = 5)]
        [Display(Name = RegistrationFluentHeader.AcademicYear)]
        public string AcademicYear { get; set; }

        [Column(RegistrationHeader.Core, Order = 6)]
        [Display(Name = RegistrationFluentHeader.Core)]
        public string Core { get; set; }

        [Column(RegistrationHeader.Specialisms, Order = 7)]
        [Display(Name = RegistrationFluentHeader.Specialisms)]
        public string Specialisms { get; set; }
    }
}
