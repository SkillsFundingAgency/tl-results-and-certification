using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PostResultsServiceTests
{
    public class When_PrsActivity_For_Appeal_IsCalled : PostResultsServiceServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private List<TqSpecialismResult> _specialismResults;

        private bool _actualResult;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active }
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            _pathwayResults = new List<TqPathwayResult>();
            _specialismResults = new List<TqSpecialismResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Results seed
                var tqPathwayResultsSeedData = new List<TqPathwayResult>();
                var profilesWithResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, PrsStatus.UnderReview), (1111111114, PrsStatus.Reviewed), (1111111115, PrsStatus.BeingAppealed) };
                foreach (var assessment in pathwayAssessments)
                {
                    var inactiveResultUlns = new List<long> { 1111111112 };
                    var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);

                    var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                    _pathwayResults.AddRange(GetPathwayResultDataToProcess(assessment, isLatestResultActive, false, prsStatus));
                }

                // Specialism Assesments
                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    var specialismAssessments = GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent);
                    tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                    foreach (var specialismAssessment in specialismAssessments)
                    {
                        // Specialism Results
                        var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                        _specialismResults.AddRange(GetSpecialismResultDataToProcess(specialismAssessment, isLatestActive, isHistoricAssessent, prsStatus));
                    }
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            DbContext.SaveChanges();

            // Test class and dependencies. 
            CreateMapper();
            PostResultsServiceRepository = new PostResultsServiceRepository(DbContext);
            var pathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultsRepository = new GenericRepository<TqPathwayResult>(pathwayResultRepositoryLogger, DbContext);

            var specialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultsRepository = new GenericRepository<TqSpecialismResult>(specialismResultRepositoryLogger, DbContext);

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);

            Configuration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };

            PostResultsServiceServiceLogger = new Logger<PostResultsServiceService>(new NullLoggerFactory());
            PostResultsServiceService = new PostResultsServiceService(Configuration, PostResultsServiceRepository, PathwayResultsRepository, SpecialismResultsRepository, NotificationService, PostResultsServiceMapper, PostResultsServiceServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(PrsActivityRequest request)
        {
            _actualResult = await PostResultsServiceService.PrsActivityAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(PrsActivityRequest request, bool expectedResult)
        {
            int? assessmentId = null;

            if (request.ComponentType == ComponentType.Core)
                assessmentId = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId && x.IsOptedin && x.EndDate == null)?.Id;
            else
                assessmentId = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId && x.IsOptedin && x.EndDate == null)?.Id;

            if (assessmentId != null)
            {
                int? resultId = null;

                if (request.ComponentType == ComponentType.Core)
                    resultId = _pathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null)?.Id;
                else
                    resultId = _specialismResults.FirstOrDefault(x => x.TqSpecialismAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null)?.Id;

                if (resultId != null)
                {
                    request.ResultId = resultId.Value;
                }
            }

            await WhenAsync(request);

            // Assert
            _actualResult.Should().Be(expectedResult);

            if (assessmentId != null)
            {
                if (request.ComponentType == ComponentType.Core)
                {
                    var latestResult = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null);
                    if (expectedResult == true)
                    {
                        if (request.PrsStatus == PrsStatus.Withdraw)
                        {
                            var previousResult = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessmentId == assessmentId && !x.IsOptedin && x.EndDate != null && x.PrsStatus == PrsStatus.BeingAppealed);

                            latestResult.PrsStatus.Should().Be(PrsStatus.Reviewed);
                            latestResult.TlLookup.Value.Should().Be(previousResult.TlLookup.Value);
                            return;
                        }

                        latestResult.PrsStatus.Should().Be(request.PrsStatus);

                        if (request.ResultLookupId > 0)
                        {
                            var expectedGrade = DbContext.TlLookup.FirstOrDefault(x => x.Id == request.ResultLookupId).Value;
                            latestResult.TlLookup.Value.Should().Be(expectedGrade);
                        }
                    }
                    else
                    {
                        var previousResult = _pathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null);
                        if (previousResult != null)
                        {
                            latestResult.PrsStatus.Should().Be(previousResult.PrsStatus);
                            latestResult.TlLookup.Value.Should().Be(previousResult.TlLookup.Value);
                        }
                    }
                }
                else
                {
                    var latestResult = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null);
                    if (expectedResult == true)
                    {
                        if (request.PrsStatus == PrsStatus.Withdraw)
                        {
                            var previousResult = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessmentId == assessmentId && !x.IsOptedin && x.EndDate != null && x.PrsStatus == PrsStatus.BeingAppealed);

                            latestResult.PrsStatus.Should().NotBeNull();
                            latestResult.TlLookup.Value.Should().Be(previousResult.TlLookup.Value);
                            return;
                        }

                        latestResult.PrsStatus.Should().Be(request.PrsStatus);

                        if (request.ResultLookupId > 0)
                        {
                            var expectedGrade = DbContext.TlLookup.FirstOrDefault(x => x.Id == request.ResultLookupId).Value;
                            latestResult.TlLookup.Value.Should().Be(expectedGrade);
                        }
                    }
                    else
                    {
                        var previousResult = _specialismResults.FirstOrDefault(x => x.TqSpecialismAssessmentId == assessmentId && x.IsOptedin && x.EndDate == null);
                        if (previousResult != null)
                        {
                            latestResult.PrsStatus.Should().Be(previousResult.PrsStatus);
                            latestResult.TlLookup.Value.Should().Be(previousResult.TlLookup.Value);
                        }
                    }
                }
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    //Result not-found - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 999, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // Registration not in active status - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // No active result - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 2, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },
                    
                    // CurrentStatus is UnderReview -> Requesting BeingAppealed - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // CurrentStatus is UnderReview -> Requesting Final - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.Final, ResultLookupId = 3 },
                      false },                    

                    // valid request with CurrentStatus is Reviewed -> Requesting BeingAppealed - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.BeingAppealed, ResultLookupId = 1 },
                      true },

                    // valid request with CurrentStatus is Reviewed -> Requesting Final - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.Final, ResultLookupId = 3 },
                      true },

                    // valid request with CurrentStatus is BeingAppealed -> Requesting Final - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.Final, ResultLookupId = 3 },
                      true },

                    // When componenttype = core - CurrentStatus is BeingAppealed -> Requesting Withdraw
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.Core, PrsStatus = PrsStatus.Withdraw, ResultLookupId = 3 },
                      true },

                    // Specialism test data

                    //Result not-found - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 999, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // Registration not in active status - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // No active result - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 2, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed },
                      false },
                    
                    // CurrentStatus is UnderReview -> Requesting BeingAppealed - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed },
                      false },

                    // CurrentStatus is UnderReview -> Requesting Final - returns false
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.Final, ResultLookupId = 8 },
                      false },                    

                    // valid request with CurrentStatus is Reviewed -> Requesting BeingAppealed - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.BeingAppealed, ResultLookupId = 8 },
                      true },

                    // valid request with CurrentStatus is Reviewed -> Requesting Final - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.Final, ResultLookupId = 8 },
                      true },

                    // valid request with CurrentStatus is BeingAppealed -> Requesting Final - returns true
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.Final, ResultLookupId = 8 },
                      true },

                     // When componenttype = specialism - CurrentStatus is BeingAppealed -> Requesting Withdraw
                    new object[]
                    { new PrsActivityRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.Specialism, PrsStatus = PrsStatus.Withdraw, ResultLookupId = 8 },
                      true },
                };
            }
        }
    }
}
