using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlSpecialismBuilder
    {
        public Domain.Models.TlSpecialism Build() => new Domain.Models.TlSpecialism
        {
            Name = "Surveying and design for construction and the built environment",
            LarId = "10123456",
            TlPathway = new TlPathwayBuilder().Build(),
            //TqProviders = TODO:
            TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
            TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };
        
        public IList<Domain.Models.TlSpecialism> BuildList() => new List<Domain.Models.TlSpecialism>
        {
            new Domain.Models.TlSpecialism
            {
                Name = "Surveying and design for construction and the built environment",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Civil Engineering",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Building services design",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Hazardous materials analysis and surveying",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Early years education and childcare",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Assisting teaching",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Supporting and mentoring students in further and higher education",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlSpecialism
            {
                Name = "Digital Production, Design and Development",
                LarId = "10123456",
                TlPathway = new TlPathwayBuilder().Build(),
                //TqProviders = TODO:
                TlPathwaySpecialismMars = new TlPathwaySpecialismMarBuilder().BuildList(),
                TlPathwaySpecialismCombinations = new TlPathwaySpecialismCombinationBuilder().BuildList(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
        };

        public IList<Domain.Models.TlSpecialism> BuildList(EnumAwardingOrganisation awardingOrganisation, Domain.Models.TlPathway tlPathway,  bool addDefaultPathway = true)
        {
            var pathway = (addDefaultPathway && tlPathway == null) ? new TlPathwayBuilder().Build(awardingOrganisation) : tlPathway;
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
                        LarId = "10123456",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Building services design",
                        LarId = "10123456",
                        TlPathway = pathway,
                        CreatedBy = Constants.CreatedByUser,
                        CreatedOn = Constants.CreatedOn,
                        ModifiedBy = Constants.ModifiedByUser,
                        ModifiedOn = Constants.ModifiedOn
                    },
                    new Domain.Models.TlSpecialism
                    {
                        Name = "Hazardous materials analysis and surveying",
                        LarId = "10123456",
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
