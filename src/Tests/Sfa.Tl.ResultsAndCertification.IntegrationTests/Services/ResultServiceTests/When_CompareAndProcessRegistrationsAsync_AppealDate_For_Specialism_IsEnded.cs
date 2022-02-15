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
    public class When_CompareAndProcessRegistrationsAsync_AppealDate_For_Specialism_IsEnded : ResultServiceBaseTest
    {
        private ResultProcessResponse _result;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayResult> _inputPathwayResultsData;
        private List<TqSpecialismResult> _inputSpecialismResultsData;
        private IList<BulkProcessValidationError> _expectedValidationErrors;
        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new() { { 1111111111, RegistrationPathwayStatus.Active } };

        public override void Given()
        {
            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Specialism Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations)
                tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList()));
            var specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            // Specialism Results seed
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();
            foreach (var assessment in specialismAssessments)
                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }));
            SeedSpecialismResultsData(tqSpecialismResultsSeedData);

            EnsureAppealDateForIsEnded(tqSpecialismAssessmentsSeedData);
            EnsureInputResultForIsChanged(specialismAssessments, tqSpecialismResultsSeedData);

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
            _result = await ResultService.CompareAndProcessResultsAsync(_inputPathwayResultsData, _inputSpecialismResultsData);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.ValidationErrors.Should().NotBeNullOrEmpty();
            _result.ValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            _result.ValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }

        private void EnsureAppealDateForIsEnded(List<TqSpecialismAssessment> tqSpecialismAssessmentsSeedData)
        {
            var assesmentSeries = DbContext.AssessmentSeries.FirstOrDefault(x => x.Id == tqSpecialismAssessmentsSeedData.FirstOrDefault().AssessmentSeriesId);
            assesmentSeries.AppealEndDate = DateTime.Today.AddDays(-1);
            DbContext.SaveChanges();
        }

        private void EnsureInputResultForIsChanged(List<TqSpecialismAssessment> specialismAssessments, List<TqSpecialismResult> tqSpecialismResultsSeedData)
        {
            var specialismResult = new TqSpecialismResultBuilder().Build(specialismAssessments.FirstOrDefault(), SpecialismComponentGrades[1]);
            _inputSpecialismResultsData = new List<TqSpecialismResult> { TqSpecialismResultDataProvider.CreateTqSpecialismResult(DbContext, specialismResult) };
            _inputPathwayResultsData = new List<TqPathwayResult>();
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