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
        private readonly Dictionary<long, RegistrationPathwayStatus> _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Withdrawn }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Active }, { 1111111114, RegistrationPathwayStatus.Active }, { 1111111115, RegistrationPathwayStatus.Active } };

        public override void Given()
        {
            CreateMapper();

            SetupExpectedValidationErrors();

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

            var pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);

            _inputSpecialismResultsData = new List<TqSpecialismResult>();
            _inputPathwayResultsData = new List<TqPathwayResult>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var profilesWithResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, null), (1111111114, null), (1111111115, PrsStatus.BeingAppealed) };
            foreach (var assessment in pathwayAssessments)
            {
                var inactiveResultUlns = new List<long> { 1111111112 };
                var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);
                var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isLatestResultActive, false, prsStatus));                
            }           

            SeedPathwayResultsData(tqPathwayResultsSeedData);

            DbContext.SaveChanges();

            _inputPathwayResultsData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments));

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
                new BulkProcessValidationError
                {
                    Uln = "1111111115",
                    ErrorMessage = ValidationMessages.ResultCannotBeInBeingAppealedStatus
                }
            };
        }
    }
}
