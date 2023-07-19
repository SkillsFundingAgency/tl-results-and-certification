using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public class IndustryPlacementStatusStringConverter : IndustryPlacementStatusConverterBase, IValueConverter<IEnumerable<Domain.Models.IndustryPlacement>, string>
    {
        public string Convert(IEnumerable<Domain.Models.IndustryPlacement> sourceMember, ResolutionContext context)
        {
            IndustryPlacementStatus industryPlacementStatus = Convert(sourceMember);
            return industryPlacementStatus.ToString();
        }
    }
}