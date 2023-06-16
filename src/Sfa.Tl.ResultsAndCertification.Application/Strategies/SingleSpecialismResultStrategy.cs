using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class SingleSpecialismResultStrategy : ISpecialismResultStrategy
    {
        public TqSpecialismResult GetResult(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null)
                throw new ArgumentNullException(nameof(specialisms), "The specialism collection cannot be null.");

            if (specialisms.Count != 1)
                throw new ArgumentException("The specialism collection must contain a single specialism.", nameof(specialisms));

            TqRegistrationSpecialism specialism = specialisms.Single();

            if (specialism == null || !specialism.TqSpecialismAssessments.Any())
                return null;

            // Get Q-Pending grade if they are any across the results
            var qPendingGrade = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).FirstOrDefault(x => x.TlLookup.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));

            // If there is Q-Pending grade then use that if not get the higher result
            var specialismHigherResult = qPendingGrade ?? specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return specialismHigherResult;
        }
    }
}