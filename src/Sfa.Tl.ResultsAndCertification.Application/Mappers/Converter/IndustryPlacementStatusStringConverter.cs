using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class IndustryPlacementStatusStringConverter : IndustryPlacementStatusConverter, IValueConverter<TqRegistrationPathway, string>
    {
        public new string Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            IndustryPlacementStatus industryPlacementStatus = base.Convert(sourceMember, context);
            return industryPlacementStatus.ToString(); ;
        }
    }
}