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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_ReRegistration_Called_With_Stage3Errors : RegistrationServiceBaseTest
    {
        private bool _result;
        private long _uln;
        private IList<TlPathway> _tlPathways;
        private TqProvider _initialRegisteredTqProvider;
        private ReregistrationRequest _reRegistrationRequest;

        public override void Given()
        {
            // Seed Tlevel data for pearson
            _uln = 1111111111;
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationData(_uln);

            CreateMapper();
            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);

            var reregisterTlProvider = TqProviders.Last().TlProvider;
            var reregisterPathway = TqProviders.Last().TqAwardingOrganisation.TlPathway;
            var reregisterPathwaySpecialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, reregisterPathway);

            SeedTlPathwaySpecialismCombinations(reregisterPathway, reregisterPathwaySpecialisms);

            _reRegistrationRequest = new ReregistrationRequest
            {
                AoUkprn = TlAwardingOrganisation.UkPrn,
                ProviderUkprn = reregisterTlProvider.UkPrn,
                AcademicYear = DateTime.UtcNow.Year,
                CoreCode = reregisterPathway.LarId,
                SpecialismCodes = TlPathwaySpecialismCombinations.Select(s => s.TlSpecialism.LarId),
                PerformedBy = "Test User"
            };            
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await RegistrationService.ReregistrationAsync(_reRegistrationRequest);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, string coreCode, List<string> specialismCodes)
        {
            _reRegistrationRequest.ProviderUkprn = providerUkprn;
            _reRegistrationRequest.CoreCode = coreCode;
            _reRegistrationRequest.SpecialismCodes = specialismCodes;

            await WhenAsync();

            _result.Should().BeFalse();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                var tlProvider = new TlProviderBuilder().Build();
                var tlPathway = new TlPathwayBuilder().Build(EnumAwardingOrganisation.Pearson);
                var tlSpecialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, tlPathway);
                return new[]
                {
                    new object[] { 10000000, tlPathway.LarId, new List<string> { tlSpecialisms.Last().LarId } }, // ProviderNotRegisteredWithAo
                    new object[] { tlProvider.UkPrn, "00000000", new List<string> { tlSpecialisms.Last().LarId } }, // CoreNotRegisteredWithProvider
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { "XYZ456125" } }, // SpecialismNotValidWithCore
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { tlSpecialisms.First().LarId } }, // SpecialismCannotBeSelectedAsSingleOption
                    new object[] { tlProvider.UkPrn, tlPathway.LarId, new List<string> { { tlSpecialisms.First().LarId }, { tlSpecialisms.Last().LarId } } }, // SpecialismIsNotValid
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            _tlPathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);

            foreach (var pathway in _tlPathways)
            {
                TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathway);
            }

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, TlAwardingOrganisation, _tlPathways);

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
            _initialRegisteredTqProvider = TqProviders.First();
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, _initialRegisteredTqProvider);

            var specialisms = new TlSpecialismBuilder().BuildList(EnumAwardingOrganisation.Pearson, _initialRegisteredTqProvider.TqAwardingOrganisation.TlPathway);

            foreach (var specialism in specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
            }

            DbContext.SaveChangesAsync();
        }

        private void SeedTlPathwaySpecialismCombinations(TlPathway tlPathway, IList<TlSpecialism> specialisms)
        {
            var combinations = new TlPathwaySpecialismCombinationBuilder().BuildList();
            TlPathwaySpecialismCombinations = new List<TlPathwaySpecialismCombination>();
            foreach (var (specialism, index) in specialisms.Take(combinations.Count).Select((value, i) => (value, i)))
            {
                combinations[index].TlPathwayId = tlPathway.Id;
                combinations[index].TlPathway = tlPathway;
                combinations[index].TlSpecialismId = specialism.Id;
                combinations[index].TlSpecialism = specialism;
                TlPathwaySpecialismCombinations.AddRange(TlevelDataProvider.CreateTlPathwaySpecialismCombinationsList(DbContext, combinations));
            }
            DbContext.SaveChangesAsync();
        }
    }
}
