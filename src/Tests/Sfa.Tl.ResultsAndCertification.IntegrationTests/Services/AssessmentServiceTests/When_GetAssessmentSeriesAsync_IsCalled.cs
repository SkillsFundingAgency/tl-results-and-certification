using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_GetAssessmentSeriesAsync_IsCalled : AssessmentServiceBaseTest
    {
        private IList<AssessmentSeriesDetails> _actualResult;
        public override void Given()
        {
            // Create mapper
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync()
        {
            if (_actualResult != null)
                return;

            _actualResult = await AssessmentService.GetAssessmentSeriesAsync();
        }

        [Fact]
        public async Task Then_Expected_Results_Are_Returned()
        {
            await WhenAsync();

            _actualResult.Should().NotBeNullOrEmpty();
            _actualResult.Count().Should().Be(AssessmentSeries.Count);
            var actualResponse = _actualResult.ToList();

            for (var i = 0; i < _actualResult.Count(); i++)
            {
                actualResponse[i].Id.Should().Be(AssessmentSeries[i].Id);
                actualResponse[i].ComponentType.Should().Be(AssessmentSeries[i].ComponentType);
                actualResponse[i].Name.Should().Be(AssessmentSeries[i].Name);

                actualResponse[i].Description.Should().Be(AssessmentSeries[i].Description);
                actualResponse[i].Year.Should().Be(AssessmentSeries[i].Year);
                actualResponse[i].StartDate.Should().Be(AssessmentSeries[i].StartDate);
                actualResponse[i].EndDate.Should().Be(AssessmentSeries[i].EndDate);
                actualResponse[i].AppealEndDate.Should().Be(AssessmentSeries[i].AppealEndDate);
            }
        }
    }
}
