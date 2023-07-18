using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult
{
    public class PathwayResultStringConverter : PathwayResultConverterBase, IValueConverter<TqRegistrationPathway, string>
    {
        public string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            TqPathwayResult tqPathwayResult = Convert(sourceMember);
            string result = tqPathwayResult?.TlLookup?.Value?.ToString();

            return string.IsNullOrEmpty(result) ? string.Empty : result;
        }
    }
}