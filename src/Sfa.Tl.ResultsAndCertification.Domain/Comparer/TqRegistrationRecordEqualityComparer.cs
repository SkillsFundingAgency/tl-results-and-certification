using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Domain.Comparer
{
    public class TqRegistrationRecordEqualityComparer : IEqualityComparer<TqRegistrationProfile>
    {
        public bool Equals(TqRegistrationProfile x, TqRegistrationProfile y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
            {
                var retVal = x.UniqueLearnerNumber == y.UniqueLearnerNumber
                            && string.Equals(x.Firstname, y.Firstname)
                            && Equals(x.Lastname, y.Lastname)
                            && Equals(x.DateofBirth, y.DateofBirth)
                            && ArePathwaysAndSpecialismsEqual(x, y);

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

                foreach (var registrationPathway in reg.TqRegistrationPathways)
                {
                    hashCode = (hashCode * 397) ^ registrationPathway.TqProviderId.GetHashCode();
                    hashCode = (hashCode * 397) ^ registrationPathway.RegistrationDate.GetHashCode();
                    //hashCode = (hashCode * 397) ^ registrationPathway.StartDate.GetHashCode();
                    //hashCode = (hashCode * 397) ^ (registrationPathway.EndDate != null ? registrationPathway.EndDate.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ registrationPathway.Status.GetHashCode();

                    foreach (var registrationSpecialism in registrationPathway.TqRegistrationSpecialisms)
                    {
                        hashCode = (hashCode * 397) ^ registrationSpecialism.TlSpecialismId.GetHashCode();
                        hashCode = (hashCode * 397) ^ registrationSpecialism.Status.GetHashCode();
                    }
                }
                return hashCode;
            }
        }

        private bool ArePathwaysAndSpecialismsEqual(TqRegistrationProfile x, TqRegistrationProfile y)
        {
            bool isPathwayEqual = (x.TqRegistrationPathways.Count == y.TqRegistrationPathways.Count);
            if (isPathwayEqual)
            {
                foreach (var xpathway in x.TqRegistrationPathways)
                {
                    var ypathway = y.TqRegistrationPathways.FirstOrDefault(p => p.TqProviderId == xpathway.TqProviderId);
                    isPathwayEqual = EqualsTqRegistrationPathway(xpathway, ypathway);

                    if (!isPathwayEqual) break;

                    bool areSpecialismEqual = AreSpecialismsEqual(xpathway, ypathway);

                    if (!isPathwayEqual || !areSpecialismEqual)
                    {
                        isPathwayEqual = false;
                        break;
                    }
                }
            }
            return isPathwayEqual;
        }

        private bool AreSpecialismsEqual(TqRegistrationPathway xpathway, TqRegistrationPathway ypathway)
        {
            var isSpecialismEqual = (xpathway.TqRegistrationSpecialisms.Count == ypathway.TqRegistrationSpecialisms.Count);
            if (isSpecialismEqual)
            {
                foreach (var xspecialism in xpathway.TqRegistrationSpecialisms)
                {
                    var yspecialism = ypathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.TlSpecialismId == xspecialism.TlSpecialismId);
                    isSpecialismEqual = EqualsTqRegistrationSpecialism(xspecialism, yspecialism);

                    if (!isSpecialismEqual) break;
                }
            }
            return isSpecialismEqual;
        }

        private bool EqualsTqRegistrationPathway(TqRegistrationPathway x, TqRegistrationPathway y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
            {
                var retVal =
                    x.TqProviderId == y.TqProviderId
                    && Equals(x.RegistrationDate, y.RegistrationDate)
                    //&& Equals(x.StartDate, y.StartDate)
                    //&& Equals(x.EndDate, y.EndDate)
                    && x.Status == y.Status;
                return retVal;
            }
        }

        private bool EqualsTqRegistrationSpecialism(TqRegistrationSpecialism x, TqRegistrationSpecialism y)
        {
            if (x == null && x == null)
                return true;
            else if (x == null || x == null)
                return false;
            else if (x.GetType() != y.GetType())
                return false;
            else
            {
                var retVal = x.TlSpecialismId == y.TlSpecialismId && x.Status == y.Status;
                return retVal;
            }
        }
    }
}
