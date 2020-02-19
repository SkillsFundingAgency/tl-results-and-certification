using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwayBuilder
    {
        public Domain.Models.TlPathway Build() => new Domain.Models.TlPathway
        {
            Name = "Design, Surveying and Planning",
            LarId = "10123456",
            TlRoute = new TlRouteBuilder().Build(),
            CreatedBy = Constants.CreatedByUser,
            CreatedOn = Constants.CreatedOn,
            ModifiedBy = Constants.ModifiedByUser,
            ModifiedOn = Constants.ModifiedOn
        };

        public Domain.Models.TlPathway Build(EnumAwardingOrganisation awardingOrganisation, bool addDefaultRoute = true)
        {
            if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                return new Domain.Models.TlPathway
                {
                    Name = "Design, Surveying and Planning",
                    LarId = "10123456",
                    TlRoute = addDefaultRoute ? new TlRouteBuilder().Build() : null,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                };
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                return new Domain.Models.TlPathway
                {
                    Name = "Education",
                    LarId = "10123457",
                    TlRoute = addDefaultRoute ? new TlRouteBuilder().Build() : null,
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

        public IList<Domain.Models.TlPathway> BuildList(EnumAwardingOrganisation awardingOrganisation)
        {
            var results = new List<Domain.Models.TlPathway>();
            var routes = new TlRouteBuilder().BuildList(awardingOrganisation);
            if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                foreach(var route in routes)
                {
                    if(route.Name == EnumExtensions.GetDisplayName(EnumTlRoute.Construction))
                    {
                        results.Add(new Domain.Models.TlPathway
                        {
                            Name = "Design, Surveying and Planning",
                            LarId = "10123456",
                            TlRoute = route,
                            CreatedBy = Constants.CreatedByUser,
                            CreatedOn = Constants.CreatedOn,
                            ModifiedBy = Constants.ModifiedByUser,
                            ModifiedOn = Constants.ModifiedOn
                        });
                    }
                    else if (route.Name == EnumExtensions.GetDisplayName(EnumTlRoute.Digital))
                    {
                        results.Add(new Domain.Models.TlPathway
                        {
                            Name = "Digital Production, Design and Development",
                            LarId = "10123468",
                            TlRoute = route,
                            CreatedBy = Constants.CreatedByUser,
                            CreatedOn = Constants.CreatedOn,
                            ModifiedBy = Constants.ModifiedByUser,
                            ModifiedOn = Constants.ModifiedOn
                        });
                    }
                }
                return results;
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                foreach (var route in routes)
                {
                    if (route.Name == EnumExtensions.GetDisplayName(EnumTlRoute.EducationAndChildcare))
                    {
                        results.Add(new Domain.Models.TlPathway
                        {
                            Name = "Education",
                            LarId = "10123457",
                            TlRoute = route,
                            CreatedBy = Constants.CreatedByUser,
                            CreatedOn = Constants.CreatedOn,
                            ModifiedBy = Constants.ModifiedByUser,
                            ModifiedOn = Constants.ModifiedOn
                        });
                    }
                }
                return results;
            }
            else
            {
                return null;
            }           
        }

        public IList<Domain.Models.TlPathway> BuildList() => new List<Domain.Models.TlPathway>
        {
            new Domain.Models.TlPathway
            {
                Name = "Design, Surveying and Planning",
                LarId = "10123456",
                TlRoute = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathway
            {
                Name = "Education",
                LarId = "10123456",
                TlRoute = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            },
            new Domain.Models.TlPathway
            {
                Name = "Digital Production, Design and Development",
                LarId = "10123456",
                TlRoute = new TlRouteBuilder().Build(),
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            }
        };
    }
}
