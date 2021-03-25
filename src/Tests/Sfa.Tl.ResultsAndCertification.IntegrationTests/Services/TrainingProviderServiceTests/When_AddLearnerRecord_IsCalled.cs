using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_AddLearnerRecord_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private AddLearnerRecordResponse _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement)>
            {
                (1111111111, false, true, true, true, true), // Lrs data and Learner Record added already
                (1111111112, false, true, false, false, false), // Lrs data and Learner Record not added already
                (1111111113, true, false, false, false, true), // Not from Lrs and Learner Record added already
                (1111111114, null, false, false, null, false), // Not from Lrs and Learner Record not added
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);            

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement);
            }

            DbContext.SaveChanges();

            // Create Service
            CreateMapper();
            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);
            TrainingProviderService = new TrainingProviderService(RegistrationPathwayRepository, TrainingProviderMapper);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AddLearnerRecordRequest request)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderService.AddLearnerRecordAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(AddLearnerRecordRequest request, AddLearnerRecordResponse expectedResult)
        {
            await WhenAsync(request);

            if (expectedResult == null)
            {
                _actualResult.Should().BeNull();
                return;
            }
            
            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == request.Uln);

            expectedProfile.Should().NotBeNull();
            _actualResult.Should().NotBeNull();
            _actualResult.IsSuccess.Should().Be(expectedResult.IsSuccess);

            if (expectedResult.IsSuccess)
            {
                _actualResult.Uln.Should().Be(expectedResult.Uln);
                _actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");

                var actualPathway = DbContext.TqRegistrationPathway.Where(p => p.TqRegistrationProfile.UniqueLearnerNumber == expectedProfile.UniqueLearnerNumber && p.TqProvider.TlProvider.UkPrn == request.Ukprn)
                                                                   .Include(p => p.IndustryPlacements)
                                                                   .Include(p => p.TqRegistrationProfile)
                                                                   .OrderByDescending(p => p.CreatedOn).FirstOrDefault();

                actualPathway.Should().NotBeNull();

                // Assert Profile data

                if (!request.HasLrsEnglishAndMaths)
                {
                    var expectedEnglishAndMaths = request.EnglishAndMathsStatus == EnglishAndMathsStatus.Achieved || request.EnglishAndMathsStatus == EnglishAndMathsStatus.AchievedWithSend;
                    var expectedIsSendLearner = request.EnglishAndMathsStatus == EnglishAndMathsStatus.AchievedWithSend ? true : (bool?)null;
                    actualPathway.TqRegistrationProfile.IsEnglishAndMathsAchieved.Should().Be(expectedEnglishAndMaths);
                    actualPathway.TqRegistrationProfile.IsSendLearner.Should().Be(expectedIsSendLearner);
                    actualPathway.TqRegistrationProfile.IsRcFeed.Should().Be(true);
                }

                actualPathway.IndustryPlacements.Count().Should().Be(1);
                var actualIndustryPlacement = actualPathway.IndustryPlacements.Single();
                actualIndustryPlacement.TqRegistrationPathwayId.Should().Be(actualPathway.Id);
                actualIndustryPlacement.Status.Should().Be(request.IndustryPlacementStatus);
            }            
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                     // Invalid Provider Ukprn - return false
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = 0000000000, Uln = 1111111111, HasLrsEnglishAndMaths = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = false } },

                    // Learner Record Already Added (Lrs data) - return false
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111111, HasLrsEnglishAndMaths = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = false } },

                    // Learner Record not Added (LRS data with no industry placement) but sent EnglishAndMathsStatus - return false
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111112, HasLrsEnglishAndMaths = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = false, Uln = 1111111112 } },

                    // Learner Record not Added (LRS data with no industry placement) - return true
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111112, HasLrsEnglishAndMaths = true, EnglishAndMathsStatus = null, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = true, Uln = 1111111112 } },

                    // Not from Lrs and Learner Record added already - return false
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111113, HasLrsEnglishAndMaths = false, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = false } },

                    // Not from Lrs and Learner Record not added but HasLrsEnglishAndMaths set to true - return false
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111114, HasLrsEnglishAndMaths = true, EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = false, Uln = 1111111114 } },

                    // Not from Lrs and Learner Record not added - return true
                    new object[]
                    { new AddLearnerRecordRequest { Ukprn = (long)Provider.BarsleyCollege, Uln = 1111111114, HasLrsEnglishAndMaths = false, EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend, IndustryPlacementStatus = IndustryPlacementStatus.Completed, PerformedBy = "Test User" },
                      new AddLearnerRecordResponse { IsSuccess = true, Uln = 1111111114 } },
                };
            }
        }       
    }
}
