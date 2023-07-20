using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult
{
    public abstract class PathwayResultConverterBase
    {
        protected TqPathwayResult Convert(IEnumerable<TqPathwayAssessment> assesments)
        {
            if (assesments.IsNullOrEmpty())
                return null;

            var pathwayResults = assesments.SelectMany(x => x.TqPathwayResults);

            // Get Q-Pending grade if they are any across the results
            var qPendingGrade = pathwayResults.FirstOrDefault(x => x.TlLookup.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));

            // If there is Q-Pending grade then use that if not get the higher result
            var pathwayHigherResult = qPendingGrade ?? pathwayResults.OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return pathwayHigherResult;
        }
    }
}