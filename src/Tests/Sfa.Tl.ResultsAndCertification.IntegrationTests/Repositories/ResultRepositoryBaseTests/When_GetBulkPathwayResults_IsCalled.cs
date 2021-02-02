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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.ResultRepositoryBaseTests
{
    public class When_GetBulkPathwayResults_IsCalled : ResultRepositoryBaseTest
    {
        private List<long> _ulns;
        private long _ulnForInactivePathways;
        private List<TqPathwayResult> _pathwayResults;
        private IList<TqPathwayResult> _result;

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

            var pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            _pathwayResults = SeedPathwayResultsData(GetPathwayResultsDataToProcess(pathwayAssessments));

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await ResultRepository.GetBulkPathwayResultsAsync(_pathwayResults);
        }

        [Fact]
        public async Task Then_Expected_PathwayResults_Are_Returned()
        {
            // when
            await WhenAsync();

            // then
            var expectedPathwayResults = _pathwayResults.Where(p => p.IsOptedin && p.EndDate == null).ToList();

            _result.Should().NotBeNull();
            _result.Count.Should().Be(expectedPathwayResults.Count);

            foreach (var expectedPathwayResult in expectedPathwayResults)
            {
                var actualPathwayResult = _result.FirstOrDefault(p => p.Id == expectedPathwayResult.Id);

                actualPathwayResult.TqPathwayAssessmentId.Should().Be(expectedPathwayResult.TqPathwayAssessmentId);
                actualPathwayResult.TlLookupId.Should().Be(expectedPathwayResult.TlLookupId);
                actualPathwayResult.IsOptedin.Should().Be(expectedPathwayResult.IsOptedin);
                actualPathwayResult.IsBulkUpload.Should().Be(expectedPathwayResult.IsBulkUpload);
                actualPathwayResult.StartDate.ToShortDateString().Should().Be(expectedPathwayResult.StartDate.ToShortDateString());
                actualPathwayResult.EndDate.Should().BeNull();
                actualPathwayResult.CreatedBy.Should().Be(expectedPathwayResult.CreatedBy);
            }
        }
    }
}
