using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class AddLearnerRecordRequest
    {
        public long Ukprn { get; set; }
        public long Uln { get; set; }
        public bool HasLrsEnglishAndMaths { get; set; }
        public EnglishAndMathsStatus? EnglishAndMathsStatus { get; set; }
        public EnglishAndMathsLrsStatus? EnglishAndMathsLrsStatus { get; set; }
        public IndustryPlacementStatus IndustryPlacementStatus { get; set; }
        public string PerformedBy { get; set; }
        public string PerformedUserEmail { get; set; }        
    }
}