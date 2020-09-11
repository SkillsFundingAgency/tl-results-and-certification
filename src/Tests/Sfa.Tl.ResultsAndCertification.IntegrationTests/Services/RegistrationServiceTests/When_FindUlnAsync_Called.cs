using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
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

            // Depencies
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            
            // Create TestClass instance
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger);            
        }

        public override void When() { }

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
            
            actualResult.Uln.Should().Be(expectedResponse.Uln);
            actualResult.IsActive.Should().Be(expectedResponse.IsActive);
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
                        new FindUlnResponse { Uln = 1111111111, RegistrationProfileId = 1, IsActive = true, IsRegisteredWithOtherAo = false } },
                    
                    // IsRegisteredWithOtherAo
                    new object[] { 10011881, 1111111112,
                        new FindUlnResponse { Uln = 1111111112, RegistrationProfileId = 0, IsActive = false, IsRegisteredWithOtherAo = true } },
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
