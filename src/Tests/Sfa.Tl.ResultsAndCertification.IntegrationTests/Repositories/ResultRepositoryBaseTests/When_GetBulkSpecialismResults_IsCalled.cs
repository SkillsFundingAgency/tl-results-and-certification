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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.ResultRepositoryBaseTests
{
    public class When_GetBulkSpecialismResults_IsCalled : ResultRepositoryBaseTest
    {
        private List<long> _ulns;
        private long _ulnForInactivePathways;
        private List<TqSpecialismResult> _specialismResults;
        private IList<TqSpecialismResult> _result;

        public override void Given()
        {
            _ulns = new List<long> { 1111111111, 1111111112, 1111111113 };
            _ulnForInactivePathways = 1111111113;

            // Seed Tlevel data for pearson
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            // Seed Registrations Data
            var registrations = SeedRegistrationsData(_ulns);

            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();

            foreach (var registration in registrations)
            {
                var seedSpecialismAssessmentsAsActive = registration.UniqueLearnerNumber != _ulnForInactivePathways;
                tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), seedSpecialismAssessmentsAsActive));
            }

            var specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            _specialismResults = SeedSpecialismResultsData(GetSpecialismResultsDataToProcess(specialismAssessments));

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await ResultRepository.GetBulkSpecialismResultsAsync(_specialismResults);
        }

        [Fact]
        public async Task Then_Expected_SpecialismResults_Are_Returned()
        {
            // when
            await WhenAsync();

            // then
            var expectedSpecialismResults = _specialismResults.Where(p => p.IsOptedin && p.EndDate == null).ToList();

            _result.Should().NotBeNull();
            _result.Count.Should().Be(expectedSpecialismResults.Count);

            foreach (var expectedPathwayResult in expectedSpecialismResults)
            {
                var actualSpecialismResult = _result.FirstOrDefault(p => p.Id == expectedPathwayResult.Id);

                actualSpecialismResult.TqSpecialismAssessmentId.Should().Be(expectedPathwayResult.TqSpecialismAssessmentId);
                actualSpecialismResult.TlLookupId.Should().Be(expectedPathwayResult.TlLookupId);
                actualSpecialismResult.IsOptedin.Should().Be(expectedPathwayResult.IsOptedin);
                actualSpecialismResult.IsBulkUpload.Should().Be(expectedPathwayResult.IsBulkUpload);
                actualSpecialismResult.StartDate.ToShortDateString().Should().Be(expectedPathwayResult.StartDate.ToShortDateString());
                actualSpecialismResult.EndDate.Should().BeNull();
                actualSpecialismResult.CreatedBy.Should().Be(expectedPathwayResult.CreatedBy);
            }
        }
    }
}
