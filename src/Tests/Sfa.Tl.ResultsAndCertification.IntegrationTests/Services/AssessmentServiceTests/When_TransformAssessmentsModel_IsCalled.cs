using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkAssessments;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_TransformAssessmentsModel_IsCalled : AssessmentServiceBaseTest
    {
        private (IList<TqPathwayAssessment>, IList<TqSpecialismAssessment>) _result;
        private IList<AssessmentRecordResponse> _assessmentRecords;
        private string _performedBy = "System";

        public override void Given()
        {
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson);
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
            _assessmentRecords = new AssessmentsBuilder().BuildValidList();
        }

        public override Task When()
        {
            _result = AssessmentService.TransformAssessmentsModel(_assessmentRecords, _performedBy);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            //await WhenAsync();
            _result.Should().NotBeNull();

            var pathwayAssessments = _result.Item1;
            var specialismAssessments = _result.Item2;

            var expectedPathwayAssessmentsCount = _assessmentRecords.Count(a => a.TqRegistrationPathwayId != null);
            var expectedSpecialismAssessmentsCount = _assessmentRecords.Count(a => a.TqRegistrationSpecialismIds != null);

            pathwayAssessments.Should().NotBeNull();
            specialismAssessments.Should().NotBeNull();

            pathwayAssessments.Count.Should().Be(expectedPathwayAssessmentsCount);
            specialismAssessments.Count.Should().Be(expectedSpecialismAssessmentsCount);

            foreach (var expectedAssessment in _assessmentRecords)
            {
                // Pathway Assessments
                var actualPathwayAssessment = pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathwayId == expectedAssessment.TqRegistrationPathwayId);

                if (expectedAssessment.TqRegistrationPathwayId == null)
                {
                    actualPathwayAssessment.Should().BeNull();
                }
                else
                {
                    actualPathwayAssessment.Should().NotBeNull();
                    actualPathwayAssessment.TqRegistrationPathwayId.Should().Be(expectedAssessment.TqRegistrationPathwayId);
                    actualPathwayAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.PathwayAssessmentSeriesId);
                }

                // Specialism Assessments
                var actualSpecialismAssessment = specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedAssessment.TqRegistrationSpecialismIds);

                if (expectedAssessment.TqRegistrationSpecialismIds == null)
                {
                    actualSpecialismAssessment.Should().BeNull();
                }
                else
                {
                    actualSpecialismAssessment.Should().NotBeNull();
                    actualSpecialismAssessment.TqRegistrationSpecialismId.Should().Be(expectedAssessment.TqRegistrationSpecialismIds);
                    actualSpecialismAssessment.AssessmentSeriesId.Should().Be(expectedAssessment.SpecialismAssessmentSeriesId);
                }
            }
        }
    }
}
