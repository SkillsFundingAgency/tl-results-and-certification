using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_GetRegistrationAsync_Is_Called : RegistrationServiceBaseTest
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
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger);
        }

        public override void When() { }

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Expected_Response_Is_Returned(long aoUkprn, int profileId, ManageRegistration expectedResponse)
        {
            var actualResult = RegistrationService.GetRegistrationAsync(aoUkprn, profileId).Result;

            if (actualResult == null)
            {
                expectedResponse.Should().BeNull();
                return;
            }

            actualResult.Uln.Should().Be(expectedResponse.Uln);
            actualResult.FirstName.Should().Be(expectedResponse.FirstName);
            actualResult.LastName.Should().Be(expectedResponse.LastName);
            actualResult.DateOfBirth.Should().Be(expectedResponse.DateOfBirth);
            actualResult.ProviderUkprn.Should().Be(expectedResponse.ProviderUkprn);
            actualResult.CoreCode.Should().Be(expectedResponse.CoreCode);
            actualResult.AcademicYear.Should().Be(expectedResponse.AcademicYear);
            actualResult.AoUkprn.Should().Be(expectedResponse.AoUkprn);
            actualResult.CreatedBy.Should().Be(expectedResponse.CreatedBy);
            actualResult.ModifiedBy.Should().Be(expectedResponse.ModifiedBy);

            actualResult.SpecialismCodes.Count().Should().Be(expectedResponse.SpecialismCodes.Count());
            actualResult.SpecialismCodes.ToList().ForEach(spl => 
            {
                expectedResponse.SpecialismCodes.Contains(spl);
            });
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
                    // Uln not found
                    new object[] { 10011881, 100, null },
                    
                    // IsActive
                    new object[] { 10011881, 1,
                        new ManageRegistration
                        {
                            Uln = profile.UniqueLearnerNumber,
                            FirstName = profile.Firstname,
                            LastName = profile.Lastname,
                            DateOfBirth = profile.DateofBirth,
                            CoreCode = tlPathway.LarId,
                            ProviderUkprn = tlProvider.UkPrn,
                            AcademicYear = tqRegistrationPathway.AcademicYear,
                            SpecialismCodes = tlSpecialisms.Select(s => s.LarId),
                            AoUkprn = 10011881
                        }
                    },
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
