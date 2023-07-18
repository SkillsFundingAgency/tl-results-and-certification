using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class PathwayResultStringConverter : PathwayResultConverter, IValueConverter<TqRegistrationPathway, string>
    {
        public new string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            TqPathwayResult result = base.Convert(sourceMember, context);
            return result == null ? string.Empty : result.ToString();
        }
    }
}