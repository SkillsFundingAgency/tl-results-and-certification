using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class RegistrationsExport
    {
        [DisplayName(RegistrationFluentHeader.Uln)]
        public string Uln { get; set; }

        [DisplayName(RegistrationFluentHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(RegistrationFluentHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(RegistrationFluentHeader.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [DisplayName(RegistrationFluentHeader.Ukprn)]
        public string Ukprn { get; set; }

        [DisplayName(RegistrationFluentHeader.AcademicYear)]
        public string AcademicYear { get; set; }

        [DisplayName(RegistrationFluentHeader.Core)]
        public string Core { get; set; }

        [DisplayName(RegistrationFluentHeader.Specialisms)]
        public string Specialisms { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }
    }
}
