using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
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

        public override OverallSpecialismResultDetail GetResult(ICollection<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms == null)
                throw new ArgumentNullException(nameof(specialisms), "The specialism collection cannot be null.");

            if (specialisms.Count != 2)
                throw new ArgumentException("The specialism collection must contain two specialisms.", nameof(specialisms));

            TqRegistrationSpecialism firstSpecialism = specialisms.First();
            TqSpecialismResult firstSpecialismResult = GetHighestResult(firstSpecialism);

            TqRegistrationSpecialism secondSpecialism = specialisms.Last();
            TqSpecialismResult secondSpecialismResult = GetHighestResult(secondSpecialism);

            TlLookup overallSpecialismResult = null;

            if (IsSpecialismGradeXNoResult(firstSpecialismResult) || IsSpecialismGradeXNoResult(secondSpecialismResult))
            {
                overallSpecialismResult = GetSpecialismTLookupResult(Constants.SpecialismComponentGradeXNoResultCode);
            }
            else if (IsSpecialismGradeQPending(firstSpecialismResult) || IsSpecialismGradeQPending(secondSpecialismResult))
            {
                overallSpecialismResult = GetSpecialismTLookupResult(Constants.SpecialismComponentGradeQpendingResultCode);
            }
            else if (firstSpecialismResult == null || secondSpecialismResult == null)
            {
                overallSpecialismResult = null;
            }
            else if (IsSpecialismGradeUnclassified(firstSpecialismResult) || IsSpecialismGradeUnclassified(secondSpecialismResult))
            {
                overallSpecialismResult = GetSpecialismTLookupResult(Constants.SpecialismComponentGradeUnclassifiedCode);
            }
            else
            {
                var overallGrade = _dualSpecialismOverallGradeLookups.FirstOrDefault(o => o.FirstTlLookupSpecialismGradeId == firstSpecialismResult.TlLookupId && o.SecondTlLookupSpecialismGradeId == secondSpecialismResult.TlLookupId);
                overallSpecialismResult = overallGrade?.TlLookupOverallSpecialismGrade;
            }

            return CreateOverallSpecialismResultDetail(firstSpecialism, firstSpecialismResult, secondSpecialism, secondSpecialismResult, overallSpecialismResult);
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

        private OverallSpecialismResultDetail CreateOverallSpecialismResultDetail(
            TqRegistrationSpecialism firstSpecialism,
            TqSpecialismResult firstSpecialismResult,
            TqRegistrationSpecialism secondSpecialism,
            TqSpecialismResult secondSpecialismResult,
            TlLookup overallSpecialismResult)
        {
            return new OverallSpecialismResultDetail
            {
                SpecialismDetails = new List<OverallSpecialismDetail>
                {
                    CreateOverallSpecialismDetail(firstSpecialism, firstSpecialismResult),
                    CreateOverallSpecialismDetail(secondSpecialism, secondSpecialismResult),
                },
                TlLookupId = overallSpecialismResult?.Id,
                OverallSpecialismResult = overallSpecialismResult?.Value
            };
        }
    }
}