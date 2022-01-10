using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
    public class When_RejoinRegistration_IsCalled : RegistrationServiceBaseTest
    {
        private bool _result;
        private RejoinRegistrationRequest _reJoinRegistrationRequest;
        private long _uln;
        private TqRegistrationProfile _tqRegistrationProfile;

        public override void Given()
        {
            // Seed Tlevel data for pearson
            _uln = 1111111111;
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _tqRegistrationProfile = SeedRegistrationData(_uln, true);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var pathwayAssessments = GetPathwayAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.ToList(), isBulkUpload: false);
            tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);
            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);

            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var assessment in pathwayAssessments)
            {
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isBulkUpload: false));
            }

            SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            // Specialism Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var specialismAssessments = GetSpecialismAssessmentsDataToProcess(_tqRegistrationProfile.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList());
            tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);
            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            DbContext.SaveChangesAsync();

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

            _reJoinRegistrationRequest = new RejoinRegistrationRequest
            {
                AoUkprn = TlAwardingOrganisation.UkPrn,
                PerformedBy = "Test User"
            };
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            await RegistrationService.WithdrawRegistrationAsync(new WithdrawRegistrationRequest { ProfileId = _reJoinRegistrationRequest.ProfileId, AoUkprn = _reJoinRegistrationRequest.AoUkprn });
            _result = await RegistrationService.RejoinRegistrationAsync(_reJoinRegistrationRequest);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(int profileId, bool expectedResult)
        {
            _reJoinRegistrationRequest.ProfileId = profileId;
            await WhenAsync();
            _result.Should().Be(expectedResult);

            if (expectedResult == false) 
                return;

            var expectedRegistrationProfile = _tqRegistrationProfile;
            var actualRegistrationProfile = DbContext.TqRegistrationProfile.AsNoTracking().Where(x => x.UniqueLearnerNumber == _tqRegistrationProfile.UniqueLearnerNumber)
                                                                                                                       .Include(x => x.TqRegistrationPathways)
                                                                                                                           .ThenInclude(x => x.TqRegistrationSpecialisms)
                                                                                                                                .ThenInclude(x => x.TqSpecialismAssessments)
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
            actualRegistrationProfile.TqRegistrationPathways.Where(x => x.Status == RegistrationPathwayStatus.Withdrawn).ToList().Count.Should().Be(1);

            // Assert Any Active Pathway
            actualRegistrationProfile.TqRegistrationPathways.Any(x => x.Status == RegistrationPathwayStatus.Active).Should().BeTrue();

            // Assert Active Pathway
            var actualActivePathway = actualRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => x.EndDate == null && x.Status == RegistrationPathwayStatus.Active);
            var expectedActivePathway = expectedRegistrationProfile.TqRegistrationPathways.FirstOrDefault(x => actualRegistrationProfile.TqRegistrationPathways.Any(y => y.TqProviderId == x.TqProviderId && x.Status == RegistrationPathwayStatus.Withdrawn));
            AssertRegistrationPathway(actualActivePathway, expectedActivePathway, false);

            // Assert Withdrawn PathwayAssessment
            var actualActiveAssessment = actualActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousAssessment = expectedActivePathway.TqPathwayAssessments.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayAssessment(actualActiveAssessment, expectedPreviousAssessment, isRejoin: true);

            // Assert Active PathwayResult
            var actualActiveResult = actualActiveAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate == null);
            var expectedPreviousResult = expectedPreviousAssessment.TqPathwayResults.FirstOrDefault(x => x.EndDate != null);
            AssertPathwayResults(actualActiveResult, expectedPreviousResult, isRejoin: true);

            // Assert Withdrawn SpecialismAssessment
            foreach (var activeSpecialism in actualActivePathway.TqRegistrationSpecialisms.Where(s => s.EndDate == null))
            {
                var withdrawnSpecialism = expectedActivePathway.TqRegistrationSpecialisms.FirstOrDefault(x => x.TlSpecialismId == activeSpecialism.TlSpecialismId && x.EndDate != null);
                var actualActiveSpecialismAssessment = activeSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate == null);
                var expectedPreviousSpecialismAssessment = withdrawnSpecialism.TqSpecialismAssessments.FirstOrDefault(x => x.EndDate != null);
                AssertSpecialismAssessment(actualActiveSpecialismAssessment, expectedPreviousSpecialismAssessment, isRejoin: true);
            }

            // Assert IndustryPlacement Data
            var actualActiveIndustryPlacement = actualActivePathway.IndustryPlacements.FirstOrDefault();
            var expectedPreviousIndustryPlacement = expectedActivePathway.IndustryPlacements.FirstOrDefault();

            actualActiveIndustryPlacement.Status.Should().Be(expectedPreviousIndustryPlacement.Status);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1, true },
                    new object[] { 10000000, false }
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
            tqRegistrationPathway.IsBulkUpload = false;

            if(seedIndustryPlacement)
            {
                var industryPlacement = IndustryPlacementProvider.CreateIndustryPlacement(DbContext, new IndustryPlacement { Status = IndustryPlacementStatus.Completed, CreatedBy = "Test User" });
                tqRegistrationPathway.IndustryPlacements.Add(industryPlacement);
            }

            foreach (var specialism in Specialisms)
            {
                var specialismToAdd = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, specialism);
                specialismToAdd.IsBulkUpload = false;
                tqRegistrationPathway.TqRegistrationSpecialisms.Add(specialismToAdd);
            }

            DbContext.SaveChangesAsync();
            return profile;
        }
    }
}
