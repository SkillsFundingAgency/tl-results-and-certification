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
    public class When_GetRegistrationDetailsByProfileId_IsCalled : RegistrationServiceBaseTest
    {
        public override void Given()
        {
            // Seed Tlevel data for pearson
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationData(1111111111);

            CreateMapper();

            // Depencies
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);

            // Create TestClass instance
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger, TqRegistrationSpecialismRepository);

        }

        public override Task When() 
        {
            return Task.CompletedTask;
        }

        [Theory(Skip = "Ravi to pickup")]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long aoUkprn, int profileId, RegistrationDetails expectedResponse)
        {
            var actualResult = RegistrationService.GetRegistrationDetailsByProfileIdAsync(aoUkprn, profileId).Result;

            if (actualResult == null)
            {
                expectedResponse.Should().BeNull();
                return;
            }

            actualResult.Uln.Should().Be(expectedResponse.Uln);
            actualResult.Name.Should().Be(expectedResponse.Name);
            actualResult.DateofBirth.Should().Be(expectedResponse.DateofBirth);
            actualResult.ProviderDisplayName.Should().Be(expectedResponse.ProviderDisplayName);
            actualResult.PathwayDisplayName.Should().Be(expectedResponse.PathwayDisplayName);
            actualResult.SpecialismsDisplayName.Should().BeEquivalentTo(expectedResponse.SpecialismsDisplayName);
            actualResult.AcademicYear.Should().Be(expectedResponse.AcademicYear);
            actualResult.Status.Should().Be(expectedResponse.Status);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == 1111111111);
                var tlProvider = new TlProviderBuilder().Build();
                var tlPathway = new TlPathwayBuilder().Build(EnumAwardingOrganisation.Pearson, new TlRouteBuilder().Build(EnumAwardingOrganisation.Pearson));
                var tlSpecialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, tlPathway);
                var tqRegistrationPathway = new TqRegistrationPathwayBuilder().Build();
                return new[]
                {
                    // // Uln not found
                    new object[] { 10011881, 100, null },
                    
                    // IsActive
                    new object[] { 10011881, 1,
                        new RegistrationDetails
                        {
                            Uln = profile.UniqueLearnerNumber,
                            Name = $"{profile.Firstname} {profile.Lastname}",
                            DateofBirth = profile.DateofBirth,
                            ProviderDisplayName = $"{tlProvider.Name} ({tlProvider.UkPrn})",
                            PathwayDisplayName = $"{tlPathway.Name} ({tlPathway.LarId})",
                            SpecialismsDisplayName = tlSpecialisms.OrderBy(s => s.Name).Select(s => $"{s.Name} ({s.LarId})"),
                            AcademicYear = tqRegistrationPathway.AcademicYear,
                            Status = RegistrationPathwayStatus.Active
                        }
                    }                    
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, TlProvider);
            DbContext.SaveChangesAsync();
        }

        private void SeedRegistrationData(long uln)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProvider);
            
            foreach (var specialism in Specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
            }
            DbContext.SaveChangesAsync();
        }
    }
}
