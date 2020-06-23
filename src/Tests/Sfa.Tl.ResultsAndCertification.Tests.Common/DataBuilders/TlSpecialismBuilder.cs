using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlSpecialismBuilder
    {       
        public IList<Domain.Models.TlSpecialism> BuildList(EnumAwardingOrganisation awardingOrganisation, Domain.Models.TlPathway tlPathway = null)
        {
            var pathway = tlPathway ?? new TlPathwayBuilder().Build(awardingOrganisation);
            var results = new List<Domain.Models.TlSpecialism>();
            if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                results = new List<Domain.Models.TlSpecialism>
                {
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Early years education and childcare",
                        LarId = "10123456",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Assisting teaching",
                        LarId = "10123456",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Supporting and mentoring students in further and higher education",
                        LarId = "10123456",
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
                results = new List<Domain.Models.TlSpecialism>
                {
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Surveying and design for construction and the built environment",
                        LarId = "10123456",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Civil Engineering",
                        LarId = "10123457",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Building services design",
                        LarId = "10123458",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Hazardous materials analysis and surveying",
                        LarId = "10123459",
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
