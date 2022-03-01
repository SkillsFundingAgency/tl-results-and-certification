using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
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
    public class When_UpdateRegistrationAsyc_IsCalled : RegistrationServiceBaseTest
    {
        private bool _result;
        private ManageRegistration _updateRegistrationRequest;
        private int _profileId;
        private long _uln;
        private TqRegistrationProfile _tqRegistrationProfile;

        public override void Given()
        {
            // Seed Tlevel data for pearson
            _uln = 1111111111;
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _tqRegistrationProfile = SeedRegistrationData(_uln, false);

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

            var newProvider = TlProviders.Last();
            _updateRegistrationRequest = new ManageRegistration
            {
                ProfileId = _profileId,
                Uln = _uln,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                AcademicYear = 2020,
                AoUkprn = TlAwardingOrganisation.UkPrn,
                ProviderUkprn = newProvider.UkPrn,
                CoreCode = Pathway.LarId,
                SpecialismCodes = TlPathwaySpecialismCombinations.Select(s => s.TlSpecialism.LarId),
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

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(bool hasProfileChanged, bool hasProviderChanged, bool hasSpecialismsChanged, bool expectedResult)
        {
            _updateRegistrationRequest.HasProfileChanged = hasProfileChanged;
            _updateRegistrationRequest.HasProviderChanged = hasProviderChanged;
            _updateRegistrationRequest.HasSpecialismsChanged = hasSpecialismsChanged;

            if(hasProviderChanged)
            {
                // Assessments seed
                var pathwayAssessments = SeedPathwayAssessmentsData(GetPathwayAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.ToList(), isBulkUpload: false));
                var pathwayResults = SeedPathwayResultsData(GetPathwayResultsDataToProcess(pathwayAssessments, isBulkUpload: false));
                
                var specialismAssessments = SeedSpecialismAssessmentsData(GetSpecialismAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList()));
                var specialismResults = SeedSpecialismResultsData(GetSpecialismResultsDataToProcess(specialismAssessments, isBulkUpload: false));
            }

            await WhenAsync();
            _result.Should().Be(expectedResult);

            if(hasProviderChanged)
            {
                AssertProviderChangedData();
            }
        }

        private void AssertProviderChangedData()
        {
            var expectedRegistrationProfile = _tqRegistrationProfile;
            var actualRegistrationProfile = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _tqRegistrationProfile.UniqueLearnerNumber)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                           .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                                                .ThenInclude(x => x.TqSpecialismAssessments)
                                                                                                                                    .ThenInclude(x => x.TqSpecialismResults)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.TqPathwayAssessments)
                                                                                                                                .ThenInclude(x => x.TqPathwayResults)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                            .ThenInclude(x => x.IndustryPlacements)
                                                                                                                       .FirstOrDefault();

            // assert registration profile data
            actualRegistrationProfile.Should().NotBeNull();
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);
            actualRegistrationProfile.Firstname.Should().Be(expectedRegistrationProfile.Firstname);
            actualRegistrationProfile.Lastname.Should().Be(expectedRegistrationProfile.Lastname);
            actualRegistrationProfile.DateofBirth.Should().Be(expectedRegistrationProfile.DateofBirth);
            actualRegistrationProfile.UniqueLearnerNumber.Should().Be(expectedRegistrationProfile.UniqueLearnerNumber);

            // Assert registration pathway data
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Active).ToList().Count.Should().Be(1);
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Transferred).ToList().Count.Should().Be(1);

            // Assert Any Active Pathway
            actualRegistrationProfile.TqRegistrationPathways.Any(x => x.Status == RegistrationPathwayStatus.Active).Should().BeTrue();

            // Assert Active Pathway
            var actualActivePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => x.EndDate == null && x.Status == RegistrationPathwayStatus.Active);
            var expectedActivePathway = expectedRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId && x.Status == RegistrationPathwayStatus.Transferred));
            AssertRegistrationPathway(actualActivePathway, expectedActivePathway, false, isTransferred: true);

            // Assert Transferred PathwayAssessment
            var actualActiveAssessment = actualActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualActiveAssessment, expectedPreviousAssessment, isTransferred: true);

            // Assert Pathway result
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            var expectedPreviousResult = expectedPreviousAssessment.TqPathwayResults.FirstOrDefault(x => x.IsOptedin && x.EndDate != null);
            AssertPathwayResults(actualActiveResult, expectedPreviousResult, isTransferred: true);

            // Assert Transferred SpecialismAssessment
            foreach (var activeSpecialism in actualActivePathway.TqRegistrationSpecialisms.Where(s => s.EndDate == null))
            {
                var transferredSpecialism = expectedActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.TlSpecialismId == activeSpecialism.TlSpecialismId && x.EndDate != null);
                var actualActiveSpecialismAssessment = activeSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate == null);
                var expectedPreviousSpecialismAssessment = transferredSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate != null);
                AssertSpecialismAssessment(actualActiveSpecialismAssessment, expectedPreviousSpecialismAssessment, isRejoin: false, isTransferred: true);

                // Assert Specialism result
                var actualActiveSpecialismResult = actualActiveSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
                var expectedPreviousSpecialismResult = expectedPreviousSpecialismAssessment.TqSpecialismResults.FirstOrDefault(x => x.IsOptedin && x.EndDate != null);
                AssertSpecialismResult(actualActiveSpecialismResult, expectedPreviousSpecialismResult, isTransferred: true);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Below is for Profile Changed
                    new object[] { true, false, false, true },
                    new object[] { false, false, false, false },

                    // Below is for Provier Changed
                    new object[] { false, true, false, true },

                    // Below is for Specialisms Changed
                    new object[] { false, false, true, true },
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

        private TqRegistrationProfile SeedRegistrationData(long uln, bool isBulkupload = true)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProviders.First());

            foreach (var specialism in Specialisms)
            {
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism));
            }

            if (!isBulkupload)
            {
                tqRegistrationPathway.IsBulkUpload = isBulkupload;
                tqRegistrationPathway.TqRegistrationSpecialisms.ToList().ForEach(s => s.IsBulkUpload = isBulkupload); 
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