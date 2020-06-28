using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqRegistrationProfileEqualityComparer : IEqualityComparer<TqRegistrationProfile>
    {
        public bool Equals(TqRegistrationProfile x, TqRegistrationProfile y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
            {
                var retVal = x.UniqueLearnerNumber == y.UniqueLearnerNumber 
                    && string.Equals(x.Firstname, y.Firstname)
                    && Equals(x.Lastname, y.Lastname)
                    && Equals(x.DateofBirth, y.DateofBirth);
                return retVal;
            }
        }

        public int GetHashCode(TqRegistrationProfile reg)
        {
            unchecked
            {
                var hashCode = reg.UniqueLearnerNumber.GetHashCode();
                hashCode = (hashCode * 397) ^ (reg.Firstname != null ? reg.Firstname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg.Lastname != null ? reg.Lastname.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (reg.DateofBirth != null ? reg.DateofBirth.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
