using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlDualSpecialismBuilder
    {       
        public IList<Domain.Models.TlDualSpecialism> BuildList(EnumAwardingOrganisation awardingOrganisation, Domain.Models.TlPathway tlPathway = null)
        {
            var pathway = tlPathway ?? new TlPathwayBuilder().Build(awardingOrganisation);
            var results = new List<Domain.Models.TlDualSpecialism>();
            if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                results = new List<Domain.Models.TlDualSpecialism>
                {
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Plumbing and Heating Engineering",
                        LarId = "ZTLOS030",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Heating Engineering and Ventilation",
                        LarId = "ZTLOS031",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Refrigeration Engineering and Air Conditioning Engineering",
                        LarId = "ZTLOS032",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    }
                };
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                results = new List<Domain.Models.TlDualSpecialism>
                {
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Surveying and design for construction and the built environment",
                        LarId = "10123456",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Civil Engineering",
                        LarId = "10123457",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Building services design",
                        LarId = "10123458",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlDualSpecialism
                    {
                        Name = "Hazardous materials analysis and surveying",
                        LarId = "10123459",
                        TlPathwayId = pathway.Id,
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    }
                };
            }
            return results;
        }
    }
}
