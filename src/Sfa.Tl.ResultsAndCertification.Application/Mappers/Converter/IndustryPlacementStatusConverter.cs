using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter
{
    public class IndustryPlacementStatusConverter : IIndustryPlacementStatusConverter
    {
        public IndustryPlacementStatus Convert(TqRegistrationPathway sourceMember, ResolutionContext context)
        {
            if (sourceMember.IndustryPlacements.IsNullOrEmpty())
            {
                return IndustryPlacementStatus.NotSpecified;
            }

            IndustryPlacement industryPlacement = sourceMember.IndustryPlacements.First();
            return industryPlacement.Status;
        }
    }
}