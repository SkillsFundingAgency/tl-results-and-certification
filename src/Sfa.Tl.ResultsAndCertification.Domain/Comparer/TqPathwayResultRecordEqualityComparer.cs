using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqPathwayResultRecordEqualityComparer : IEqualityComparer<TqPathwayResult>
    {
        public bool Equals(TqPathwayResult x, TqPathwayResult y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
            {
                var retVal = x.TqPathwayAssessmentId == y.TqPathwayAssessmentId
                    && x.TlLookupId == y.TlLookupId
                    && x.IsOptedin == y.IsOptedin;
                return retVal;
            }
        }

        public int GetHashCode(TqPathwayResult pathwayResult)
        {
            unchecked
            {
                var hashCode = pathwayResult.TqPathwayAssessmentId.GetHashCode();
                hashCode = (hashCode * 397) ^ pathwayResult.TlLookupId.GetHashCode();
                hashCode = (hashCode * 397) ^ pathwayResult.IsOptedin.GetHashCode();
                return hashCode;
            }
        }
    }
}
