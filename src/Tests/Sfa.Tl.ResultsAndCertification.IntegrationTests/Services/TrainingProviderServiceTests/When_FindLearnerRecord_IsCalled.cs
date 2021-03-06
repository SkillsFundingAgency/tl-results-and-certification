﻿using FluentAssertions;
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
    public class When_FindLearnerRecord_IsCalled : TrainingProviderServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private FindLearnerRecord _actualResult;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)>
            {
                (1111111111, false, true, true, true, true, true), // Lrs data with Send Qualification + IP
                (1111111112, true, false, false, false, false, null), // Not from Lrs
                (1111111113, false, true, false, true, false, null), // Lrs data without Send Qualification
                (1111111114, false, true, true, true, false, null), // Lrs data with Send Qualification
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

            TrainingProviderRepositoryLogger = new Logger<TrainingProviderRepository>(new NullLoggerFactory());
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TrainingProviderRepositoryLogger);
            
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

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, RegistrationPathwayRepository, IndustryPlacementRepository, TrainingProviderRepository, NotificationService, ResultsAndCertificationConfiguration, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, long uln, bool isSendConfirmationRequired)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderService.FindLearnerRecordAsync(providerUkprn, uln, isSendConfirmationRequired);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, bool isSendConfirmationRequired, FindLearnerRecord expectedResult)
        {
            await WhenAsync((long)provider, uln, isSendConfirmationRequired);

            if (expectedResult == null)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedProvider = TlProviders.FirstOrDefault(p => p.UkPrn == (long)provider);
            var expectedProviderName = expectedProvider != null ? $"{expectedProvider.Name} ({expectedProvider.UkPrn})" : null;
            var expectedPathwayName = $"{Pathway.Name} ({Pathway.LarId})";
            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);

            expectedProfile.Should().NotBeNull();
            _actualResult.Should().NotBeNull();
            _actualResult.Uln.Should().Be(expectedResult.Uln);
            _actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedProviderName);
            _actualResult.PathwayName.Should().Be(expectedPathwayName);
            _actualResult.IsLearnerRegistered.Should().Be(expectedResult.IsLearnerRegistered);
            _actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedResult.IsEnglishAndMathsAchieved);
            _actualResult.HasLrsEnglishAndMaths.Should().Be(expectedResult.HasLrsEnglishAndMaths);
            _actualResult.IsSendLearner.Should().Be(expectedProfile.IsSendLearner);
            _actualResult.IsLearnerRecordAdded.Should().Be(expectedResult.IsLearnerRecordAdded);
            _actualResult.IsSendConfirmationRequired.Should().Be(expectedResult.IsSendConfirmationRequired);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, false, null }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarsleyCollege, false, new FindLearnerRecord { Uln = 1111111111, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = true } }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, false, null }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarsleyCollege, false, new FindLearnerRecord { Uln = 1111111112, IsLearnerRegistered = true, HasLrsEnglishAndMaths = false, IsEnglishAndMathsAchieved = false, IsLearnerRecordAdded = false } }, // Withdrawn
                    new object[] { 1111111113, Provider.BarsleyCollege, false, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = false, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = false } }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, false, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = false } }, // Active
                    
                    new object[] { 1111111114, Provider.BarsleyCollege, true, new FindLearnerRecord { Uln = 1111111114, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = false, IsSendConfirmationRequired = true } }
                };
            }
        }

        private void TransferRegistration(long uln, Provider transferTo)
        {
            var toProvider = DbContext.TlProvider.FirstOrDefault(x => x.UkPrn == (long)transferTo);
            var transferToTqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, toProvider, true);

            var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);

            foreach(var pathway in profile.TqRegistrationPathways)
            {
                pathway.Status = RegistrationPathwayStatus.Transferred;
                pathway.EndDate = DateTime.UtcNow;

                foreach(var specialism in pathway.TqRegistrationSpecialisms)
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
