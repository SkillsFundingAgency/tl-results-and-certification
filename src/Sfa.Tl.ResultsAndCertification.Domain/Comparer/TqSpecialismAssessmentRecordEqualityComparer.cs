using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqSpecialismAssessmentRecordEqualityComparer : IEqualityComparer<TqSpecialismAssessment>
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
            {
                var retVal = x.TqRegistrationSpecialismId == y.TqRegistrationSpecialismId
                    && x.AssessmentSeriesId == y.AssessmentSeriesId
                    && x.IsOptedin == y.IsOptedin;
                return retVal;
            }
        }

        public int GetHashCode(TqSpecialismAssessment specialismAssessment)
        {
            unchecked
            {
                var hashCode = specialismAssessment.TqRegistrationSpecialismId.GetHashCode();
                hashCode = (hashCode * 397) ^ specialismAssessment.AssessmentSeriesId.GetHashCode();
                hashCode = (hashCode * 397) ^ specialismAssessment.IsOptedin.GetHashCode();
                return hashCode;
            }
        }
    }
}
