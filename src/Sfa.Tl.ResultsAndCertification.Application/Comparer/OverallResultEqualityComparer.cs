using Sfa.Tl.ResultsAndCertification.Common.Extensions;
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
                       x.SpecialismResultAwarded.Equals(y.SpecialismResultAwarded, StringComparison.InvariantCultureIgnoreCase) &&
                       x.CalculationStatus == y.CalculationStatus;
        }

        public int GetHashCode(OverallResult overallResult)
        {
            unchecked
            {
                var hashCode = overallResult.TqRegistrationPathwayId.GetHashCode();
                hashCode = (hashCode * 397) ^ overallResult.Details.GetHashCodeWithNullCheck();
                hashCode = (hashCode * 397) ^ overallResult.ResultAwarded.GetHashCodeWithNullCheck();
                hashCode = (hashCode * 397) ^ overallResult.SpecialismResultAwarded.GetHashCodeWithNullCheck();
                hashCode = (hashCode * 397) ^ overallResult.CalculationStatus.GetHashCode();

                return hashCode;
            }
        }
    }
}
