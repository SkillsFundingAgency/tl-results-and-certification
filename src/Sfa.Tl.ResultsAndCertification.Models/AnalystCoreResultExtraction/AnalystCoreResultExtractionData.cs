using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultExtraction;
using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultsExtraction
{
    public class AnalystCoreResultExtractionData
    {
        [DisplayName(AnalystCoreResultExtractHeader.Uln)]
        public long UniqueLearnerNumber { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.Status)]
        public string Status { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.UKPRN)]
        public long UkPrn { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.ProviderName)]
        public string ProviderName { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.DateOfBirth)]
        public DateOnly DateofBirth { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.Gender)]
        public string Gender { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.TLevel)]
        public string TlevelTitle { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.StartYear)]
        public int StartYear { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.CoreComponent)]
        public string CoreComponent { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(AnalystCoreResultExtractHeader.CoreResult)]
        public string CoreResult { get; set; }
    }
}