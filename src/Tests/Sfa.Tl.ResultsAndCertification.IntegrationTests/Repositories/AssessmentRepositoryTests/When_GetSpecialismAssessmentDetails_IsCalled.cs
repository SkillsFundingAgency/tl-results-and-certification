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
    public class When_GetSpecialismAssessmentDetails_IsCalled : AssessmentRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private IList<TqSpecialismAssessment> _result;

        public override void Given()
        {
            AoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> 
            { 
                { 1111111111, RegistrationPathwayStatus.Active }, 
                { 1111111112, RegistrationPathwayStatus.Active }, 
                { 1111111113, RegistrationPathwayStatus.Withdrawn } 
            };

            /// Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            // Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);
            }

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

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

            _result = await AssessmentRepository.GetSpecialismAssessmentDetailsAsync(aoUkprn, new List<int> { assessmentId });
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, int assessmentId, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, assessmentId);

            if (expectedResponse == false)
            {
                _result.Any().Should().BeFalse();
                return;
            }

            // Expected result
            var expectedAssessment = _specialismAssessments.FirstOrDefault(x => x.Id == assessmentId);
            expectedAssessment.Should().NotBeNull();

            var expectedSpecialism = expectedAssessment.TqRegistrationSpecialism;            
            expectedSpecialism.Should().NotBeNull();

            // Actual result
            var actualAssessment = _result.FirstOrDefault();
            var actualSpecialism = actualAssessment.TqRegistrationSpecialism;

            // Assert Registration Pathway
            actualSpecialism.TqRegistrationPathway.TqRegistrationProfileId.Should().Be(expectedSpecialism.TqRegistrationPathway.TqRegistrationProfileId);
            actualSpecialism.TqRegistrationPathway.TqProviderId.Should().Be(expectedSpecialism.TqRegistrationPathway.TqProviderId);
            actualSpecialism.TqRegistrationPathway.AcademicYear.Should().Be(expectedSpecialism.TqRegistrationPathway.AcademicYear);
            actualSpecialism.TqRegistrationPathway.StartDate.Should().Be(expectedSpecialism.TqRegistrationPathway.StartDate);
            actualSpecialism.TqRegistrationPathway.EndDate.Should().Be(expectedSpecialism.TqRegistrationPathway.EndDate);
            actualSpecialism.TqRegistrationPathway.Status.Should().Be(expectedSpecialism.TqRegistrationPathway.Status);

            AssertSpecialismAssessment(actualAssessment, expectedAssessment);
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
