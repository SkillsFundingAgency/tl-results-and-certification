using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public class When_GetPathwayAssessmentDetailsAsync_IsCalled : AssessmentRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private TqPathwayAssessment _result;

        public override void Given()
        {
            AoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

            /// Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);;
                
                // Build Pathway results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            SeedPathwayResultsData(tqPathwayResultsSeedData);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int assessmentId)
        {
            if (_result != null)
                return;

            _result = await AssessmentRepository.GetPathwayAssessmentDetailsAsync(aoUkprn, assessmentId);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, int assessmentId, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, assessmentId);

            if (_result == null)
            {
                expectedResponse.Should().BeFalse();
                return;
            }

            // Expected result
            var expectedAssessment = _pathwayAssessments.FirstOrDefault(x => x.Id == assessmentId);
            expectedAssessment.Should().NotBeNull();

            var expectedPathway = expectedAssessment.TqRegistrationPathway;            
            expectedPathway.Should().NotBeNull();

            // Actual result
            var actualAssessment = _result;
            var actualPathway = actualAssessment.TqRegistrationPathway;

            // Assert Registration Pathway
            actualPathway.TqRegistrationProfileId.Should().Be(expectedPathway.TqRegistrationProfileId);
            actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
            actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            actualPathway.StartDate.Should().Be(expectedPathway.StartDate);
            actualPathway.EndDate.Should().Be(expectedPathway.EndDate);
            actualPathway.Status.Should().Be(expectedPathway.Status);
            
            AssertPathwayAssessment(actualAssessment, expectedAssessment);

            foreach (var (actiaResult, index) in actualAssessment.TqPathwayResults.Select((actualResult, i) => (actualResult, i)))
                AssertPathwayResult(actiaResult, expectedAssessment.TqPathwayResults.ToArray()[index]);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // AssessmentId not valid
                    new object[] { 10011881, 0, false },
                    new object[] { 10011881, 300, false },

                    // AssessmentId not found for registered AoUkprn
                    new object[] { 00000000, 1, false },
                    
                    // Valid
                    new object[] { 10011881, 1, true },
                    new object[] { 10011881, 3, true }
                };
            }
        }
    }
}
