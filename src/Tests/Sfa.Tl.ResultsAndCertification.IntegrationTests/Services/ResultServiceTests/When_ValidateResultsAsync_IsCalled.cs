using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
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
        private List<TqSpecialismAssessment> _specialismAssessments;

        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus> 
        { 
            { 1111111111, RegistrationPathwayStatus.Withdrawn },
            { 1111111112, RegistrationPathwayStatus.Active },
            { 1111111113, RegistrationPathwayStatus.Active },
            { 1111111114, RegistrationPathwayStatus.Active },
            { 1111111115, RegistrationPathwayStatus.Active }
        };

        public override void Given()
        {
            CreateMapper();

            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Second cohort Ulns for Specialisms assessment entry to be taken in 1st year
            var secondCohortUlns = new List<long> { 1111111115 };
            RegisterUlnForNextCohort(_registrations, secondCohortUlns, 2021);

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

            // Specialism Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111 && x.UniqueLearnerNumber != 1111111112))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent));
            }

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            DbContext.SaveChanges();

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
            var actualResult = _stage3Result.Result;

            var expectedValidationErrors = GetExpectedValidationErrors();
            var actualValidationErrors = actualResult.Where(x => !x.IsValid).SelectMany(x => x.ValidationErrors).ToList();
            actualValidationErrors.Should().HaveCount(expectedValidationErrors.Count);

            actualValidationErrors.Should().BeEquivalentTo(expectedValidationErrors);

            // Valid Row should populate the response Object
            var actualValidRows = actualResult.Where(x => x.IsValid);
            actualValidRows.SelectMany(x => x.ValidationErrors).Count().Should().Be(0);

            var expectedTqPathwayAssessmentId = _pathwayAssessments.FirstOrDefault(pa => pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == 1111111113 && pa.IsOptedin && pa.EndDate == null)?.Id;
            var expectedTlLookupId = TlLookup.FirstOrDefault(x => x.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals("A", StringComparison.InvariantCultureIgnoreCase))?.Id;
            actualValidRows.FirstOrDefault().TqPathwayAssessmentId.Should().Be(expectedTqPathwayAssessmentId);
            actualValidRows.FirstOrDefault().PathwayComponentGradeLookupId.Should().Be(expectedTlLookupId);

            var expectedTqSpecialismAssessmentId = _specialismAssessments.FirstOrDefault(pa => pa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == 1111111113 && pa.IsOptedin && pa.EndDate == null).Id;
            var expectedSpecialismTlLookupId = SpecialismComponentGrades.FirstOrDefault(x => x.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals("Merit", StringComparison.InvariantCultureIgnoreCase))?.Id;
            var expecctedSpecialismResult = new Dictionary<int, int?>() { { expectedTqSpecialismAssessmentId, expectedSpecialismTlLookupId } };
            actualValidRows.FirstOrDefault().SpecialismResults.Should().BeEquivalentTo(expecctedSpecialismResult);
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
                new ResultCsvRecordResponse { RowNum = 10, Uln = 1111111113, CoreCode = "10123456", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A", SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Summer 2022", SpecialismGrades = new List<string> { "Merit" }},
                
                // 11. Specialism - code not recognised 
                new ResultCsvRecordResponse { RowNum = 11, Uln = 1111111114, SpecialismCodes = new List<string> { "12345678" }, SpecialismAssessmentSeries = "Summer 2022", SpecialismGrades = new List<string> { "Merit" }},
            
                // 12. Specialism - Assessment series does not exist.
                new ResultCsvRecordResponse { RowNum = 12, Uln = 1111111113, SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Hello 2022", SpecialismGrades = new List<string> { "Merit" }},

                // 13. Specialism - No assessment entry 
                new ResultCsvRecordResponse { RowNum = 13, Uln = 1111111112, SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Summer 2022", SpecialismGrades = new List<string> { "Merit" }},

                // 14. Specialism - Assessment series does not match with registered entry and Assessment series is not open
                new ResultCsvRecordResponse { RowNum = 14, Uln = 1111111113, SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Summer 2023", SpecialismGrades = new List<string> { "Merit" }},

                // 16. Specialism - Grade is not valid 
                new ResultCsvRecordResponse { RowNum = 16, Uln = 1111111114, SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Summer 2022", SpecialismGrades = new List<string> { "Hello" }},

                 // 17. Valid Row - Second cohort learner registerd in 2021 and allowed to take specialism in summer 2022
                new ResultCsvRecordResponse { RowNum = 17, Uln = 1111111115, CoreCode = "10123456", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A", SpecialismCodes = new List<string> { "10123456" }, SpecialismAssessmentSeries = "Summer 2022", SpecialismGrades = new List<string> { "Merit" }},

            };
        }

        private IList<BulkProcessValidationError> GetExpectedValidationErrors()
        {
            var validationErrors = new List<BulkProcessValidationError>
            {
                new BulkProcessValidationError { RowNum = "1", Uln = "9999999999", ErrorMessage = "ULN not registered with awarding organisation"  },
                new BulkProcessValidationError { RowNum = "2", Uln = "1111111111", ErrorMessage = "Cannot add results to a withdrawn registration"  },
                new BulkProcessValidationError { RowNum = "3", Uln = "1111111112", ErrorMessage = "Core component code either not recognised or not registered for this ULN" },
                new BulkProcessValidationError { RowNum = "4", Uln = "1111111112", ErrorMessage = "Assessment series does not exist - see results data format and rules guide for examples of valid series"  },
                new BulkProcessValidationError { RowNum = "5", Uln = "1111111112", ErrorMessage = "Core component grade not valid - needs to be A* to E, or Unclassified"  },
                new BulkProcessValidationError { RowNum = "6", Uln = "1111111114", ErrorMessage = "No assessment entry is currently active for the core component on this registration - needs adding first through assessment entries file upload or manual entry"  },
                new BulkProcessValidationError { RowNum = "7", Uln = "1111111113", ErrorMessage = "Assessment series does not match the series on the registration" },
                new BulkProcessValidationError { RowNum = "8", Uln = "1111111113", ErrorMessage = "Core component code either not recognised or not registered for this ULN"  },
                new BulkProcessValidationError { RowNum = "8", Uln = "1111111113", ErrorMessage = "Assessment series does not exist - see results data format and rules guide for examples of valid series"  },
                new BulkProcessValidationError { RowNum = "8", Uln = "1111111113", ErrorMessage = "Core component grade not valid - needs to be A* to E, or Unclassified"  },
                new BulkProcessValidationError { RowNum = "9", Uln = "1111111114", ErrorMessage = "Core component code either not recognised or not registered for this ULN"  },
                new BulkProcessValidationError { RowNum = "9", Uln = "1111111114", ErrorMessage = "Assessment series does not exist - see results data format and rules guide for examples of valid series"  },
                new BulkProcessValidationError { RowNum = "9", Uln = "1111111114", ErrorMessage = "Core component grade not valid - needs to be A* to E, or Unclassified"  },
                new BulkProcessValidationError { RowNum = "9", Uln = "1111111114", ErrorMessage = "No assessment entry is currently active for the core component on this registration - needs adding first through assessment entries file upload or manual entry" },

                new BulkProcessValidationError { RowNum = "11", Uln = "1111111114", ErrorMessage = "Specialism code(s) either not recognised or not registered for this ULN" },
                new BulkProcessValidationError { RowNum = "12", Uln = "1111111113", ErrorMessage = "Specialism assessment series does not exist" },
                new BulkProcessValidationError { RowNum = "13", Uln = "1111111112", ErrorMessage = "No assessment entry is currently active for the Specialism on this registration - needs adding first through assessment entries file upload or manual entry" },
                new BulkProcessValidationError { RowNum = "14", Uln = "1111111113", ErrorMessage = "Assessment series does not match the series on the registration" },
                new BulkProcessValidationError { RowNum = "14", Uln = "1111111113", ErrorMessage = "Incorrect Assessment series" },
                new BulkProcessValidationError { RowNum = "16", Uln = "1111111114", ErrorMessage = "Specialism grade not valid" },
            };

            return validationErrors;
        }
    }
}
