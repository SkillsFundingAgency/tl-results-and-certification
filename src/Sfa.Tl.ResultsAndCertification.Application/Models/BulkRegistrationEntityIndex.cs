using Sfa.Tl.ResultsAndCertification.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Application.Models
{
    internal class BulkRegistrationEntityIndex
    {
        public int PathwayAssessmentStartIndex { get; set; } = Constants.PathwayAssessmentsStartIndex;
        public int PathwayResultStartIndex { get; set; } = Constants.PathwayResultsStartIndex;
        public int SpecialismAssessmentStartIndex { get; set; } = Constants.SpecialismAssessmentsStartIndex;
        public int SpecialismResultStartIndex { get; set; } = Constants.SpecialismResultsStartIndex;
        public int IpStartIndex { get; set; } = Constants.IndustryPlacementStartIndex;
        public int OverallResultStartIndex { get; set; } = Constants.IndustryPlacementStartIndex;
    }
}
