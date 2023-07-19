using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult
{
    public class PathwayResultStringConverter : PathwayResultConverterBase, IValueConverter<IEnumerable<TqPathwayAssessment>, string>
    {
        public string Convert(IEnumerable<TqPathwayAssessment> assesments, ResolutionContext context)
        {
            TqPathwayResult tqPathwayResult = Convert(assesments);
            string result = tqPathwayResult?.TlLookup?.Value?.ToString();

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }
    }
}