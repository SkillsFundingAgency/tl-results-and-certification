using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_UpdateLearnerRecordAsync_For_EnglishAndMaths_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool? isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool? isEngishAndMathsAchieved, bool seedIndustryPlacement)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private bool _actualResult;

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
                (1111111112, false, true, false, false, false), // Lrs data and Learner Record not added
                (1111111113, true, false, false, null, false), // Not from Lrs and Learner Record not added
                (1111111114, true, false, false, true, true), // Not from Lrs and Learner Record added
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

            CreateMapper();

            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);

            IndustryPlacementRepositoryLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementRepositoryLogger, DbContext);

            TrainingProviderRepositoryLogger = new Logger<TrainingProviderRepository>(new NullLoggerFactory());
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TrainingProviderRepositoryLogger);

            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());

            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);

            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, RegistrationPathwayRepository, IndustryPlacementRepository, TrainingProviderRepository, NotificationService, ResultsAndCertificationConfiguration, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(UpdateLearnerRecordRequest request)
        {
            _actualResult = await TrainingProviderService.UpdateLearnerRecordAsync(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(UpdateLearnerRecordRequest request, bool expectedResult)
        {
            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == request.Uln);
            expectedProfile.Should().NotBeNull();

            request.ProfileId = expectedProfile.Id;

            await WhenAsync(request);

            if (expectedResult == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            _actualResult.Should().BeTrue();

            var actualProfile = await DbContext.TqRegistrationProfile.FirstOrDefaultAsync(p => p.Id == expectedProfile.Id && p.UniqueLearnerNumber == expectedProfile.UniqueLearnerNumber
                                                                        && p.IsEnglishAndMathsAchieved.HasValue && p.IsRcFeed == true
                                                                        && p.TqRegistrationPathways.Any(pa => pa.TqProvider.TlProvider.UkPrn == request.Ukprn
                                                                        && (pa.Status == RegistrationPathwayStatus.Active || pa.Status == RegistrationPathwayStatus.Withdrawn)));

            actualProfile.Should().NotBeNull();

            // Assert Profile data

            actualProfile.Id.Should().Be(expectedProfile.Id);
            actualProfile.UniqueLearnerNumber.Should().Be(expectedProfile.UniqueLearnerNumber);

            // Assert EnglishAndMaths data
            var expectedEnglishAndMathAchieved = request.EnglishAndMathsStatus == EnglishAndMathsStatus.Achieved || request.EnglishAndMathsStatus == EnglishAndMathsStatus.AchievedWithSend;
            var expectedSendLearner = request.EnglishAndMathsStatus == EnglishAndMathsStatus.AchievedWithSend ? true : (bool?)null;
            actualProfile.IsEnglishAndMathsAchieved.Should().Be(expectedEnglishAndMathAchieved);
            actualProfile.IsSendLearner.Should().Be(expectedSendLearner);
            actualProfile.ModifiedBy.Should().Be(request.PerformedBy);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Invalid Provider Ukprn - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = 0000000000, Uln = 1111111111, HasEnglishAndMathsChanged = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, PerformedBy = "Test User" }, false },

                    // Learner Record not Added (LRS data) - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarnsleyCollege, Uln = 1111111112, HasEnglishAndMathsChanged = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, PerformedBy = "Test User" }, false },

                    // Not from Lrs and Learner Record not added - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarnsleyCollege, Uln = 1111111113, HasEnglishAndMathsChanged = true, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, PerformedBy = "Test User" }, false },

                    // Not from Lrs and Learner Record added but HasEnglishAndMathsChanged set to false - return false
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarnsleyCollege, Uln = 1111111114, HasEnglishAndMathsChanged = false, EnglishAndMathsStatus = EnglishAndMathsStatus.Achieved, PerformedBy = "Test User" }, false },

                    // Not from Lrs and Learner Record added - return true
                    new object[]
                    { new UpdateLearnerRecordRequest { Ukprn = (long)Provider.BarnsleyCollege, Uln = 1111111114, HasEnglishAndMathsChanged = true, EnglishAndMathsStatus = EnglishAndMathsStatus.AchievedWithSend, PerformedBy = "Test User" }, true }
                };
            }
        }

        protected override void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(TrainingProviderMapper).Assembly);                
                c.ConstructServicesUsing(type =>
                            type.Name.Contains("DateTimeResolver") ?
                                new DateTimeResolver<UpdateLearnerRecordRequest, TqRegistrationProfile>(new DateTimeProvider()) :
                                null);
            });
            TrainingProviderMapper = new Mapper(mapperConfig);
        }
    }
}
