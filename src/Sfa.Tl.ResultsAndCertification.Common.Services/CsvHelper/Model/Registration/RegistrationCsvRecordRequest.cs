using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using CsvHeader = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants.RegistrationHeader;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    public class RegistrationCsvRecordRequest : FileBaseModel
    {
        [Name(CsvHeader.Uln)]
        [Display(Name = "ULN")]
        public string Uln { get; set; }

        [Required]
        [Name(CsvHeader.FirstName)]
        [Display(Name="First name")]
        public string FirstName { get; set; }

        [Required]
        [Name(CsvHeader.LastName)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Name(CsvHeader.DateOfBirth)]
        [Display(Name = "Date of birth")]
        public string DateOfBirth { get; set; }

        [Required]
        [Name(CsvHeader.Ukprn)]
        [Display(Name = "UKPRN")]
        public string Ukprn { get; set; }

        [Required]
        [Name(CsvHeader.StartDate)]
        [Display(Name = "Start date")]
        public string StartDate { get; set; }

        [Required]
        [Name(CsvHeader.Core)]
        [Display(Name = "Core code")]
        public string Core { get; set; }

        [Required]
        [Name(CsvHeader.Specialisms)]
        [Display(Name = "Specialism")]
        public string Specialisms { get; set; }
    }
}
