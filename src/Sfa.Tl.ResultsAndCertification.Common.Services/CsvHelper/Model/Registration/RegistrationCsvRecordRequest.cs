using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    public class RegistrationCsvRecordRequest : FileBaseModel
    { 
        [Column(RegistrationHeader.Uln, Order = 0)]
        [Display(Name = RegistrationHeader.Uln)]
        public string Uln { get; set; }

        [Column(RegistrationHeader.FirstName, Order = 1)]
        [Display(Name = RegistrationHeader.FirstName)]
        public string FirstName { get; set; }

        [Column(RegistrationHeader.LastName, Order = 2)]
        [Display(Name = RegistrationHeader.LastName)]
        public string LastName { get; set; }

        [Column(RegistrationHeader.DateOfBirth, Order = 3)]
        [Display(Name = RegistrationHeader.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Column(RegistrationHeader.Ukprn, Order = 4)]
        [Display(Name = RegistrationHeader.Ukprn)]
        public string Ukprn { get; set; }

        [Column(RegistrationHeader.StartDate, Order = 5)]
        [Display(Name = RegistrationHeader.StartDate)]
        public string StartDate { get; set; }

        [Column(RegistrationHeader.Core, Order = 6)]
        [Display(Name = RegistrationHeader.Core)]
        public string Core { get; set; }

        [Column(RegistrationHeader.Specialisms, Order = 7)]
        [Display(Name = "Specialism code")]
        public string Specialisms { get; set; }
    }
}
