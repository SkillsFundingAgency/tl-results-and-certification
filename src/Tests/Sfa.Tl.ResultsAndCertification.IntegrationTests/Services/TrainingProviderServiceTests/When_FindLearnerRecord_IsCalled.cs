﻿using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
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

            CreateMapper();

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
            RegistrationProfileRepositoryLogger = new Logger<GenericRepository<TqRegistrationProfile>>(new NullLoggerFactory());
            RegistrationProfileRepository = new GenericRepository<TqRegistrationProfile>(RegistrationProfileRepositoryLogger, DbContext);

            TrainingProviderRepository = new TrainingProviderRepository(DbContext);
            TrainingProviderServiceLogger = new Logger<TrainingProviderService>(new NullLoggerFactory());

            BatchRepositoryLogger = new Logger<GenericRepository<Batch>>(new NullLoggerFactory());
            BatchRepository = new GenericRepository<Batch>(BatchRepositoryLogger, DbContext);

            PrintCertificateRepositoryLogger = new Logger<GenericRepository<PrintCertificate>>(new NullLoggerFactory());
            PrintCertificateRepository = new GenericRepository<PrintCertificate>(PrintCertificateRepositoryLogger, DbContext);

            TrainingProviderService = new TrainingProviderService(RegistrationProfileRepository, TrainingProviderRepository, BatchRepository, PrintCertificateRepository, OverallResultCalculationService, TrainingProviderMapper, TrainingProviderServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderService.FindLearnerRecordAsync(providerUkprn, uln);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, FindLearnerRecord expectedResult)
        {
            await WhenAsync((long)provider, uln);

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
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, null }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111111, IsLearnerRegistered = true } },  // Active
                    new object[] { 1111111111, Provider.WalsallCollege, null }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111112, IsLearnerRegistered = true } },  // Withdrawn
                    new object[] { 1111111113, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = false } }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = true } },   // Active
                    
                    new object[] { 1111111114, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111114, IsLearnerRegistered = true } }
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
