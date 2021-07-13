using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_AppealGrade_IsCalled : PostResultsServiceServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;

        private bool _actualResult;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Withdrawn }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Active } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
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

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            // Results seed
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var assessment in _pathwayAssessments)
            {
                var inactiveResultUlns = new List<long> { 1111111112 };
                var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);

                tqPathwayResultsSeedData.AddRange(GetPathwayResultDataToProcess(assessment, isLatestResultActive, false));
            }
            _pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData);

            // Test class and dependencies. 
            CreateMapper();
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
            var pathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultsRepository = new GenericRepository<TqPathwayResult>(pathwayResultRepositoryLogger, DbContext);
            PostResultsServiceServiceLogger = new Logger<PostResultsServiceService>(new NullLoggerFactory());

            PostResultsServiceService = new PostResultsServiceService(PostResultsServiceRepository, PathwayResultsRepository, PostResultsServiceMapper, PostResultsServiceServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AppealGradeRequest request)
        {
            _actualResult = await PostResultsServiceService.AppealGradeAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(AppealGradeRequest request, bool expectedResult)
        {
            var assessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId && x.IsOptedin && x.EndDate == null);

            if (assessment != null)
            {
                var resultId = _pathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == assessment.Id && x.IsOptedin && x.EndDate == null)?.Id;
                if (resultId != null)
                {
                    request.ResultId = resultId.Value;
                }
            }

            await WhenAsync(request);

            // Assert
            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                     // Result not-found - returns false
                    new object[]
                    { new AppealGradeRequest { AoUkprn = 10011881, ProfileId = 999, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // Registration not in active status - returns false
                    new object[]
                    { new AppealGradeRequest { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // No active result - returns false
                    new object[]
                    { new AppealGradeRequest { AoUkprn = 10011881, ProfileId = 2, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // When componenttype = specialism - returns false
                    new object[]
                    { new AppealGradeRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed },
                     false },                    

                    // valid request with Active result - returns true
                    new object[]
                    { new AppealGradeRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      true }
                };
            }
        }
    }
}