using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class ProviderDataProvider
    {
        #region  TlProvider
        
        public static TlProvider CreateTlProvider(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlProvider = new TlProviderBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tlProvider);
            }
            return tlProvider;
        }

        public static TlProvider CreateTlProvider(ResultsAndCertificationDbContext _dbContext, TlProvider tlProvider, bool addToDbContext = true)
        {
            if (tlProvider == null)
            {
                tlProvider = new TlProviderBuilder().Build();
            }

            if (addToDbContext)
            {
                _dbContext.Add(tlProvider);
            }
            return tlProvider;
        }

        public static IEnumerable<TlProvider> CreateTlProviders(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tlProviders = new TlProviderBuilder().BuildList();
            if (addToDbContext)
            {
                _dbContext.AddRangeAsync(tlProviders);
            }
            return tlProviders;
        }


        public static TlProvider CreateTlProvider(ResultsAndCertificationDbContext _dbContext, long ukprn, string name, string displayName, bool addToDbContext = true)
        {
            var tlProvider = new TlProvider
            {
                UkPrn = ukprn,
                Name = name,
                DisplayName = displayName
            };

            if (addToDbContext)
            {
                _dbContext.Add(tlProvider);
            }
            return tlProvider;
        }

        #endregion

        #region TqProvider

        public static TqProvider CreateTqProvider(ResultsAndCertificationDbContext _dbContext, int tqAwardingOrganisationId, int tlProviderId, int pathwayId, bool addToDbContext = true)
        {
            var tqProvider = new TqProvider
            {
                TqAwardingOrganisationId = tqAwardingOrganisationId,
                TlProviderId = tlProviderId,
                TlPathwayId = pathwayId
            };

            if (addToDbContext)
            {
                _dbContext.Add(tqProvider);
            }
            return tqProvider;
        }

        public static TqProvider CreateTqProvider(ResultsAndCertificationDbContext _dbContext, bool addToDbContext = true)
        {
            var tqProvider = new TqProviderBuilder().Build();

            if (addToDbContext)
            {
                _dbContext.Add(tqProvider);
            }
            return tqProvider;
        }

        public static TqProvider CreateTqProvider(ResultsAndCertificationDbContext _dbContext, TqAwardingOrganisation tqAwardingOrganisation, TlProvider tlProvider, bool addToDbContext = true)
        {
            if (tlProvider != null && tqAwardingOrganisation != null)
            {
                var tqProvider = new TqProvider
                {
                    TlProviderId = tlProvider.Id,
                    TqAwardingOrganisationId = tqAwardingOrganisation.Id
                };

                if (addToDbContext)
                {
                    _dbContext.Add(tqProvider);
                }
                return tqProvider;
            }
            return null;
        }

        public static IList<TqProvider> CreateTqProviders(ResultsAndCertificationDbContext _dbContext, IList<TqAwardingOrganisation> tqAwardingOrganisations, TlProvider tlProvider, bool addToDbContext = true)
        {
            if (tlProvider != null && tqAwardingOrganisations != null && tqAwardingOrganisations.Count > 0)
            {
                var tqProviders = new List<TqProvider>();
                foreach(var tqAwardingOrganisation in tqAwardingOrganisations)
                {
                    tqProviders.Add(new TqProvider
                    {
                        TlProviderId = tlProvider.Id,
                        TqAwardingOrganisationId = tqAwardingOrganisation.Id,
                        TlPathwayId = tqAwardingOrganisation.TlPathwayId
                    });
                }

                if (addToDbContext)
                {
                    _dbContext.AddRange(tqProviders);
                }
                return tqProviders;
            }
            return null;
        }

        #endregion
    }
}
