using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqPathwayAssessmentRecordEqualityComparer : IEqualityComparer<TqPathwayAssessment>
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
            {
                var retVal = x.TqRegistrationPathwayId == y.TqRegistrationPathwayId
                    && x.AssessmentSeriesId == y.AssessmentSeriesId
                    && x.IsOptedin == y.IsOptedin;                    
                return retVal;
            }
        }

        public int GetHashCode(TqPathwayAssessment pathwayAssessment)
        {
            unchecked
            {
                var hashCode = pathwayAssessment.TqRegistrationPathwayId.GetHashCode();
                hashCode = (hashCode * 397) ^ pathwayAssessment.AssessmentSeriesId.GetHashCode();
                hashCode = (hashCode * 397) ^ pathwayAssessment.IsOptedin.GetHashCode();
                return hashCode;
            }
        }
    }
}
