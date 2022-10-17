using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.TrainingProviderRepositoryTests
{
    public class When_GetLearnerRecordDetailsAsync_IsCalled : TrainingProviderRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner)> _testCriteriaData;
        private List<TqRegistrationProfile> _profiles;
        private LearnerRecordDetails _actualResult;
        private List<PrintCertificate> _printCertificates;

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
                (1111111113, false, true, false, true, false, false), // Lrs data without Send Qualification
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

            // Overall Results seed
            var ulnsWithOverallResult = new List<long> { 1111111112, 1111111113 };
            var _overallResults = SeedOverallResultData(_profiles, ulnsWithOverallResult, false);

            // Seed PrintCertificate
            _printCertificates = new List<PrintCertificate>();

            foreach (var profile in _profiles)
                _printCertificates.Add(SeedPrintCertificate(profile.TqRegistrationPathways.FirstOrDefault()));

            DbContext.SaveChanges();

            // Test class.
            TrainingProviderRepository = new TrainingProviderRepository(DbContext, TraningProviderRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, int profileId, int? pathwayId)
        {
            if (_actualResult != null)
                return;

            _actualResult = await TrainingProviderRepository.GetLearnerRecordDetailsAsync(providerUkprn, profileId, pathwayId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, bool includePathwayId, bool isTransferedRecord, bool expectedResult)
        {
            var profileId = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln)?.Id ?? 0;
            int? pathwayId = null;

            if (includePathwayId && profileId > 0)
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

            var expectedProviderName = expectedProvider != null ? expectedProvider.Name : null;
            var expectedProviderUkprn = expectedProvider != null ? expectedProvider.UkPrn : 0;
            var expectedProfile = _profiles.FirstOrDefault(p => p.Id == profileId);
            expectedProfile.Should().NotBeNull();

            var providerAddress = expectedProvider?.TlProviderAddresses?.OrderByDescending(ad => ad.CreatedOn)?.FirstOrDefault();
            var expectedProviderAddress = new Address { AddressId = providerAddress?.Id ?? 0, OrganisationName = providerAddress?.OrganisationName, DepartmentName = providerAddress?.DepartmentName, AddressLine1 = providerAddress?.AddressLine1, AddressLine2 = providerAddress?.AddressLine2, Town = providerAddress?.Town, Postcode = providerAddress?.Postcode };

            var expectedPathway = isTransferedRecord ? expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Transferred)
                : expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active || p.Status == RegistrationPathwayStatus.Withdrawn);

            expectedPathway.Should().NotBeNull();

            var tlpathway = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway;
            tlpathway.Should().NotBeNull();

            var expectedIsLearnerRegistered = expectedPathway.Status == RegistrationPathwayStatus.Active || expectedPathway.Status == RegistrationPathwayStatus.Withdrawn;
            var expectedIndustryPlacementId = expectedPathway.IndustryPlacements.FirstOrDefault()?.Id ?? 0;
            var expectedIndustryPlacementStatus = expectedPathway.IndustryPlacements.FirstOrDefault()?.Status ?? null;
            var expectedOverallResult = expectedPathway.OverallResults.FirstOrDefault(o => o.TqRegistrationPathway.Status == RegistrationPathwayStatus.Withdrawn ? o.EndDate != null : o.EndDate == null);
            var expectedOverallReultDetails = expectedOverallResult?.Details;
            var expectedOverallResultPublishDate = expectedOverallResult?.PublishDate;

            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(expectedProfile.Id);
            _actualResult.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
            _actualResult.Name.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedProviderName);
            _actualResult.ProviderUkprn.Should().Be(expectedProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(tlpathway.TlevelTitle);
            _actualResult.AcademicYear.Should().Be(expectedPathway.AcademicYear);
            _actualResult.AwardingOrganisationName.Should().Be(expectedPathway.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton.DisplayName);
            _actualResult.MathsStatus.Should().Be(expectedProfile.MathsStatus);
            _actualResult.EnglishStatus.Should().Be(expectedProfile.EnglishStatus);

            _actualResult.IsLearnerRegistered.Should().Be(expectedIsLearnerRegistered);
            _actualResult.IndustryPlacementId.Should().Be(expectedIndustryPlacementId);
            _actualResult.IndustryPlacementStatus.Should().Be(expectedIndustryPlacementStatus);

            // Overall results
            _actualResult.OverallResultDetails.Should().Be(expectedOverallReultDetails);
            _actualResult.OverallResultPublishDate.Should().Be(expectedOverallResultPublishDate);

            // PrintCertificate
            var expectedPrintCertificate = _printCertificates.FirstOrDefault(p => p.TqRegistrationPathwayId == expectedPathway.Id && p.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active);

            _actualResult.PrintCertificateId.Should().Be(expectedPrintCertificate?.Id);
            _actualResult.PrintCertificateType.Should().Be(expectedPrintCertificate?.Type);
            _actualResult.ProviderAddress.Should().BeEquivalentTo(expectedProviderAddress);
            _actualResult.LastDocumentRequestedDate.Should().Be(expectedPrintCertificate?.CreatedOn);
            _actualResult.IsReprint.Should().Be(expectedPrintCertificate?.IsReprint);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, false, false, false }, // Invalid Uln

                    new object[] { 1111111111, Provider.BarnsleyCollege, true, false, true }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, false, false, false }, // Uln not from WalsallCollege

                    new object[] { 1111111112, Provider.BarnsleyCollege, true, false, true }, // Withdrawn
                    new object[] { 1111111113, Provider.BarnsleyCollege, false, true, true }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, true, false, true } // Active
                };
            }
        }
    }
}
