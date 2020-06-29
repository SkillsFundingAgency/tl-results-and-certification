using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationSpecialismBuilder
    {
        public Domain.Models.TqRegistrationSpecialism Build() => new Domain.Models.TqRegistrationSpecialism
        {
            TqRegistrationPathwayId = 1,
            TlSpecialismId = 1,
            StartDate = DateTime.UtcNow,
            Status = RegistrationSpecialismStatus.Active,
            IsBulkUpload = true,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqRegistrationSpecialism> BuildList() => new List<Domain.Models.TqRegistrationSpecialism>
        {
            new Domain.Models.TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = 1,
                TlSpecialismId = 1,
                StartDate = DateTime.UtcNow,
                Status = RegistrationSpecialismStatus.Active,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationSpecialism
            {
                TqRegistrationPathwayId = 1,
                TlSpecialismId = 2,
                StartDate = DateTime.UtcNow,
                Status = RegistrationSpecialismStatus.Active,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
