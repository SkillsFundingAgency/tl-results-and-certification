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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Invalid_Results : ResultServiceBaseTest
    {
        private ResultProcessResponse _result;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayResult> _inputPathwayResultsData;
        private List<TqSpecialismResult> _inputSpecialismResultsData;
        private IList<BulkProcessValidationError> _expectedValidationErrors;
        private Dictionary<long, RegistrationPathwayStatus> _ulns;

        public override void Given()
        {
            CreateMapper();

            SetupExpectedValidationErrors();

            _ulns = new Dictionary<long, RegistrationPathwayStatus> 
            { 
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active }
            };
            
            // Data seed
            SeedTestData(EnumAwardingOrganisation.Pearson);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent));
            }

            var pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);

            _inputSpecialismResultsData = new List<TqSpecialismResult>();
            _inputPathwayResultsData = new List<TqPathwayResult>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var profilesWithResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, PrsStatus.UnderReview), (1111111114, PrsStatus.BeingAppealed), (1111111115, null), (1111111116, null) };
            foreach (var assessment in pathwayAssessments)
            {
                var inactiveResultUlns = new List<long> { 1111111112 };
                var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);
                var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isLatestResultActive, false, prsStatus));                
            }           

            SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            // Specialism Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent));
            }

            var specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            var ulnsToAddResults = new List<long> { 1111111113, 1111111114 };
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();
            var profilesWithSpecialismResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, null), (1111111114, null), (1111111115, PrsStatus.UnderReview), (1111111116, PrsStatus.BeingAppealed) };
            
            foreach (var assessment in specialismAssessments)
            {
                var inactiveResultUlns = new List<long> { 1111111112 };
                var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);
                var prsStatus = profilesWithSpecialismResults.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }, isLatestResultActive, false, prsStatus));
            }

            SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            DbContext.SaveChanges();

            _inputPathwayResultsData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments));

            _inputSpecialismResultsData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessments));

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

        private void SetupExpectedValidationErrors()
        {
            _expectedValidationErrors = new List<BulkProcessValidationError>
            {
                // Pathway validation result (UnderReview)
                new BulkProcessValidationError
                {
                    Uln = "1111111113",
                    ErrorMessage = ValidationMessages.ResultCannotBeInUnderReviewOrBeingAppealedStatus
                },
                // Pathway validation result (BeingAppealed)
                new BulkProcessValidationError
                {
                    Uln = "1111111114",
                    ErrorMessage = ValidationMessages.ResultCannotBeInUnderReviewOrBeingAppealedStatus
                },
                // Specialism validation result (UnderReview)
                new BulkProcessValidationError
                {
                    Uln = "1111111115",
                    ErrorMessage = ValidationMessages.ResultCannotBeInUnderReviewOrBeingAppealedStatus
                },
                // Specialism validation result (BeingAppealed)
                new BulkProcessValidationError
                {
                    Uln = "1111111116",
                    ErrorMessage = ValidationMessages.ResultCannotBeInUnderReviewOrBeingAppealedStatus
                }
            };
        }
    }
}
