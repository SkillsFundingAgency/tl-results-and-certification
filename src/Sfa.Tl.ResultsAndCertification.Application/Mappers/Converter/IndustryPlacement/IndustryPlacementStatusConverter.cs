using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public class IndustryPlacementStatusConverter : IndustryPlacementStatusConverterBase, IIndustryPlacementStatusConverter
    {
        public IndustryPlacementStatus Convert(IEnumerable<Domain.Models.IndustryPlacement> sourceMember, ResolutionContext context)
        {
            return Convert(sourceMember);
        }
    }
}