using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.CommonRepositoryTests
{
    public abstract class CommonRepositoryBaseTest : BaseTest<TqProvider>
    {
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected IList<TlProvider> TlProviders;
        protected IList<AcademicYear> AcademicYears;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected ICommonRepository CommonRepository;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation, bool isAoActive, bool seedTqAo, List<(long ukprn, bool isProviderActive, bool seedTqProvider)> providerTestCriteria)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);            

            TlAwardingOrganisation.IsActive = isAoActive;

            if(seedTqAo)
                TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);


            if(providerTestCriteria != null)
            {
                TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();
                foreach (var (ukprn, isProviderActive, seedTqProvider) in providerTestCriteria)
                {                    
                    var tlProvider = TlProviders.FirstOrDefault(p => p.UkPrn == ukprn);

                    if (tlProvider != null)
                    {
                        tlProvider.IsActive = isProviderActive;

                        if (seedTqProvider)
                            ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, tlProvider);
                    }
                }
            }          

            DbContext.SaveChanges();
        }

        public void SeedAcademicYears()
        {
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}
