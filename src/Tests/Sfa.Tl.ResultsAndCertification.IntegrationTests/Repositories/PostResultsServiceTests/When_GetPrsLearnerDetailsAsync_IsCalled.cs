using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.PostResultsServiceTests
{
    public class When_GetPrsLearnerDetailsAsync_IsCalled : PostResultsServiceRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private List<long> _profilesWithAssessment;
        private List<long> _profilesWithResults;

        private PrsLearnerDetails _actualResult;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },   // First Assessment with Result and another without Result
                { 1111111112, RegistrationPathwayStatus.Active },   // One Assessment with Result
                { 1111111113, RegistrationPathwayStatus.Active },   // Assessment Only
                { 1111111114, RegistrationPathwayStatus.Withdrawn },// Assessment Only + Withdrawn
                { 1111111115, RegistrationPathwayStatus.Active },   // No Assessment
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            foreach (var uln in _ulns)
                _profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, TqProvider));

            DbContext.SaveChanges();

            // Seed Assessments And Results
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            _profilesWithAssessment = new List<long> { 1111111111, 1111111112, 1111111113, 1111111114 };
            foreach (var profile in _profiles.Where(x => _profilesWithAssessment.Contains(x.UniqueLearnerNumber)))
            {
                var isLatestActive = _ulns[profile.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(profile.TqRegistrationPathways.ToList(), isLatestActive);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Seed Pathway results
                _profilesWithResults = new List<long> { 1111111111, 1111111112, 1111111113 };
                foreach (var assessment in pathwayAssessments.Where(x => _profilesWithResults.Contains(x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber)))
                {
                    var hasHitoricData = new List<long> { 1111111112 };
                    var hasHistoricResult = hasHitoricData.Any(x => x == profile.UniqueLearnerNumber);

                    var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, seedPathwayResultsAsActive: true, hasHistoricResult);
                    tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                }
            }

            // Add additional assessment to ULN - 1111111111
            tqPathwayAssessmentsSeedData.Add(AddAssessmentFor(1111111111));
            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, true);

            // Test class.
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            var profileId = 20;
            _actualResult = await PostResultsServiceRepository.GetPrsLearnerDetailsAsync(aoUkprn, profileId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, AwardingOrganisation ao, bool hasAssessment, bool hasResult, bool isRecordFound)
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

            var expctedProvider = expectedTqAwardingOrganisation.TqProviders.FirstOrDefault().TlProvider;
            _actualResult.ProviderUkprn.Should().Be(expctedProvider.UkPrn);
            _actualResult.ProviderName.Should().Be(expctedProvider.Name);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    //new object[] { 9999999999, AwardingOrganisation.Pearson, null, null, false }, // Invalid Uln
                    //new object[] { 1111111111, AwardingOrganisation.Ncfe,  null, null, false },   // Invalid for Ncfe
                    new object[] { 1111111111, AwardingOrganisation.Pearson, true, true, true },  // First Assessment with Result and another without Result
                    new object[] { 1111111112, AwardingOrganisation.Pearson, true, true, true }, // One Assessment with Result
                    new object[] { 1111111113, AwardingOrganisation.Pearson, true, false, true }, // Assessment Only
                    //new object[] { 1111111114, AwardingOrganisation.Pearson, null, null, false }, // Invalid - because of Withdrawn
                    new object[] { 1111111115, AwardingOrganisation.Pearson, false, false, true } // No Assessment
                };
            }
        }

        private TqPathwayAssessment AddAssessmentFor(long uln)
        {
            var activePathwayAssessment = new TqPathwayAssessmentBuilder().Build(_profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln)
                .TqRegistrationPathways.FirstOrDefault(), AssessmentSeries[1]);
            return PathwayAssessmentDataProvider.CreateTqPathwayAssessment(DbContext, activePathwayAssessment);
        }
    }
}
