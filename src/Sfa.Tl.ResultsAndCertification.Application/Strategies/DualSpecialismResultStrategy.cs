using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class DualSpecialismResultStrategy : SpecialismResultStrategyBase
    {
        private readonly IEnumerable<TlLookup> _tlLookup;
        private readonly IEnumerable<DualSpecialismOverallGradeLookup> _dualSpecialismOverallGradeLookups;

        public DualSpecialismResultStrategy(IEnumerable<TlLookup> tlLookup, IEnumerable<DualSpecialismOverallGradeLookup> dualSpecialismOverallGradeLookups)
        {
            _tlLookup = tlLookup;
            _dualSpecialismOverallGradeLookups = dualSpecialismOverallGradeLookups;
        }

        public override TlLookup GetResult(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null)
                throw new ArgumentNullException(nameof(specialisms), "The specialism collection cannot be null.");

            if (specialisms.Count != 2)
                throw new ArgumentException("The specialism collection must contain two specialisms.", nameof(specialisms));

            TqSpecialismResult firstSpecialismResult = GetHighestResult(specialisms.First());
            TqSpecialismResult secondSpecialismResult = GetHighestResult(specialisms.Last());

            if (IsSpecialismGradeXNoResult(firstSpecialismResult) || IsSpecialismGradeXNoResult(secondSpecialismResult))
            {
                return GetSpecialismTLookupResult(Constants.SpecialismComponentGradeXNoResultCode);
            }

            if (IsSpecialismGradeQPending(firstSpecialismResult) || IsSpecialismGradeQPending(secondSpecialismResult))
            {
                return GetSpecialismTLookupResult(Constants.SpecialismComponentGradeQpendingResultCode);
            }

            if (firstSpecialismResult == null || secondSpecialismResult == null)
            {
                return null;
            }

            if (IsSpecialismGradeUnclassified(firstSpecialismResult) || IsSpecialismGradeUnclassified(secondSpecialismResult))
            {
                return GetSpecialismTLookupResult(Constants.SpecialismComponentGradeUnclassifiedCode);
            }

            var overallGrade = _dualSpecialismOverallGradeLookups.FirstOrDefault(o => o.FirstTlLookupSpecialismGradeId == firstSpecialismResult.TlLookupId && o.SecondTlLookupSpecialismGradeId == secondSpecialismResult.TlLookupId);
            return overallGrade?.TlLookupOverallSpecialismGrade;
        }

        private bool IsSpecialismGradeQPending(TqSpecialismResult specialismResult)
        {
            return IsComponentGradeWithCode(specialismResult?.TlLookupId, Constants.SpecialismComponentGradeQpendingResultCode);
        }

        private bool IsSpecialismGradeXNoResult(TqSpecialismResult specialismResult)
        {
            return IsComponentGradeWithCode(specialismResult?.TlLookupId, Constants.SpecialismComponentGradeXNoResultCode);
        }

        private bool IsSpecialismGradeUnclassified(TqSpecialismResult specialismResult)
        {
            return IsComponentGradeWithCode(specialismResult?.TlLookupId, Constants.SpecialismComponentGradeUnclassifiedCode);
        }

        private bool IsComponentGradeWithCode(int? gradeId, string gradeCode)
        {
            return gradeId.HasValue ? _tlLookup.Any(o => o.Id == gradeId.Value && o.Code.Equals(gradeCode, StringComparison.InvariantCultureIgnoreCase)) : false;
        }

        private TlLookup GetSpecialismTLookupResult(string code)
        {
            return _tlLookup.FirstOrDefault(o => o.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}