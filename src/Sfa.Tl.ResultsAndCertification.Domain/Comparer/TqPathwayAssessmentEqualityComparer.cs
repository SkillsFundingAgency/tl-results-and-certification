using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqPathwayAssessmentEqualityComparer : IEqualityComparer<TqPathwayAssessment>
    {
        public bool Equals(TqPathwayAssessment x, TqPathwayAssessment y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationPathwayId == y.TqRegistrationPathwayId &&
                    ((x.AssessmentSeriesId == y.AssessmentSeriesId) || (x.AssessmentSeriesId != y.AssessmentSeriesId && y.AssessmentSeriesId <= 0));
                   //(sameSeriesId || (IsRemovalRequest))
        }

        public int GetHashCode(TqPathwayAssessment assessment)
        {
            unchecked
            {
                var hashCode = assessment.TqRegistrationPathwayId.GetHashCode();
                return hashCode;
            }
        }
    }
}
