using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.AssessmentRepositoryTests
{
    public class When_GetBulkPathwayAssessments_IsCalled : AssessmentRepositoryBaseTest
    {
        private List<long> _ulns;
        private long _ulnForInactivePathways;        
        private List<TqPathwayAssessment> _pathwayAssessments;
        private IList<TqPathwayAssessment> _result;

        public override void Given()
        {
            _ulns = new List<long> { 1111111111, 1111111112, 1111111113 };
            _ulnForInactivePathways = 1111111113;

            // Seed Tlevel data for pearson
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            // Seed Registrations Data
            var registrations = SeedRegistrationsData(_ulns);
            
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            
            foreach (var registration in registrations)
            {
                var seedPathwayAssessmentsAsActive = registration.UniqueLearnerNumber != _ulnForInactivePathways;
                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), seedPathwayAssessmentsAsActive));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await AssessmentRepository.GetBulkPathwayAssessmentsAsync(_pathwayAssessments);
        }

        [Fact]
        public async Task Then_Expected_PathwayAssessments_Are_Returned()
        {
            // when
            await WhenAsync();

            // then
            var expectedPathwayAssessments = _pathwayAssessments.Where(p => p.IsOptedin && p.EndDate == null).ToList();

            _result.Should().NotBeNull();
            _result.Count.Should().Be(expectedPathwayAssessments.Count);

            foreach(var expectedPathwayAssessment in expectedPathwayAssessments)
            {
                var actualPathwayAssessment = _result.FirstOrDefault(p => p.Id == expectedPathwayAssessment.Id);

                actualPathwayAssessment.TqRegistrationPathwayId.Should().Be(actualPathwayAssessment.TqRegistrationPathwayId);
                actualPathwayAssessment.AssessmentSeriesId.Should().Be(actualPathwayAssessment.AssessmentSeriesId);
                actualPathwayAssessment.IsOptedin.Should().Be(actualPathwayAssessment.IsOptedin);
                actualPathwayAssessment.IsBulkUpload.Should().Be(actualPathwayAssessment.IsBulkUpload);
                actualPathwayAssessment.StartDate.ToShortDateString().Should().Be(actualPathwayAssessment.StartDate.ToShortDateString());
                actualPathwayAssessment.EndDate.Should().BeNull();
                actualPathwayAssessment.CreatedBy.Should().Be(actualPathwayAssessment.CreatedBy);
            }
        }
    }
}
