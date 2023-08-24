using Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction;
using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract
{
    public class CoreRommExtractData
    {
        [DisplayName(AnalystOverallResultExtractionHeader.Uln)]
        public long UniqueLearnerNumber { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.Status)]
        public string Status { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.Ukprn)]
        public long Ukprn { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.ProviderName)]
        public string ProviderName { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.DateOfBirth)]
        public DateOnly DateOfBirth { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.Gender)]
        public string Gender { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.TLevel)]
        public string TlevelTitle { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.StartYear)]
        public string StartYear { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.CoreComponent)]
        public string CoreComponent { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.CoreResult)]
        public string CoreResult { get; set; }
    }
}