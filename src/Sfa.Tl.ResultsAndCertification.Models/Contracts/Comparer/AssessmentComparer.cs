using System;
using System.Collections.Generic;
using PathwayAssessment = Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Comparer
{
    public class AssessmentComparer: IEqualityComparer<PathwayAssessment>
    {
        public bool Equals(PathwayAssessment x, PathwayAssessment y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            
            return x.SeriesId == y.SeriesId && x.SeriesName == y.SeriesName;
        }

        public int GetHashCode(PathwayAssessment assessment)
        {
            unchecked
            {
                var hashCode = assessment.SeriesId.GetHashCode();
                return hashCode;
            }
        }
    }
}
