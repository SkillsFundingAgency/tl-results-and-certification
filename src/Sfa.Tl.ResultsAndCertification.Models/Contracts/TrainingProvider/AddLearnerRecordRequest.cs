using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class AddLearnerRecordRequest
    {
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public EnglishAndMathsStatus? EnglishAndMathsStatus { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }
        public string PerformedBy { get; set; }
    }
}