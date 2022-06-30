using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Comparer
{
    internal class OverallResultEqualityComparer : IEqualityComparer<OverallResult>
    {
        public bool Equals(OverallResult x, OverallResult y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
                return x.TqRegistrationPathwayId == y.TqRegistrationPathwayId &&
                       x.Details.Equals(y.Details, StringComparison.InvariantCultureIgnoreCase) &&
                       x.ResultAwarded.Equals(y.ResultAwarded, StringComparison.InvariantCultureIgnoreCase) &&
                       x.CalculationStatus == y.CalculationStatus;
        }

        public int GetHashCode(OverallResult overallResult)
        {
            unchecked
            {
                var hashCode = overallResult.TqRegistrationPathwayId.GetHashCode();
                hashCode = (hashCode * 397) ^ overallResult.Details.GetHashCode();
                hashCode = (hashCode * 397) ^ overallResult.ResultAwarded.GetHashCode();
                hashCode = (hashCode * 397) ^ overallResult.CalculationStatus.GetHashCode();

                return hashCode;
            }
        }
    }
}
