using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class IndustryPlacementRecordEqualityComparer : IEqualityComparer<IndustryPlacement>
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
                return x.TqRegistrationPathwayId == y.TqRegistrationPathwayId &&
                       x.Status == y.Status &&
                       x.Details.Equals(y.Details, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(IndustryPlacement placement)
        {
            unchecked
            {
                var hashCode = placement.TqRegistrationPathwayId.GetHashCode();
                hashCode = (hashCode * 397) ^ placement.Details.GetHashCode();
                hashCode = (hashCode * 397) ^ placement.Status.GetHashCode();

                return hashCode;
            }
        }
    }
}
