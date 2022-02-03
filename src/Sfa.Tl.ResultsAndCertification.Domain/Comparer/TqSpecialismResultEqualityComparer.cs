using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqSpecialismResultEqualityComparer : IEqualityComparer<TqSpecialismResult>
    {
        public bool Equals(TqSpecialismResult x, TqSpecialismResult y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqSpecialismAssessmentId == y.TqSpecialismAssessmentId;
        }

        public int GetHashCode(TqSpecialismResult result)
        {
            unchecked
            {
                var hashCode = result.TqSpecialismAssessmentId.GetHashCode();
                return hashCode;
            }
        }
    }
}
