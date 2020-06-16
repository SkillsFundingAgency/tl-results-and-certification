using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using CsvHeader = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants.RegistrationHeader;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration
{
    public class RegistrationCsvRecordRequest : FileBaseModel
    {
        [Name(CsvHeader.Uln)]
        [Display(Name = CsvHeader.Uln)]
        public string Uln { get; set; }

        [Name(CsvHeader.FirstName)]
        [Display(Name = CsvHeader.FirstName)]
        public string FirstName { get; set; }

        [Name(CsvHeader.LastName)]
        [Display(Name = CsvHeader.LastName)]
        public string LastName { get; set; }

        [Name(CsvHeader.DateOfBirth)]
        [Display(Name = CsvHeader.DateOfBirth)]
        public string DateOfBirth { get; set; }

        [Name(CsvHeader.Ukprn)]
        [Display(Name = CsvHeader.Ukprn)]
        public string Ukprn { get; set; }

        [Name(CsvHeader.StartDate)]
        [Display(Name = CsvHeader.StartDate)]
        public string StartDate { get; set; }

        [Name(CsvHeader.Core)]
        [Display(Name = CsvHeader.Core)]
        public string Core { get; set; }

        [Name(CsvHeader.Specialisms)]
        [Display(Name = "Specialism code")]
        public string Specialisms { get; set; }
    }
}
