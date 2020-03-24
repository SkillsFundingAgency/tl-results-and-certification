using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider
{
    public class ProviderDataProvider
    {
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
    }
}
