using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers.Converter.IndustryPlacement
{
    public abstract class IndustryPlacementStatusConverterBase
    {
        protected IndustryPlacementStatus Convert(IEnumerable<Domain.Models.IndustryPlacement> sourceMember)
        {
            if (sourceMember.IsNullOrEmpty())
            {
                return IndustryPlacementStatus.NotSpecified;
            }

            Domain.Models.IndustryPlacement industryPlacement = sourceMember.First();
            return industryPlacement.Status;
        }
    }
}