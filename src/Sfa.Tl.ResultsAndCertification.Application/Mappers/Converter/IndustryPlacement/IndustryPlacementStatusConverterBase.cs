using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public abstract class IndustryPlacementStatusConverterBase
    {
        protected IndustryPlacementStatus Convert(TqRegistrationPathway sourceMember)
        {
            if (sourceMember.IndustryPlacements.IsNullOrEmpty())
            {
                return IndustryPlacementStatus.NotSpecified;
            }

            Domain.Models.IndustryPlacement industryPlacement = sourceMember.IndustryPlacements.First();
            return industryPlacement.Status;
        }
    }
}