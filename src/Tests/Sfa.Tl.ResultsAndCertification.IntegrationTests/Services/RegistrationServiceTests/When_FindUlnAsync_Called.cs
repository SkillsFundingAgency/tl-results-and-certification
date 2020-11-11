using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_FindUlnAsync_Called : RegistrationServiceBaseTest
    {
        public override void Given()
        {
            // Seed Tlevel data for pearson
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationData(1111111111);

            // Seed Tlevel data for ncfe
            SeedTestData(EnumAwardingOrganisation.Ncfe, true);
            SeedRegistrationData(1111111112);
            
            CreateMapper();

            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, RegistrationMapper, RegistrationRepositoryLogger);
        }

        public override Task When() 
        {
            return Task.CompletedTask;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long ukprn, long uln, FindUlnResponse expectedResponse)
        {
            var actualResult = RegistrationService.FindUlnAsync(ukprn, uln).Result;

            if (actualResult == null)
            {
                expectedResponse.Should().BeNull();
                return;
            }

            var expecedStatus = expectedResponse.IsRegisteredWithOtherAo ? RegistrationPathwayStatus.NotSpecified : expectedResponse.Status;
            
            actualResult.Uln.Should().Be(expectedResponse.Uln);
            actualResult.Status.Should().Be(expecedStatus);
            actualResult.IsRegisteredWithOtherAo.Should().Be(expectedResponse.IsRegisteredWithOtherAo);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // // Uln not found
                    new object[] { 10011881, 1234567890, null },
                    
                    // IsActive
                    new object[] { 10011881, 1111111111,
                        new FindUlnResponse { Uln = 1111111111, RegistrationProfileId = 1, Status = RegistrationPathwayStatus.Active, IsRegisteredWithOtherAo = false } },
                    
                    // IsRegisteredWithOtherAo
                    new object[] { 10011881, 1111111112,
                        new FindUlnResponse { Uln = 1111111112, RegistrationProfileId = 0, Status = RegistrationPathwayStatus.Active, IsRegisteredWithOtherAo = true } },
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, TlAwardingOrganisation, pathways);

            TqProviders = new List<TqProvider>();
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();

            var tlProvider = TlProviders.FirstOrDefault();
            foreach (var tqAwardingOrganisation in tqAwardingOrganisations)
            {
                TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider));
            }

            DbContext.SaveChangesAsync();
        }

        private void SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProviders.First());

            DbContext.SaveChangesAsync();
        }
    }
}
