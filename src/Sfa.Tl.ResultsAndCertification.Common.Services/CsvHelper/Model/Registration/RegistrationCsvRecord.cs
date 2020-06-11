using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using Header = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants.RegistrationHeader;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    public class RegistrationCsvRecord : FileBaseModel
    {
        [Required]
        [Name(Header.Uln)]
        public string Uln { get; set; }

        [Required]
        [Name(Header.FirstName)]
        public string FirstName { get; set; }

        [Required]
        [Name(Header.LastName)]
        public string LastName { get; set; }

        [Required]
        [Name(Header.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Required]
        [Name(Header.Ukprn)]
        public string Ukprn { get; set; }

        [Required]
        [Name(Header.StartDate)]
        public string StartDate { get; set; }

        [Required]
        [Name(Header.Core)]
        public string Core { get; set; }

        [Required]
        [Name(Header.Specialisms)]
        public string Specialisms { get; set; }

        //[Ignore]
        //public int RowNum { get; set; }
    }
}
