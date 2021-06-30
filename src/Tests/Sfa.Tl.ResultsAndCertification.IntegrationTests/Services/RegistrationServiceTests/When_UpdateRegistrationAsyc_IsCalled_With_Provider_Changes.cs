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
    public class When_UpdateRegistrationAsyc_IsCalled_With_Provider_Changes : RegistrationServiceBaseTest
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
            _tqRegistrationProfile = SeedRegistrationData(_uln, true);

            var pathwayAssessments = SeedPathwayAssessmentsData(GetPathwayAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.ToList(), isBulkUpload: false));

            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            foreach (var assessment in pathwayAssessments)
            {
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isBulkUpload: false));
            }

            SeedPathwayResultsData(tqPathwayResultsSeedData);

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

            var newProvider = TlProviders.Last();
            _updateRegistrationRequest = new ManageRegistration
            {
                ProfileId = _profileId,
                Uln = _uln,
                FirstName = "John",
                LastName = "Smith",
                DateOfBirth = DateTime.UtcNow.AddYears(-20),
                AoUkprn = TlAwardingOrganisation.UkPrn,
                ProviderUkprn = newProvider.UkPrn,
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

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(bool hasProviderChanged, bool expectedResult)
        {
            _updateRegistrationRequest.HasProviderChanged = hasProviderChanged;

            await WhenAsync();
            _result.Should().Be(expectedResult);

            if (expectedResult)
            {
                var expectedRegistrationProfile = _tqRegistrationProfile;
                var actualRegistrationProfile = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _tqRegistrationProfile.UniqueLearnerNumber)
                                                                                                                           .Include(x => x.TqRegistrationPathways)
                                                                                                                               .ThenInclude(x => x.TqRegistrationSpecialisms)
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
                actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Active).ToList().Count.Should().Be(1);
                actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Transferred).ToList().Count.Should().Be(1);

                var expectedPathwayBeforeTransfer = expectedRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Transferred).ToList();
                var expectedPathwayAfterTransfer = expectedRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == Common.Enum.RegistrationPathwayStatus.Active).ToList();

                // Assert Transferred Pathway
                var actualTransferredPathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => expectedPathwayBeforeTransfer.Any(y => y.TqProviderId == x.TqProviderId));
                var expectedTransferredPathway = expectedPathwayBeforeTransfer.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
                AssertRegistrationPathway(actualTransferredPathway, expectedTransferredPathway);

                // Assert Active Pathway
                var activePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => expectedPathwayAfterTransfer.Any(y => y.TqProviderId == x.TqProviderId));
                var expectedActivePathway = expectedPathwayAfterTransfer.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId));
                AssertRegistrationPathway(activePathway, expectedActivePathway);

                // Assert Active PathwayAssessment
                var actualActiveAssessment = activePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
                var expectedActiveAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
                AssertPathwayAssessment(actualActiveAssessment, expectedActiveAssessment);

                // Assert Transferred PathwayAssessment
                var actualTransferredAssessment = actualTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
                var expectedTransferredAssessment = expectedTransferredPathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
                AssertPathwayAssessment(actualTransferredAssessment, expectedTransferredAssessment);

                // Assert Active PathwayResult
                var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
                var expectedActiveResult = expectedActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
                AssertPathwayResults(actualActiveResult, expectedActiveResult);

                // Assert Transferred PathwayResult
                var actualTransferredResult = actualTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
                var expectedTransferredResult = expectedTransferredAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
                AssertPathwayResults(actualTransferredResult, expectedTransferredResult);

                // Assert IndustryPlacement Data
                var actualActiveIndustryPlacement = activePathway.IndustryPlacements.FirstOrDefault();
                var expectedPreviousIndustryPlacement = expectedTransferredPathway.IndustryPlacements.FirstOrDefault();

                actualActiveIndustryPlacement.Status.Should().Be(expectedPreviousIndustryPlacement.Status);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {                  
                    // Below is for Provier Changed
                    new object[] { true, true},

                    // Below is for Provier not changed
                    new object[] { false, false }
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
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }

        private TqRegistrationProfile SeedRegistrationData(long uln, bool seedIndustryPlacement = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, TqProviders.First());

            if (seedIndustryPlacement)
            {
                var industryPlacement = IndustryPlacementProvider.CreateIndustryPlacement(DbContext, new IndustryPlacement { Status = IndustryPlacementStatus.Completed, CreatedBy = "Test User" });
                tqRegistrationPathway.IndustryPlacements.Add(industryPlacement);
            }

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
