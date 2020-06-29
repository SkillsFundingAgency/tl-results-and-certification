using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationPathwayBuilder
    {
        public Domain.Models.TqRegistrationPathway Build() => new Domain.Models.TqRegistrationPathway
        {
            TqRegistrationProfileId = 1111111111,
            TqProviderId = 1,
            AcademicYear = 2020,
            RegistrationDate = "07/05/2020".ToDateTime(),
            StartDate = DateTime.UtcNow,
            Status = ResultsAndCertification.Common.Enum.RegistrationPathwayStatus.Active,
            IsBulkUpload = true,
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public IList<Domain.Models.TqRegistrationPathway> BuildList() => new List<Domain.Models.TqRegistrationPathway>
        {
            new Domain.Models.TqRegistrationPathway
            {
                TqRegistrationProfileId = 1111111111,
                TqProviderId = 1,
                AcademicYear = 2020,
                RegistrationDate = "07/05/2020".ToDateTime(),
                StartDate = DateTime.UtcNow,
                Status = ResultsAndCertification.Common.Enum.RegistrationPathwayStatus.Active,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationPathway
            {
                TqRegistrationProfileId = 1111111112,
                TqProviderId = 1,
                AcademicYear = 2020,
                RegistrationDate = "10/06/2020".ToDateTime(),
                StartDate = DateTime.UtcNow,
                Status = ResultsAndCertification.Common.Enum.RegistrationPathwayStatus.Active,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TqRegistrationPathway
            {
                TqRegistrationProfileId = 1111111113,
                TqProviderId = 1,
                AcademicYear = 2020,
                RegistrationDate = "11/06/2020".ToDateTime(),
                StartDate = DateTime.UtcNow,
                Status = ResultsAndCertification.Common.Enum.RegistrationPathwayStatus.Active,
                IsBulkUpload = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
