using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_ValidateAssessmentsAsync_IsCalled : AssessmentServiceBaseTest
    {
        private IList<AssessmentCsvRecordResponse> _stage2Response;
        private Task<IList<AssessmentRecordResponse>> _stage3Result;
        private long _aoUkprn;

        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus> 
        { 
            { 1111111111, RegistrationPathwayStatus.Withdrawn },
            { 1111111112, RegistrationPathwayStatus.Active },
            { 1111111113, RegistrationPathwayStatus.Active },
            { 1111111114, RegistrationPathwayStatus.Active },
            { 1111111115, RegistrationPathwayStatus.Active },
            { 1111111116, RegistrationPathwayStatus.Active }
        };

        public override void Given()
        {
            CreateMapper();

            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);


            var registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Second cohort Ulns for Specialisms assessment entry to be taken in 1st year
            var secondCohortUlns = new List<long> { 1111111116 };
            RegisterUlnForNextCohort(registrations, secondCohortUlns, 2021);

            var coupletRegistrations = new List<long> { 1111111114, 1111111115 };
            var coupletAssessments = new List<long> { 1111111115, 1111111116 };

            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var coupletReg in coupletRegistrations)
            {
                var tqRegistrationPathway =  registrations.FirstOrDefault(x => x.UniqueLearnerNumber == coupletReg).TqRegistrationPathways.FirstOrDefault();
                tqRegistrationPathway.TqRegistrationSpecialisms = null;
                RegistrationsDataProvider.CreateTqRegistrationSpecialisms(DbContext, tqRegistrationPathway);

                // Add Previous Specialism assessment
                if (coupletAssessments.Contains(tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber))
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(tqRegistrationPathway.TqRegistrationSpecialisms.ToList(), isHistorical: true));
                }
            }
            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            // Dependencies 
            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            // TestClass
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);

            // setup input parameter
            SetupInputParameter();
        }

        public override Task When()
        {
            _stage3Result = AssessmentService.ValidateAssessmentsAsync(_aoUkprn, _stage2Response);
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
            expectedErrors = new List<string> { ValidationMessages.CannotAddAssessmentToWithdrawnRegistration };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 3. Invalid - Core Code, Specialism Code, Core assessment entry and Specialism assessment entry
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(4);
            expectedErrors = new List<string>
            {
                ValidationMessages.InvalidCoreCode,
                ValidationMessages.InvalidSpecialismCode,
                ValidationMessages.InvalidCoreAssessmentEntry,
                ValidationMessages.InvalidSpecialismAssessmentEntry
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 4. Core and Specialism series are not open. 
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(2);
            expectedErrors = new List<string>
            {
                ValidationMessages.InvalidNextCoreAssessmentEntry,
                ValidationMessages.InvalidNextSpecialismAssessmentEntry
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 5. Couplets registered - first entry, both entries are expected but has only one.   
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string>
            {
                ValidationMessages.SpecialismCodeMustBePair
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);

            // 6. Couplets registered - Contains invalid code   
            rowIndex++;
            actualResult = _stage3Result.Result[rowIndex];
            actualResult.IsValid.Should().BeFalse();
            actualResult.ValidationErrors.Count.Should().Be(1);
            expectedErrors = new List<string>
            {
                ValidationMessages.InvalidSpecialismCode
            };
            TestValidatonErrors(actualResult.ValidationErrors, expectedErrors, rowIndex);


            // 7. Valid Rows.
            for (int i = ++rowIndex; i < _stage2Response.ToList().Count; i++)
            {
                actualResult = _stage3Result.Result[i];
                actualResult.IsValid.Should().BeTrue();
                actualResult.ValidationErrors.Count.Should().Be(0);
            }
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
            _stage2Response = new List<AssessmentCsvRecordResponse>
            {
                // 1. ULN not recognised with AO
                new AssessmentCsvRecordResponse { RowNum = 1, Uln = 9999999999 },

                // 2. ULN is withdrawn
                new AssessmentCsvRecordResponse { RowNum = 2, Uln = 1111111111 },

                // 3. Invalid - Core Code, Specialism Code, Core assessment entry and Specialism assessment entry
                new AssessmentCsvRecordResponse { RowNum = 3, Uln = 1111111112, CoreCode = "Invalid", CoreAssessmentEntry = "Invalid", SpecialismCodes = new List<string> { "Invalid" }, SpecialismAssessmentEntry = "Invalid" },

                // 4. Core and Specialism assessment series are not open. 
                new AssessmentCsvRecordResponse { RowNum = 4, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentEntry = "Summer 2023" },

                // 5. Couplets registered - first entry, both codes must present.   
                new AssessmentCsvRecordResponse { RowNum = 5, Uln = 1111111114, SpecialismCodes = new List<string> {"10123456" }, SpecialismAssessmentEntry = "SUMMER 2022" },
                
                // 6. Couplets registered - Contains invalid code   
                new AssessmentCsvRecordResponse { RowNum = 6, Uln = 1111111114, SpecialismCodes = new List<string> {"10123456,99999999" }, SpecialismAssessmentEntry = "SUMMER 2022" },
                
                // 7. All below are VALID Rows
                new AssessmentCsvRecordResponse { RowNum = 7, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentEntry = "Summer 2021" },
                new AssessmentCsvRecordResponse { RowNum = 8, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentEntry = "SUMMER 2021" },
                new AssessmentCsvRecordResponse { RowNum = 9, Uln = 1111111113, SpecialismCodes = new List<string> {"10123456" }, SpecialismAssessmentEntry = "Summer 2022" },
                new AssessmentCsvRecordResponse { RowNum = 10, Uln = 1111111113, SpecialismCodes = new List<string> {"10123456" }, SpecialismAssessmentEntry = "SUMMER 2022" },
                new AssessmentCsvRecordResponse { RowNum = 11, Uln = 1111111114, SpecialismCodes = new List<string> {"10123456", "10123457" }, SpecialismAssessmentEntry = "SUMMER 2022" }, // New entry for Couplet
                new AssessmentCsvRecordResponse { RowNum = 12, Uln = 1111111115, SpecialismCodes = new List<string> {"10123456" }, SpecialismAssessmentEntry = "SUMMER 2022" }, // Resit from Couplet
                new AssessmentCsvRecordResponse { RowNum = 13, Uln = 1111111116, SpecialismCodes = new List<string> {"10123456" }, SpecialismAssessmentEntry = "SUMMER 2022" }, // Registerd in 2021 (second cohort) and can take specialism in Summer 2022
            };
        }
    }
}
