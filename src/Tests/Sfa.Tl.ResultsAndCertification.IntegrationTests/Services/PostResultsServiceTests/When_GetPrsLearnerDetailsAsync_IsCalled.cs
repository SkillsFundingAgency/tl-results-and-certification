using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_GetPrsLearnerDetailsAsync_IsCalled : PostResultsServiceServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private List<TqPathwayAssessment> _tqPathwayAssessmentsSeedData;

        private PrsLearnerDetails _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },   // Assessment + Result
                { 1111111112, RegistrationPathwayStatus.Active },   // Assessment + Result (with history)
                { 1111111113, RegistrationPathwayStatus.Withdrawn },// Assessment + Result (Withdrawn)
                { 1111111114, RegistrationPathwayStatus.Active },   // Assessmet Only 
                { 1111111115, RegistrationPathwayStatus.Active },   // No Assessment
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _profiles = SeedRegistrationsData(_ulns, TqProvider);
            DbContext.SaveChanges();

            // Seed Assessments And Results
            _tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            var profilesWithAssessment = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114 };
            foreach (var profile in _profiles.Where(x => profilesWithAssessment.Contains(x.UniqueLearnerNumber)))
            {
                var isLatestActive = _ulns[profile.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(profile.TqRegistrationPathways.ToList(), isLatestActive);
                _tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Seed Pathway results
                var profilesWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
                foreach (var assessment in pathwayAssessments.Where(x => profilesWithResults.Contains(x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)))
                {
                    var hasHitoricData = new List<long> { 1111111112 };
                    var hasHistoricResult = hasHitoricData.Any(x => x == profile.UniqueLearnerNumber);

                    var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, seedPathwayResultsAsActive: true, hasHistoricResult);
                    tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                }
            }

            SeedPathwayAssessmentsData(_tqPathwayAssessmentsSeedData, true);

            // Test class and dependencies
            CreateMapper();
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
            var pathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultsRepository = new GenericRepository<TqPathwayResult>(pathwayResultRepositoryLogger, DbContext);
            PostResultsServiceServiceLogger = new Logger<PostResultsServiceService>(new NullLoggerFactory());

            PostResultsServiceService = new PostResultsServiceService(PostResultsServiceRepository, PathwayResultsRepository, PostResultsServiceMapper, PostResultsServiceServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, long uln)
        {
            var profileId = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln).Id;
            var assessmentId = _tqPathwayAssessmentsSeedData.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln)?.Id;
            _actualResult = await PostResultsServiceRepository.GetPrsLearnerDetailsAsync(aoUkprn, profileId, assessmentId ?? 0);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, AwardingOrganisation ao, bool isRecordFound)
        {
            await WhenAsync((long)ao, uln);

            if (isRecordFound == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedProfile = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            _actualResult.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
            _actualResult.Firstname.Should().Be(expectedProfile.Firstname);
            _actualResult.Lastname.Should().Be(expectedProfile.Lastname);
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);

            var expectedPathway = expectedProfile.TqRegistrationPathways.OrderByDescending(x => x.CreatedOn).FirstOrDefault(x => x.Status == RegistrationPathwayStatus.Active);
            _actualResult.Status.Should().Be(expectedPathway.Status);

            var expectedTqAwardingOrganisation = expectedPathway.TqProvider.TqAwardingOrganisation;
            _actualResult.TlevelTitle.Should().Be(expectedTqAwardingOrganisation.TlPathway.TlevelTitle);

            var expectedTlPathway = expectedTqAwardingOrganisation.TqProviders.FirstOrDefault().TqAwardingOrganisation.TlPathway;
            _actualResult.PathwayName.Should().Be(expectedTlPathway.Name);
            _actualResult.PathwayCode.Should().Be(expectedTlPathway.LarId);

            var expctedProvider = expectedTqAwardingOrganisation.TqProviders.FirstOrDefault().TlProvider;
            _actualResult.ProviderUkprn.Should().Be(expctedProvider.UkPrn);
            _actualResult.ProviderName.Should().Be(expctedProvider.Name);

            var expectedAssessment = expectedPathway.TqPathwayAssessments.FirstOrDefault();
            _actualResult.PathwayAssessmentId.Should().Be(expectedAssessment.Id);
            _actualResult.PathwayAssessmentSeries.Should().Be(expectedAssessment.AssessmentSeries.Name);
            _actualResult.AppealEndDate.Should().Be(expectedAssessment.AssessmentSeries.AppealEndDate);

            var expectedResult = expectedAssessment.TqPathwayResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            _actualResult.PathwayResultId.Should().Be(expectedResult.Id);
            _actualResult.PathwayGrade.Should().Be(expectedResult.TlLookup.Value);
            _actualResult.PathwayGradeLastUpdatedBy.Should().Be(expectedResult.CreatedBy);
            _actualResult.PathwayGradeLastUpdatedOn.Should().Be(expectedResult.CreatedOn);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, AwardingOrganisation.Ncfe,  false },     // Invalid for Ncfe
                    new object[] { 1111111111, AwardingOrganisation.Pearson, true },   // Assessment + Result
                    new object[] { 1111111112, AwardingOrganisation.Pearson, true },  // Assessment + Result (with history)
                    new object[] { 1111111113, AwardingOrganisation.Pearson, false }, // Assessment + Result (Withdrawn)
                    new object[] { 1111111114, AwardingOrganisation.Pearson, false }, // Assessmet Only 
                    new object[] { 1111111115, AwardingOrganisation.Pearson, false } // No Assessment
                };
            }
        }
    }
}
