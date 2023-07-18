using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public class IndustryPlacementStatusStringConverter : IndustryPlacementStatusConverter, IValueConverter<TqRegistrationPathway, string>
    {
        public new string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            IndustryPlacementStatus industryPlacementStatus = Convert(sourceMember);
            return industryPlacementStatus.ToString();
        }
    }
}