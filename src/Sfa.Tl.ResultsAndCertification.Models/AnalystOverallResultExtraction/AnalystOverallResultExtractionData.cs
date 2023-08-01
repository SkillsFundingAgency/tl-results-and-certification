using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystOverallResultExtraction
{
    public class AnalystOverallResultExtractionData
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

        [DisplayName(AnalystOverallResultExtractionHeader.OccupationalSpecialism)]
        public string OccupationalSpecialism { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.SpecialismResult)]
        public string SpecialismResult { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get; set; }

        [DisplayName(AnalystOverallResultExtractionHeader.OverallResult)]
        public string OverallResult { get; set; }
    }
}