using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class RegistrationsExport
    {
        [DisplayName(RegistrationFluentHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(RegistrationFluentHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(RegistrationFluentHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(RegistrationFluentHeader.DateOfBirth)]
        public string DisplayDateOfBirth => DateOfBirth.ToString("ddMMyyyy");

        [DisplayName(RegistrationFluentHeader.Ukprn)]
        public long Ukprn { get; set; }

        [DisplayName(RegistrationFluentHeader.AcademicYear)]
        public string DisplayAcademicYear => AcademicYear.ToString().Length == 4 ? $"{AcademicYear}/{(AcademicYear + 1).ToString().Substring(2)}" : string.Empty;

        [DisplayName(RegistrationFluentHeader.Core)]
        public string Core { get; set; }

        [DisplayName(RegistrationFluentHeader.Specialisms)]
        public string Specialisms { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int AcademicYear { get; set; }       

        public DateTime CreatedOn { get; set; }
    }
}
