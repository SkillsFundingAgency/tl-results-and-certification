using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqPathwayResultEqualityComparer : IEqualityComparer<TqPathwayResult>
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
                return x.TqPathwayAssessmentId == y.TqPathwayAssessmentId;
        }

        public int GetHashCode(TqPathwayResult result)
        {
            unchecked
            {
                var hashCode = result.TqPathwayAssessmentId.GetHashCode();
                return hashCode;
            }
        }
    }
}