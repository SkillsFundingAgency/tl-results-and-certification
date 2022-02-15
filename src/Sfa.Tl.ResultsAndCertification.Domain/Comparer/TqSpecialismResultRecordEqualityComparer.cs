using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqSpecialismResultRecordEqualityComparer : IEqualityComparer<TqSpecialismResult>
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
            {
                var retVal = x.TqSpecialismAssessmentId == y.TqSpecialismAssessmentId
                    && x.TlLookupId == y.TlLookupId
                    && x.IsOptedin == y.IsOptedin;
                return retVal;
            }
        }

        public int GetHashCode(TqSpecialismResult specialismResult)
        {
            unchecked
            {
                var hashCode = specialismResult.TqSpecialismAssessmentId.GetHashCode();
                hashCode = (hashCode * 397) ^ specialismResult.TlLookupId.GetHashCode();
                hashCode = (hashCode * 397) ^ specialismResult.IsOptedin.GetHashCode();
                return hashCode;
            }
        }
    }
}
