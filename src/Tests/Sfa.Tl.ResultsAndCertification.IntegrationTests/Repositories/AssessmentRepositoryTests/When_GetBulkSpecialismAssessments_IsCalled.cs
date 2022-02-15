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
    public class When_GetBulkSpecialismAssessments_IsCalled : AssessmentRepositoryBaseTest
    {        
        private List<long> _ulns;
        private long _ulnForInactivePathways;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private IList<TqSpecialismAssessment> _result;        

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

                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), seedSpecialismAssessmentsAsActive));
                }
            }

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);
            SeedSpecialismResultsData(GetSpecialismResultsDataToProcess(_specialismAssessments));
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            _result = await AssessmentRepository.GetBulkSpecialismAssessmentsAsync(_specialismAssessments);
        }

        [Fact]
        public async Task Then_Expected_SpecialismAssessments_Are_Returned()
        {
            // when
            await WhenAsync();

            // then
            var expectedSpecialismAssessments = _specialismAssessments.Where(p => p.IsOptedin && p.EndDate == null).ToList();

            _result.Should().NotBeNull();
            _result.Count.Should().Be(expectedSpecialismAssessments.Count);

            foreach (var expectedSpecialismAssessment in expectedSpecialismAssessments)
            {
                var actualSpecialismAssessment = _result.FirstOrDefault(p => p.Id == expectedSpecialismAssessment.Id);

                expectedSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(actualSpecialismAssessment.TqRegistrationSpecialismId);
                expectedSpecialismAssessment.AssessmentSeriesId.Should().Be(actualSpecialismAssessment.AssessmentSeriesId);
                expectedSpecialismAssessment.IsOptedin.Should().Be(actualSpecialismAssessment.IsOptedin);
                expectedSpecialismAssessment.IsBulkUpload.Should().Be(actualSpecialismAssessment.IsBulkUpload);
                expectedSpecialismAssessment.StartDate.ToShortDateString().Should().Be(actualSpecialismAssessment.StartDate.ToShortDateString());
                expectedSpecialismAssessment.EndDate.Should().BeNull();
                expectedSpecialismAssessment.CreatedBy.Should().Be(actualSpecialismAssessment.CreatedBy);

                var actualSpecialismResult = actualSpecialismAssessment.TqSpecialismResults.FirstOrDefault();
                var expectedSpecialismResult = expectedSpecialismAssessment.TqSpecialismResults.FirstOrDefault();

                expectedSpecialismResult.Should().BeEquivalentTo(actualSpecialismResult);
            }
        }
    }
}
