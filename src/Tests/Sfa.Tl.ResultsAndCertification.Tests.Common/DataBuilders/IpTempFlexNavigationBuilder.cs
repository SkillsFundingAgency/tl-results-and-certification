using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class IpTempFlexNavigationBuilder
    {
        public Domain.Models.IpTempFlexNavigation Build(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);

            return new Domain.Models.IpTempFlexNavigation
            {
                TlPathwayId = pathway.Id,
                AcademicYear = 2020,
                AskTempFlexibility = true,
                AskBlendedPlacement = true,
                IsActive = true,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<Domain.Models.IpTempFlexNavigation> BuildList(EnumAwardingOrganisation awardingOrganisation)
        {
            var pathway = new TlPathwayBuilder().Build(awardingOrganisation);

            var ipTempFlexCombinations = new List<Domain.Models.IpTempFlexNavigation>()
            {
                new Domain.Models.IpTempFlexNavigation
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    AcademicYear = 2020,
                    AskTempFlexibility = true,
                    AskBlendedPlacement = true,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpTempFlexNavigation
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    AcademicYear = 2021,
                    AskTempFlexibility = false,
                    AskBlendedPlacement = true,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new Domain.Models.IpTempFlexNavigation
                {
                    TlPathwayId = pathway.Id,
                    TlPathway = pathway,
                    AcademicYear = 2022,
                    AskTempFlexibility = true,
                    AskBlendedPlacement = false,
                    IsActive = true,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
            return ipTempFlexCombinations;
        }
    }
}
