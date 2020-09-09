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
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_UpdateRegistrationAsyc_IsCalled : RegistrationServiceBaseTest
    {
        private bool _result;
        private ManageRegistration _updateRegistrationRequest;
        private int _profileId;
        private long _uln;

        public override void Given()
        {
            // Seed Tlevel data for pearson
            _uln = 1111111111;
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationData(_uln);

            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, RegistrationMapper, RegistrationRepositoryLogger);

            var newProvider = TlProviders.Last();
            _updateRegistrationRequest = new ManageRegistration
            {
                ProfileId = _profileId,
                Uln = _uln,
                AoUkprn = TlAwardingOrganisation.UkPrn,
                ProviderUkprn = newProvider.UkPrn,
                CoreCode = Pathway.LarId,
                SpecialismCodes = Specialisms.Select(s => s.LarId),
                PerformedBy = "Test User",
                HasProviderChanged = false
            };            
        }

        public override void When() {}

        public async Task WhenAsync()
        {            
            _result = await RegistrationService.UpdateRegistrationAsync(_updateRegistrationRequest);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Result(bool hasProviderChanged, bool expectedResult)
        {
            _updateRegistrationRequest.HasProviderChanged = hasProviderChanged;
            await WhenAsync();
            _result.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { true, true },
                    new object[] { false, false},
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

            TqProviders = new List<TqProvider>();
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();

            foreach (var tlProvider in TlProviders)
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

            foreach (var specialism in Specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
            }

            DbContext.SaveChangesAsync();

            _profileId = tqRegistrationProfile.Id;
        }
    }
}