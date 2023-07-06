using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Application.Interfaces
{
    public interface IOverallGradeStrategy
    {
        string GetOverAllGrade(int tlPathwayId, int? pathwayGradeId, int? specialismGradeId, IndustryPlacementStatus ipStatus);
    }
}