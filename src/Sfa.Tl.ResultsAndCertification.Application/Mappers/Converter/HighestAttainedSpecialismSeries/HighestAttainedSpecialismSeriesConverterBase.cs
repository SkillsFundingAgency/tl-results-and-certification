using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.HighestAttainedSpecialismSeries
{
    public abstract class HighestAttainedSpecialismSeriesConverterBase
    {
        protected TqSpecialismResult Convert(IEnumerable<TqRegistrationSpecialism> specialisms)
        {
            if (specialisms.IsNullOrEmpty())
            {
                return null;
            }

            var specialismResults = specialisms
                .SelectMany(s => s.TqSpecialismAssessments)
                .SelectMany(a => a.TqSpecialismResults);

            if (specialismResults.IsNullOrEmpty())
            {
                return null;
            }

            var bestSpecialismResult = specialismResults.OrderBy(r => r.TlLookup.SortOrder).FirstOrDefault();

            return bestSpecialismResult;
        }
    }
}