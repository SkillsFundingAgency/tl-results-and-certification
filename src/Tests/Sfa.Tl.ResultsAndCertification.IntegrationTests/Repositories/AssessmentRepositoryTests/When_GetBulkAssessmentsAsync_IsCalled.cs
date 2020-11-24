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
    public class When_GetBulkAssessmentsAsync_IsCalled : AssessmentRepositoryBaseTest
    {
        private long _aoUkprn;
        private Dictionary<long, RegistrationPathwayStatus> _ulns;

        private IEnumerable<TqRegistrationPathway> _result;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;

        public override void Given()
        {
            // Parameters
            _aoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            var registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent));
                
                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent));
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            DbContext.SaveChanges();
            
            // TestClass
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_result != null)
                return;

            _result = await AssessmentRepository.GetBulkAssessmentsAsync(_aoUkprn, _ulns.Keys);
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            // when
            await WhenAsync();

            _result.Should().NotBeNullOrEmpty();
            _result.Count().Should().Be(_ulns.Count);

            // Uln: 1111111111 - Registration(Active) with no assessments
            var actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == 1111111111);
            actualresult.Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.FirstOrDefault().TqSpecialismAssessments.Should().BeEmpty();

            // Uln: 1111111113 - Registration(Withdrawn), TqPathwayAssessments(Withdrawn) and TqSpecialismAssessments(Withdrawn)
            var uln = 1111111113;
            actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln);
            actualresult.Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.Should().HaveCount(1);

            // Uln: 1111111112 - Registration(Active), TqPathwayAssessments(Active + History) and TqSpecialismAssessments(Active + History)
            uln = 1111111112;
            actualresult = _result.SingleOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == uln);
            actualresult.Should().NotBeNull();
            actualresult.TqRegistrationSpecialisms.Should().HaveCount(1);
        }        
    }
}
