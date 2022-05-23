using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public class When_FindLearnerRecordAsync_IsCalled : TrainingProviderRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)> _testCriteriaData;
        private IList<TqRegistrationProfile> _profiles;
        private FindLearnerRecord _actualResult;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
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
                (1111111113, false, true, false, true, false, null) // Lrs data without Send Qualification
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            Qualifications = SeedQualificationData();

            foreach (var uln in _ulns)
            {
                _profiles.Add(SeedRegistrationDataByStatus(uln.Key, uln.Value, TqProvider));
            }

            TransferRegistration(_profiles.FirstOrDefault(p => p.UniqueLearnerNumber == 1111111113), Provider.WalsallCollege);

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner);
            }

            DbContext.SaveChanges();

            // Test class.
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TraningProviderRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderRepository.FindLearnerRecordAsync(providerUkprn, uln);
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
            _actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedResult.IsEnglishAndMathsAchieved);
            _actualResult.HasLrsEnglishAndMaths.Should().Be(expectedResult.HasLrsEnglishAndMaths);
            _actualResult.IsSendLearner.Should().Be(expectedProfile.IsSendLearner);
            _actualResult.IsLearnerRecordAdded.Should().Be(expectedResult.IsLearnerRecordAdded);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, null }, // Invalid Uln
                    new object[] { 1111111111, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111111, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = true } }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, null }, // Uln not from WalsallCollege
                    new object[] { 1111111112, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111112, IsLearnerRegistered = true, HasLrsEnglishAndMaths = false, IsEnglishAndMathsAchieved = false, IsLearnerRecordAdded = false } }, // Withdrawn
                    new object[] { 1111111113, Provider.BarnsleyCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = false, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = false } }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, new FindLearnerRecord { Uln = 1111111113, IsLearnerRegistered = true, HasLrsEnglishAndMaths = true, IsEnglishAndMathsAchieved = true, IsLearnerRecordAdded = false } } // Active
                };
            }
        }
    }
}
