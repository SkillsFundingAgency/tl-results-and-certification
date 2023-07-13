using System;

namespace Sfa.Tl.ResultsAndCertification.Models.AnalystResultsExtraction
{
    public class AnalystOverallResultExtractionData
    {
        public long UniqueLearnerNumber { get; set; }

        public string Status { get; set; }

        public long UkPrn { get; set; }

        public string ProviderName { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public DateTime DateofBirth { get; set; }

        public string Gender { get; set; }

        public string TlevelTitle { get; set; }

        public int StartYear { get; set; }

        public string CoreComponent { get; set; }

        public string LarId { get; set; }

        public string CoreCode { get; set; }

        public string CoreResult { get; set; }

        public string OccupationalSpecialism { get; set; }

        public string SpecialismCode { get; set; }

        public string SpecialismResult { get; set; }

        public string IndustryPlacementStatus { get; set; }

        public string OverallResult { get; set; }
    }
}