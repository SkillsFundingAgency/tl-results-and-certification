using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.PathwayResult
{
    public class PathwayResultConverter : PathwayResultConverterBase, IPathwayResultConverter
    {
        public TqPathwayResult Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            return Convert(sourceMember);

        }
    }
}