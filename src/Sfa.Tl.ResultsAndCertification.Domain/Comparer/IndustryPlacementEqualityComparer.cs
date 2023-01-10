using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class IndustryPlacementEqualityComparer : IEqualityComparer<IndustryPlacement>
    {
        public bool Equals(IndustryPlacement x, IndustryPlacement y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationPathwayId == y.TqRegistrationPathwayId;
        }

        public int GetHashCode(IndustryPlacement placement)
        {
            unchecked
            {
                var hashCode = placement.TqRegistrationPathwayId.GetHashCode();
                return hashCode;
            }
        }
    }
}
