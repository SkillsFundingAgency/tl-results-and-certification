using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class BulkRegistrationsTextFixture : BaseTest<TqRegistrationProfile>
    {
        protected RegistrationService RegistrationService;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected IList<TlSpecialism> Specialisms;
        protected TlProvider TlProvider;
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IProviderRepository ProviderRepository;
        public IRegistrationRepository RegistrationRepository;
        protected ILogger<ProviderRepository> ProviderRepositoryLogger;
        protected ILogger<RegistrationRepository> RegistrationRepositoryLogger;

        public RegistrationProcessResponse Result;
        public IList<TqRegistrationProfile> TqRegistrationProfilesData;
        public long Uln;

        public BulkRegistrationsTextFixture() : base(true) { }

        public override void Given()
        {
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository);
        }

        public override void When() {}

        public async Task WhenAsync()
        {
            Result = await RegistrationService.CompareAndProcessRegistrationsAsync(TqRegistrationProfilesData);
        }

        public void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathways.First());

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, tlAwardingOrganisation, pathways);
            DbContext.SaveChanges();

            TqProviders = new List<TqProvider>();            
            foreach (var tqAwardingOrganisation in tqAwardingOrganisations)
            {
                TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, TlProviders.FirstOrDefault()));
            }

            DbContext.SaveChanges();
        }

        public void SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProviders.First());
            DbContext.SaveChanges();
        }

        public List<TqRegistrationProfile> GetRegistrationsDataToProcess(List<long> ulns)
        {
            var tqRegistrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (uln, index) in ulns.Select((value, i) => (value, i)))
            {
                var tqProvider = TqProviders[index];
                var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
                var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider);

                tqRegistrationProfile.TqRegistrationPathways = new List<TqRegistrationPathway> { tqRegistrationPathway };
                tqRegistrationProfiles.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfiles;
        }
    }
}
