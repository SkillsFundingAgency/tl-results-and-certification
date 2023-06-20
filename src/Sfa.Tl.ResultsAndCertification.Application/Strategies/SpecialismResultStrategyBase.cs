using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public abstract class SpecialismResultStrategyBase : ISpecialismResultStrategy
    {
        protected TqSpecialismResult GetHighestResult(TqRegistrationSpecialism specialism)
        {
            if (specialism == null || !specialism.TqSpecialismAssessments.Any())
                return null;

            // Get Q-Pending grade if they are any across the results
            var qPendingGrade = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).FirstOrDefault(x => x.TlLookup.Code.Equals(Constants.SpecialismComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));

            // If there is Q-Pending grade then use that if not get the higher result
            var specialismHigherResult = qPendingGrade ?? specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return specialismHigherResult;
        }

        protected OverallSpecialismDetail CreateOverallSpecialismDetail(TqRegistrationSpecialism registrationSpecialism, TqSpecialismResult specialismResult)
        {
            return new OverallSpecialismDetail
            {
                SpecialismName = registrationSpecialism.TlSpecialism.Name,
                SpecialismLarId = registrationSpecialism.TlSpecialism.LarId,
                SpecialismResult = specialismResult?.TlLookup?.Value
            };
        }

        public abstract OverallSpecialismResultDetail GetResult(ICollection<TqRegistrationSpecialism> specialisms);
    }
}
