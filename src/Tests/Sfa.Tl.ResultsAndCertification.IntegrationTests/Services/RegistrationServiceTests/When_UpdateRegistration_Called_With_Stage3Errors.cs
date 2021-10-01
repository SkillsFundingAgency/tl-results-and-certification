using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
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
    public class When_UpdateRegistration_Called_With_Stage3Errors : RegistrationServiceBaseTest
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
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);

            _updateRegistrationRequest = new ManageRegistration
            {
                ProfileId = _profileId,
                Uln = _uln,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                AoUkprn = TlAwardingOrganisation.UkPrn,
                ProviderUkprn = TlProviders.First().UkPrn,
                CoreCode = Pathway.LarId,
                SpecialismCodes = new List<string>(),
                PerformedBy = "Test User",
                HasProviderChanged = false
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await RegistrationService.UpdateRegistrationAsync(_updateRegistrationRequest);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, string coreCode, List<string> specialismCodes)
        {
            _updateRegistrationRequest.ProviderUkprn = providerUkprn;
            _updateRegistrationRequest.CoreCode = coreCode;
            _updateRegistrationRequest.SpecialismCodes = specialismCodes;

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

            var combinations = new TlPathwaySpecialismCombinationBuilder().BuildList();
            TlPathwaySpecialismCombinations = new List<TlPathwaySpecialismCombination>();
            foreach (var (specialism, index) in Specialisms.Take(combinations.Count).Select((value, i) => (value, i)))
            {
                combinations[index].TlPathwayId = Pathway.Id;
                combinations[index].TlPathway = Pathway;
                combinations[index].TlSpecialismId = specialism.Id;
                combinations[index].TlSpecialism = specialism;
                TlPathwaySpecialismCombinations.AddRange(TlevelDataProvider.CreateTlPathwaySpecialismCombinationsList(DbContext, combinations));
            }

            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }

        private TqRegistrationProfile SeedRegistrationData(long uln)
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
            return profile;
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(RegistrationMapper).Assembly);
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<ManageRegistration, TqRegistrationProfile>(new DateTimeProvider()) :
                                null);
            });
            RegistrationMapper = new Mapper(mapperConfig);
        }
    }
}
