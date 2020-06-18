using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlRouteBuilder
    {
        public Domain.Models.TlRoute Build(EnumAwardingOrganisation awardingOrganisation)
        {
            if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
               return new Domain.Models.TlRoute
                {
                    Name = "Education and Childcare",
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                };
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                return new Domain.Models.TlRoute
                {
                    Name = "Construction",
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

        public IList<Domain.Models.TlRoute> BuildList(EnumAwardingOrganisation awardingOrganisation)
        {
            if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                return new List<Domain.Models.TlRoute>
                {
                    new Domain.Models.TlRoute
                    {
                        Name = "Education and Childcare",
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    }
                };                
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                return new List<Domain.Models.TlRoute>
                {
                    new Domain.Models.TlRoute
                    {
                        Name = "Construction",
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlRoute
                    {
                        Name = "Digital",
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    }
                };                
            }
            else
            {
                return null;
            }
        }
    }
}
