using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class When_TransformResultsModel_IsCalled : ResultServiceBaseTest
    {
        private (IList<TqPathwayResult>, IList<TqSpecialismResult>) _result;
        private IList<ResultRecordResponse> _resultRecords;
        private string _performedBy = "System";

        public override void Given()
        {
            CreateMapper();
            SeedTestData(EnumAwardingOrganisation.Pearson);

            // Dependencies 
            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            SpecialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultRepository = new GenericRepository<TqSpecialismResult>(SpecialismResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            ResultServiceLogger = new Logger<ResultService>(new NullLoggerFactory());

            // Service
            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, SpecialismResultRepository, ResultMapper, ResultServiceLogger);

            _resultRecords = new ResultsBuilder().BuildValidList();
        }

        public override Task When()
        {
            _result = ResultService.TransformResultsModel(_resultRecords, _performedBy);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            //await WhenAsync();
            _result.Should().NotBeNull();

            var pathwayResults = _result.Item1;
            var actualSpecialismResults = _result.Item2;

            var expectedPathwayResultsCount = _resultRecords.Count(a => a.TqPathwayAssessmentId != null);
            var expectedSpecialismResultCount = _resultRecords.Sum(sr => sr.SpecialismResults.Count());

            pathwayResults.Should().NotBeNull();
            actualSpecialismResults.Should().NotBeNull();

            pathwayResults.Count.Should().Be(expectedPathwayResultsCount);
            actualSpecialismResults.Count.Should().Be(expectedSpecialismResultCount);

            foreach (var expectedResult in _resultRecords)
            {
                // Pathway Result
                var actualPathwayResult = pathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == expectedResult.TqPathwayAssessmentId);

                if (expectedResult.TqPathwayAssessmentId == null)
                {
                    actualPathwayResult.Should().BeNull();
                }
                else
                {
                    actualPathwayResult.Should().NotBeNull();
                    actualPathwayResult.TqPathwayAssessmentId.Should().Be(expectedResult.TqPathwayAssessmentId);
                    actualPathwayResult.TlLookupId.Should().Be(expectedResult.PathwayComponentGradeLookupId);
                    actualPathwayResult.IsOptedin.Should().BeTrue();
                    actualPathwayResult.IsBulkUpload.Should().BeTrue();
                }


                // Assessment Results
                foreach (var expectedSpecialismResult in expectedResult.SpecialismResults)
                {
                    var actualSpecialismResult = actualSpecialismResults.FirstOrDefault(x => x.TqSpecialismAssessmentId == expectedSpecialismResult.Key);

                    actualSpecialismResult.Should().NotBeNull();
                    actualSpecialismResult.TqSpecialismAssessmentId.Should().Be(expectedSpecialismResult.Key);
                    actualSpecialismResult.TlLookupId.Should().Be(expectedSpecialismResult.Value ?? 0);
                    actualSpecialismResult.IsOptedin.Should().BeTrue();
                    actualSpecialismResult.IsBulkUpload.Should().BeTrue();
                }
            }
        }
    }
}
