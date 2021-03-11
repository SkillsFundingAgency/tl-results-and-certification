using FluentAssertions;
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
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active }
            };

            _testCriteriaData = new List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement)>
            {
                (1111111111, false, true, true, true, true), // Lrs data with Send Qualification + IP
                (1111111112, true, false, false, false, false), // Not from Lrs
                (1111111113, false, true, false, true, false), // Lrs data without Send Qualification
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _profiles = SeedRegistrationsData(_ulns, TqProvider);
            TransferRegistration(1111111113, Provider.WalsallCollege);

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved);
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

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long uln, Provider provider, FindLearnerRecord expectedResult)
        {
            var actualResult = TrainingProviderService.FindLearnerRecordAsync((long)provider, uln).Result;

            if (expectedResult == null)
            {
                actualResult.Should().BeNull();
                return;
            }

            var expectedProvider = TlProviders.FirstOrDefault(p => p.UkPrn == (long)provider);
            var expectedProviderName = expectedProvider != null ? $"{expectedProvider.Name} ({expectedProvider.UkPrn})" : null;
            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);

            expectedProfile.Should().NotBeNull();
            actualResult.Should().NotBeNull();
            actualResult.Uln.Should().Be(expectedResult.Uln);
            actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
            actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            actualResult.ProviderName.Should().Be(expectedProviderName);
            actualResult.IsLearnerRegistered.Should().Be(expectedResult.IsLearnerRegistered);
            actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedResult.IsEnglishAndMathsAchieved);
            actualResult.HasLrsEnglishAndMaths.Should().Be(expectedResult.HasLrsEnglishAndMaths);
            actualResult.HasSendQualification.Should().Be(expectedResult.HasSendQualification);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, null }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111111, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, HasSendQualification = true, IsLearnerRecordAdded = true } }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, null }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111112, IsLearnerRegistered = true, HasLrsEnglishAndMaths = false, HasSendQualification = false, IsLearnerRecordAdded = false } }, // Withdrawn
                    new object[] { 1111111113, Provider.BarsleyCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = false, HasLrsEnglishAndMaths = true, HasSendQualification = false, IsLearnerRecordAdded = false } }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, HasSendQualification = false, IsLearnerRecordAdded = false } } // Active
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

    public enum Provider
    {
        BarsleyCollege = 10000536,
        WalsallCollege = 10007315
    }
}
