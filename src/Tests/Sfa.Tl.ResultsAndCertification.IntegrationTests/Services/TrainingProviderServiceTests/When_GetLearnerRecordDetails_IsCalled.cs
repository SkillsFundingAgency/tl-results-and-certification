using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.TrainingProviderServiceTests
{
    public class When_GetLearnerRecordDetails_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private LearnerRecordDetails _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)>
            {
                (1111111111, false, true, true, true, true, true), // Lrs data with Send Qualification + IP
                (1111111112, true, false, false, false, false, null), // Not from Lrs
                (1111111113, false, true, false, true, false, false), // Lrs data without Send Qualification
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);
            TransferRegistration(1111111113, Provider.WalsallCollege);

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner);
            }

            DbContext.SaveChanges();

            // Create Service
            CreateMapper();
            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);
            
            IndustryPlacementRepositoryLogger = new Logger<GenericRepository<IndustryPlacement>>(new NullLoggerFactory());
            IndustryPlacementRepository = new GenericRepository<IndustryPlacement>(IndustryPlacementRepositoryLogger, DbContext);

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

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, RegistrationPathwayRepository, IndustryPlacementRepository, NotificationService, ResultsAndCertificationConfiguration, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, int profileId, int? pathwayId)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderService.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, bool includePathwayId, bool isTransferedRecord, bool expectedResult)
        {
            var profileId = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln)?.Id ?? 0;
            int? pathwayId = null;
            
            if(includePathwayId && profileId > 0)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var pathway = isTransferedRecord ? profile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Transferred)
                : profile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn);
                
                pathwayId = pathway.Id;
            }

            await WhenAsync((long)provider, profileId, pathwayId);

            if (expectedResult == false)
            {
                _actualResult.Should().BeNull();
                return;
            }            

            var expectedProvider = TlProviders.FirstOrDefault(p => p.UkPrn == (long)provider);
            expectedProvider.Should().NotBeNull();

            var expectedProviderName = expectedProvider != null ? $"{expectedProvider.Name} ({expectedProvider.UkPrn})" : null;
            var expectedProfile = _profiles.FirstOrDefault(p => p.Id == profileId);
            expectedProfile.Should().NotBeNull();

            var expectedPathway = isTransferedRecord ? expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Transferred)
                : expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn);

            expectedPathway.Should().NotBeNull();

            var tlpathway = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway;
            tlpathway.Should().NotBeNull();

            var expectedPathwayName = $"{tlpathway.Name} ({tlpathway.LarId})";
            var expectedIsLearnerRegistered = expectedPathway.Status == RegistrationPathwayStatus.Active || expectedPathway.Status == RegistrationPathwayStatus.Withdrawn;
            var expectedHasLrsEnglishAndMaths = expectedProfile.IsRcFeed == false && expectedProfile.QualificationAchieved.Any();
            var expectedIsLearnerRecordAdded = expectedPathway.TqRegistrationProfile.IsEnglishAndMathsAchieved.HasValue && expectedPathway.IndustryPlacements.Any();
            var expectedIndustryPlacementId = expectedPathway.IndustryPlacements.FirstOrDefault()?.Id ?? 0;
            var expectedIndustryPlacementStatus = expectedPathway.IndustryPlacements.FirstOrDefault()?.Status ?? null;

            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(expectedProfile.Id);
            _actualResult.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
            _actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedProviderName);
            _actualResult.PathwayName.Should().Be(expectedPathwayName);
            _actualResult.IsLearnerRegistered.Should().Be(expectedIsLearnerRegistered);
            _actualResult.IsLearnerRecordAdded.Should().Be(expectedIsLearnerRecordAdded);
            _actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedProfile.IsEnglishAndMathsAchieved ?? false);
            _actualResult.HasLrsEnglishAndMaths.Should().Be(expectedHasLrsEnglishAndMaths);
            _actualResult.IsSendLearner.Should().Be(expectedProfile.IsSendLearner);
            _actualResult.IndustryPlacementId.Should().Be(expectedIndustryPlacementId);
            _actualResult.IndustryPlacementStatus.Should().Be(expectedIndustryPlacementStatus);

        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, false, false, false }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarsleyCollege, true, false, true }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, false, false, false }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarsleyCollege, true, false, true }, // Withdrawn
                    new object[] { 1111111113, Provider.BarsleyCollege, false, true, true }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, true, false, true } // Active
                };
            }
        }

        private void TransferRegistration(long uln, Provider transferTo)
        {
            var toProvider = DbContext.TlProvider.FirstOrDefault(x => x.UkPrn == (long)transferTo);
            var transferToTqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, toProvider, true);

            var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);

            foreach (var pathway in profile.TqRegistrationPathways)
            {
                pathway.Status = RegistrationPathwayStatus.Transferred;
                pathway.EndDate = DateTime.UtcNow;

                foreach (var specialism in pathway.TqRegistrationSpecialisms)
                {
                    specialism.IsOptedin = true;
                    specialism.EndDate = DateTime.UtcNow;
                }
            }

            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, profile, transferToTqProvider);
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);
            DbContext.SaveChanges();
        }
    }
}
