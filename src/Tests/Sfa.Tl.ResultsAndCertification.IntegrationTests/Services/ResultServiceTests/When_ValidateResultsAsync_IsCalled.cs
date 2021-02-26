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
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class When_ValidateResultsAsync_IsCalled : ResultServiceBaseTest
    {
        private long _aoUkprn;
        private IList<ResultCsvRecordResponse> _stage2Response;
        private Task<IList<ResultRecordResponse>> _stage3Result;        
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Withdrawn }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Active }, { 1111111114, RegistrationPathwayStatus.Active } };

        public override void Given()
        {
            CreateMapper();

            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);           
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111 && x.UniqueLearnerNumber != 1111111114))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            DbContext.SaveChanges();

            // Dependencies 
            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            ResultServiceLogger = new Logger<ResultService>(new NullLoggerFactory());

            // Service
            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, ResultMapper, ResultServiceLogger);

            // setup input parameter
            SetupInputParameter();
        }

        public override Task When()
        {
            _stage3Result = ResultService.ValidateResultsAsync(_aoUkprn, _stage2Response);
            return Task.CompletedTask;
        }

        [Fact]
        public void Expected_Results_Are_Returned()
        {
            _stage3Result.Result.Should().NotBeNull();
            _stage3Result.Result.Count().Should().Be(_stage2Response.Count());

            // 1. ULN not recognised with AO
            var rowIndex = 0;
            var actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            var expectedErrors = new List<string> { ValidationMessages.UlnNotRegistered };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 2. ULN is withdrawn
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.CannotAddResultToWithdrawnRegistration };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 3. Invalid - Core Code
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.InvalidCoreComponentCode };            
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 4. Invalid - Assessment Series 
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.InvalidCoreAssessmentSeriesEntry };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 5. Invalid - Core Grade
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.InvalidCoreComponentGrade };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 6. Invalid - No assessment entry is currently active
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.NoCoreAssessmentEntryCurrentlyActive };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 7. Invalid - Assessment entry mapping error
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string> { ValidationMessages.AssessmentSeriesDoesNotMatchTheSeriesOnTheRegistration };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 8. Invalid - Combination of Core code, Core assessment series and core grade error
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(3);
            expectedErrors = new List<string>
            {
                ValidationMessages.InvalidCoreComponentCode,
                ValidationMessages.InvalidCoreAssessmentSeriesEntry,
                ValidationMessages.InvalidCoreComponentGrade
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 9. Invalid - Combination of Core code, Core assessment series, core grade and No assessment entry is currently active error
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(4);
            expectedErrors = new List<string>
            {
                ValidationMessages.InvalidCoreComponentCode,
                ValidationMessages.InvalidCoreAssessmentSeriesEntry,
                ValidationMessages.InvalidCoreComponentGrade,
                ValidationMessages.NoCoreAssessmentEntryCurrentlyActive
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 10. Valid Row.
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeTrue();
            actualResult.ValidationErrors.Count.Should().Be(0);
                        
            var expectedTqPathwayAssessmentId = _pathwayAssessments.FirstOrDefault(pa => pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == _stage2Response[rowIndex].Uln && pa.IsOptedin && pa.EndDate == null)?.Id;
            var expectedTlLookupId = TlLookup.FirstOrDefault(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(_stage2Response[rowIndex].CoreGrade, StringComparison.InvariantCultureIgnoreCase))?.Id;
            
            actualResult.TqPathwayAssessmentId.Should().Be(expectedTqPathwayAssessmentId);
            actualResult.PathwayComponentGradeLookupId.Should().Be(expectedTlLookupId);
        }

        private void TestValidatonErrors(IList<BulkProcessValidationError> actualErrors, List<string> expectedErrors, int rowIndex)
        {
            actualErrors.Count.Should().Be(expectedErrors.Count);
            var idx = 0;
            foreach (var error in actualErrors)
            {
                error.RowNum.Should().Be(_stage2Response[rowIndex].RowNum.ToString());
                error.Uln.Should().Be(_stage2Response[rowIndex].Uln.ToString());
                error.ErrorMessage.Should().Be(expectedErrors[idx++]);
            }
        }

        private void SetupInputParameter()
        {
            // Param 1
            _aoUkprn = 10011881;

            // Param 2;
            _stage2Response = new List<ResultCsvRecordResponse>
            {
                // 1. ULN not recognised with AO
                new ResultCsvRecordResponse { RowNum = 1, Uln = 9999999999 },

                // 2. ULN is withdrawn
                new ResultCsvRecordResponse { RowNum = 2, Uln = 1111111111 },

                // 3. Invalid - Core Code
                new ResultCsvRecordResponse { RowNum = 3, Uln = 1111111112, CoreCode = "Invalid", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A" },

                // 4. Invalid - Assessment Series
                new ResultCsvRecordResponse { RowNum = 4, Uln = 1111111112, CoreCode = "10123456", CoreAssessmentSeries = "xyz 2021", CoreGrade = "A" },

                // 5. Invalid - Core Grade
                new ResultCsvRecordResponse { RowNum = 5, Uln = 1111111112, CoreCode = "10123456", CoreAssessmentSeries = "Summer 2021", CoreGrade = "Z" },

                // 6. Invalid - No assessment entry is currently active
                new ResultCsvRecordResponse { RowNum = 6, Uln = 1111111114, CoreCode = "10123456", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A" },

                // 7. Invalid - Assessment entry mapping error
                new ResultCsvRecordResponse { RowNum = 7, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentSeries = "Autumn 2022", CoreGrade = "A" },

                // 8. Invalid - Combination of Core code, Core assessment series and core grade error
                new ResultCsvRecordResponse { RowNum = 8, Uln = 1111111113, CoreCode = "00000000", CoreAssessmentSeries = "xyz 2022", CoreGrade = "Z" },
                
                // 9. Invalid - Combination of Core code, Core assessment series, core grade and No assessment entry is currently active error
                new ResultCsvRecordResponse { RowNum = 9, Uln = 1111111114, CoreCode = "00000000", CoreAssessmentSeries = "xyz 2022", CoreGrade = "Z" },

                // 10. Valid Row
                new ResultCsvRecordResponse { RowNum = 10, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A" }
            };
        }
    }
}
