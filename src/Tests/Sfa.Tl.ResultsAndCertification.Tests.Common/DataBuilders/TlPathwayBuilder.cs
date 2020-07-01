using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class TlPathwayBuilder
    {
        public TlPathway Build(EnumAwardingOrganisation awardingOrganisation, TlRoute tlRoute = null)
        {
            if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                return new TlPathway
                {
                    TlevelTitle = "T Level in Design, Surveying and Planning for Construction",
                    Name = "Design, Surveying and Planning",
                    LarId = "10123456",
                    TlRoute = tlRoute ?? new TlRouteBuilder().Build(awardingOrganisation),
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                };
            }
            else if (awardingOrganisation == EnumAwardingOrganisation.Ncfe)
            {
                return new TlPathway
                {
                    TlevelTitle = "T Level in Education and Childcare",
                    Name = "Education",
                    LarId = "10123457",
                    TlRoute = tlRoute ?? new TlRouteBuilder().Build(awardingOrganisation),
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

        public IList<TlPathway> BuildList(EnumAwardingOrganisation awardingOrganisation, IList<TlRoute> tlRoutes = null)
        {
            var results = new List<TlPathway>();
            var routes = tlRoutes ?? new TlRouteBuilder().BuildList(awardingOrganisation);
            if (awardingOrganisation == EnumAwardingOrganisation.Pearson)
            {
                foreach(var route in routes)
                {
                    if(route.Name == EnumExtensions.GetDisplayName(EnumTlRoute.Construction))
                    {
                        results.Add(new TlPathway
                        {
                            TlevelTitle = "T Level in Design, Surveying and Planning for Construction",
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
                        results.Add(new TlPathway
                        {
                            TlevelTitle = "T Level in Digital Production, Design and Development",
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
                        results.Add(new TlPathway
                        {
                            TlevelTitle = "T Level in Education and Childcare",
                            Name = "Education",
                            LarId = "10123457",
                            TlRoute = route,
                            CreatedBy = Constants.CreatedByUser,
                            CreatedOn = Constants.CreatedOn,
                            ModifiedBy = Constants.ModifiedByUser,
                            ModifiedOn = Constants.ModifiedOn
                        });
                    }
                    else if (route.Name == EnumExtensions.GetDisplayName(EnumTlRoute.Digital))
                    {
                        results.Add(new TlPathway
                        {
                            TlevelTitle = "T Level in Digital Support and Services",
                            Name = "Digital Support Services",
                            LarId = "10623456",
                            TlRoute = route,
                            CreatedBy = Constants.CreatedByUser,
                            CreatedOn = Constants.CreatedOn,
                            ModifiedBy = Constants.ModifiedByUser,
                            ModifiedOn = Constants.ModifiedOn
                        });

                        results.Add(new TlPathway
                        {
                            TlevelTitle = "T Level in Digital Business Services",
                            Name = "Digital Business Services",
                            LarId = "10723456",
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
    }
}
