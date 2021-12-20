using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqSpecialismAssessmentEqualityComparer : IEqualityComparer<TqSpecialismAssessment>
    {
        public bool Equals(TqSpecialismAssessment x, TqSpecialismAssessment y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationSpecialismId == y.TqRegistrationSpecialismId &&
                    ((x.AssessmentSeriesId == y.AssessmentSeriesId) || (x.AssessmentSeriesId != y.AssessmentSeriesId && y.AssessmentSeriesId <= 0));
                    //(sameSeriesId || (IsRemovalRequest))
        }

        public int GetHashCode(TqSpecialismAssessment assessment)
        {
            unchecked
            {
                var hashCode = assessment.TqRegistrationSpecialismId.GetHashCode();
                return hashCode;
            }
        }
    }
}
