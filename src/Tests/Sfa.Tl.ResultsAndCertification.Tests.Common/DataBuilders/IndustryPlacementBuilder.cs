﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IndustryPlacementBuilder
    {
        public IndustryPlacement Build(TqRegistrationPathway tqRegistrationPathway = null, IndustryPlacementStatus? status = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            status ??= IndustryPlacementStatus.Completed;

            return new IndustryPlacement
            {
                TqRegistrationPathwayId = tqRegistrationPathway.Id,
                TqRegistrationPathway = tqRegistrationPathway,
                Status = status.Value,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }
    }
}
