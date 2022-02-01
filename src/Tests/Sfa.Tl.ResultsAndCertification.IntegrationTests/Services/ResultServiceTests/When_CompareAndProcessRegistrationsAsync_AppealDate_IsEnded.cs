using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class When_CompareAndProcessRegistrationsAsync_AppealDate_IsEnded : ResultServiceBaseTest
    {
        private ResultProcessResponse _result;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayResult> _inputPathwayResultsData;
        private IList<BulkProcessValidationError> _expectedValidationErrors;
        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active } };

        public override void Given()
        {
            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            foreach (var registration in _registrations)
                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList()));
            var pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);

            // Results seed
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var assessment in pathwayAssessments)
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }));
            SeedPathwayResultsData(tqPathwayResultsSeedData);

            EnsureAppealDateIsEnded(tqPathwayAssessmentsSeedData);
            EnsureInputResultIsChanged(pathwayAssessments, tqPathwayResultsSeedData);
            SetupExpectedValidationErrors();

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
            CreateMapper();

            // Service
            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, SpecialismResultRepository, ResultMapper, ResultServiceLogger);
        }

        public async override Task When()
        {
            _result = await ResultService.CompareAndProcessResultsAsync(_inputPathwayResultsData);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.ValidationErrors.Should().NotBeNullOrEmpty();
            _result.ValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            _result.ValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }

        private void EnsureAppealDateIsEnded(List<TqPathwayAssessment> tqPathwayAssessmentsSeedData)
        {
            var assesmentSeries = DbContext.AssessmentSeries.FirstOrDefault(x => x.Id == tqPathwayAssessmentsSeedData.FirstOrDefault().AssessmentSeriesId);
            assesmentSeries.AppealEndDate = DateTime.Today.AddDays(-1);
            DbContext.SaveChanges();
        }
        private void EnsureInputResultIsChanged(List<TqPathwayAssessment> pathwayAssessments, List<TqPathwayResult> tqPathwayResultsSeedData)
        {
            var index = tqPathwayResultsSeedData.FirstOrDefault().TlLookupId + 1;
            var pathwayResult = new TqPathwayResultBuilder().Build(pathwayAssessments.FirstOrDefault(), PathwayComponentGrades[index]);
            _inputPathwayResultsData = new List<TqPathwayResult> { TqPathwayResultDataProvider.CreateTqPathwayResult(DbContext, pathwayResult) };
        }

        private void SetupExpectedValidationErrors()
        {
            _expectedValidationErrors = new List<BulkProcessValidationError>
            {
                new BulkProcessValidationError
                {
                    Uln = "1111111111",
                    ErrorMessage = ValidationMessages.ResultIsInFinal
                }
            };
        }
    }
}
