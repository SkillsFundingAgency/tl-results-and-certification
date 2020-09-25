using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationSpecialismBuilder
    {
        public TqRegistrationSpecialism Build(TqRegistrationPathway tqRegistrationPathway = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            return new TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = tqRegistrationPathway.Id,
                TqRegistrationPathway = tqRegistrationPathway,
                TlSpecialismId = 1,
                StartDate = DateTime.UtcNow,
                IsOptedin = true,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<TqRegistrationSpecialism> BuildList(TqRegistrationPathway tqRegistrationPathway = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            var tqRegistrationSpecialisms = new List<TqRegistrationSpecialism> 
            {
                new TqRegistrationSpecialism
                {
                    TqRegistrationPathwayId = tqRegistrationPathway.Id,
                    TqRegistrationPathway = tqRegistrationPathway,
                    TlSpecialismId = 1,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new TqRegistrationSpecialism
                {
                    TqRegistrationPathwayId = tqRegistrationPathway.Id,
                    TqRegistrationPathway = tqRegistrationPathway,
                    TlSpecialismId = 2,
                    StartDate = DateTime.UtcNow,
                    IsOptedin = true,
                    IsBulkUpload = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return tqRegistrationSpecialisms;
        }
    }
}
