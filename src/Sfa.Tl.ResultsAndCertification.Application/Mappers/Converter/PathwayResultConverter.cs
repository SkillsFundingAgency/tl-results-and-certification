using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class PathwayResultConverter : IPathwayResultConverter
    {
        public TqPathwayResult Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            if (sourceMember.TqPathwayAssessments.IsNullOrEmpty())
                return null;

            var pathwayResults = sourceMember.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults);

            // Get Q-Pending grade if they are any across the results
            var qPendingGrade = pathwayResults.FirstOrDefault(x => x.TlLookup.Code.Equals(Constants.PathwayComponentGradeQpendingResultCode, StringComparison.InvariantCultureIgnoreCase));

            // If there is Q-Pending grade then use that if not get the higher result
            var pathwayHigherResult = qPendingGrade ?? pathwayResults.OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return pathwayHigherResult;
        }
    }
}