using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
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

        [Theory]
        [MemberData(nameof(Data))]
        public void Then_Returns_Expected_Results(long uln, Provider provider, bool isTransferedRecord, bool expectedResult)
        {
            var profileId = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln)?.Id ?? 0;
            var actualResult = TrainingProviderService.GetLearnerRecordDetailsAsync((long)provider, profileId).Result;

            if (expectedResult == false)
            {
                actualResult.Should().BeNull();
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

            actualResult.Should().NotBeNull();
            actualResult.ProfileId.Should().Be(expectedProfile.Id);
            actualResult.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
            actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
            actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            actualResult.ProviderName.Should().Be(expectedProviderName);
            actualResult.PathwayName.Should().Be(expectedPathwayName);
            actualResult.IsLearnerRegistered.Should().Be(expectedIsLearnerRegistered);
            actualResult.IsLearnerRecordAdded.Should().Be(expectedIsLearnerRecordAdded);
            actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedProfile.IsEnglishAndMathsAchieved ?? false);
            actualResult.HasLrsEnglishAndMaths.Should().Be(expectedHasLrsEnglishAndMaths);
            actualResult.IsSendLearner.Should().Be(expectedProfile.IsSendLearner ?? false);
            actualResult.IndustryPlacementId.Should().Be(expectedIndustryPlacementId);
            actualResult.IndustryPlacementStatus.Should().Be(expectedIndustryPlacementStatus);

        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, false, false }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarsleyCollege, false, true }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, false, false }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarsleyCollege, false, true }, // Withdrawn
                    new object[] { 1111111113, Provider.BarsleyCollege, true, true }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, false, true } // Active
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
