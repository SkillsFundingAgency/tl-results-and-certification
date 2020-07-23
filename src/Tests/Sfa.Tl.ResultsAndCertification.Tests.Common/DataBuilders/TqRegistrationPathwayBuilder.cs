using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TqRegistrationPathwayBuilder
    {
        public TqRegistrationPathway Build(TqRegistrationProfile tqRegistrationProfile = null)
        {
            tqRegistrationProfile ??= new TqRegistrationProfileBuilder().Build();
            return new TqRegistrationPathway
            {
                TqRegistrationProfileId = tqRegistrationProfile.Id,
                TqRegistrationProfile = tqRegistrationProfile,
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
        }

        public IList<TqRegistrationPathway> BuildList(TqRegistrationProfile tqRegistrationProfile = null)
        {
            tqRegistrationProfile ??= new TqRegistrationProfileBuilder().Build();
            var tqRegistrationPathways = new List<TqRegistrationPathway> {
                new TqRegistrationPathway
                {
                    TqRegistrationProfileId = tqRegistrationProfile.Id,
                    TqRegistrationProfile = tqRegistrationProfile,
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
                new TqRegistrationPathway
                {
                    TqRegistrationProfileId = tqRegistrationProfile.Id,
                    TqRegistrationProfile = tqRegistrationProfile,
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
                new TqRegistrationPathway
                {
                    TqRegistrationProfileId = tqRegistrationProfile.Id,
                    TqRegistrationProfile = tqRegistrationProfile,
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
            return tqRegistrationPathways;
        }
    }
}
