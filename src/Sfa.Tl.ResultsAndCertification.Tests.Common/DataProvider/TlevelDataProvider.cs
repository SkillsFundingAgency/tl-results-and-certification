using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class TlevelDataProvider
    {
        #region TlAwarding Organisation

        public static TlAwardingOrganisation CreateTlAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlAwardingOrganisation = new TlAwardingOrganisationBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlAwardingOrganisation);
            }
            return tlAwardingOrganisation;
        }

        public static TlAwardingOrganisation CreateTlAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, TlAwardingOrganisation tlAwardingOrganisation, bool addToDbContext = true)
        {
            if (tlAwardingOrganisation == null)
            {
                tlAwardingOrganisation = new TlAwardingOrganisationBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlAwardingOrganisation);
            }
            return tlAwardingOrganisation;
        }

        public static TlAwardingOrganisation CreateTlAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, long ukprn, string name, string displayName, bool addToDbContext = true)
        {

            var tlAwardingOrganisation = new TlAwardingOrganisation
            {
                UkPrn = ukprn,
                Name = name,
                DisplayName = displayName
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlAwardingOrganisation);
            }
            return tlAwardingOrganisation;
        }

        #endregion
        
        #region TlRoute

        public static TlRoute CreateTlRoute(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlRoute = new TlRouteBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlRoute);
            }
            return tlRoute;
        }

        public static TlRoute CreateTlRoute(ResultsAndCertificationDbContext _dbContext, TlRoute tlRoute, bool addToDbContext = true)
        {
            if (addToDbContext && tlRoute != null)
            {
                _dbContext.Add(tlRoute);
            }
            return tlRoute;
        }

        public static TlRoute CreateTlRoute(ResultsAndCertificationDbContext _dbContext, string routeName, bool addToDbContext = true)
        {
            var route = new TlRoute
            {
                Name = routeName
            };

            if (addToDbContext)
            {
                _dbContext.Add(route);
            }
            return route;
        }

        #endregion

        #region TlPathway

        public static TlPathway CreateTlPathway(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlPathway = new TlPathwayBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlPathway);
            }
            return tlPathway;
        }

        public static TlPathway CreateTlPathway(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, bool addToDbContext = true)
        {
            if (tlPathway == null)
            {
                tlPathway = new TlPathwayBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlPathway);
            }
            return tlPathway;
        }

        public static TlPathway CreateTlPathway(ResultsAndCertificationDbContext _dbContext, TlRoute tlRoute, string larId, string pathwayName, bool addToDbContext = true)
        {
            if (tlRoute == null)
            {
                tlRoute = new TlRouteBuilder().Build();
            }

            var tlPathway = new TlPathway
            {
                TlRouteId = tlRoute.Id,
                LarId = larId,
                Name = pathwayName,
                TlRoute = tlRoute
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlPathway);
            }
            return tlPathway;
        }
        
        #endregion

        #region TlSpecialism

        public static TlSpecialism CreateTlSpecialism(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlSpecialism = new TlSpecialismBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlSpecialism);
            }
            return tlSpecialism;
        }

        public static TlSpecialism CreateTlSpecialism(ResultsAndCertificationDbContext _dbContext, TlSpecialism tlSpecialism, bool addToDbContext = true)
        {
            if(tlSpecialism == null) {
                tlSpecialism = new TlSpecialismBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlSpecialism);
            }
            return tlSpecialism;
        }

        public static TlSpecialism CreateTlSpecialism(ResultsAndCertificationDbContext _dbContext, TlPathway tlPathway, string larId, string name, bool addToDbContext = true)
        {
            if (tlPathway == null)
            {
                tlPathway = new TlPathwayBuilder().Build();
            }

            var tlSpecialism = new TlSpecialism
            {
                LarId = larId,
                TlPathwayId = tlPathway.Id,
                Name = name,
                TlPathway = tlPathway
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlSpecialism);
            }
            return tlSpecialism;
        }

        #endregion

        #region TqAwardingOrganisation

        public static TqAwardingOrganisation CreateTqAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tqAwardingOrganisation = new TqAwardingOrganisationBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tqAwardingOrganisation);
            }
            return tqAwardingOrganisation;
        }

        public static TqAwardingOrganisation CreateTqAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, TqAwardingOrganisation tqAwardingOrganisation, bool addToDbContext = true)
        {
            if (tqAwardingOrganisation == null)
            {
                tqAwardingOrganisation = new TqAwardingOrganisationBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tqAwardingOrganisation);
            }
            return tqAwardingOrganisation;
        }

        public static TqAwardingOrganisation CreateTqAwardingOrganisation(ResultsAndCertificationDbContext _dbContext, TlRoute tlRoute, TlPathway tlPathway, TlAwardingOrganisation tlAwardingOrganisation, TlevelReviewStatus tLevelReviewStatus = TlevelReviewStatus.AwaitingConfirmation, bool addToDbContext = true)
        {

            var tqAwardingOrganisation = new TqAwardingOrganisation
            {
                TlAwardingOrganisatonId = tlAwardingOrganisation.Id,
                TlRouteId = tlRoute.Id,
                TlPathwayId = tlPathway.Id,
                TlAwardingOrganisaton = tlAwardingOrganisation,
                TlRoute = tlRoute,
                TlPathway = tlPathway,
                ReviewStatus = (int)tLevelReviewStatus
            };
            
            if (addToDbContext)
            {
                _dbContext.Add(tqAwardingOrganisation);
            }
            return tqAwardingOrganisation;
        }

        #endregion
    }
}
