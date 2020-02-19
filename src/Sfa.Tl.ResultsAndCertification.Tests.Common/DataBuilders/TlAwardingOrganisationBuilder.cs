using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlAwardingOrganisationBuilder
    {
        public Domain.Models.TlAwardingOrganisation Build() => new Domain.Models.TlAwardingOrganisation
        {
            UkPrn = 10011881,
            DisplayName = "Pearson",
            Name = "Pearson",
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public Domain.Models.TlAwardingOrganisation Build(EnumAwardingOrganisation awardingOrganisation)
        {
            if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                return new Domain.Models.TlAwardingOrganisation
                {
                    UkPrn = 10011881,
                    DisplayName = "Pearson",
                    Name = "Pearson",
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                };
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                return new Domain.Models.TlAwardingOrganisation
                {
                    UkPrn = 10009696,
                    DisplayName = "Ncfe",
                    Name = "Ncfe",
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                };
            }
            else
            {
                return null;
            }
        }

        public IList<Domain.Models.TlAwardingOrganisation> BuildList() => new List<Domain.Models.TlAwardingOrganisation>
        {
            new Domain.Models.TlAwardingOrganisation
            {
                UkPrn = 10009696,
                DisplayName = "Ncfe",
                Name = "Ncfe",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlAwardingOrganisation
            {
                UkPrn = 10011881,
                DisplayName = "Pearson",
                Name = "Pearson",
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
