using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public class IndustryPlacementStatusConverter : IndustryPlacementStatusConverterBase, IIndustryPlacementStatusConverter
    {
        public IndustryPlacementStatus Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            return Convert(sourceMember);
        }
    }
}