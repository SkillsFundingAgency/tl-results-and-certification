using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.HighestAttainedCoreSeries
{
    public abstract class HighestAttainedCoreSeriesConverterBase
    {
        protected TqPathwayResult Convert(IEnumerable<TqPathwayAssessment> assessments)
        {
            if (assessments.IsNullOrEmpty())
                return null;

            var pathwayResults = assessments.SelectMany(x => x.TqPathwayResults);
            var bestPathwayResult = pathwayResults.OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return bestPathwayResult;
        }
    }
}
