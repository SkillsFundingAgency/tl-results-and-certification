using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.StatementOfAchievementServiceTests
{
    public class When_GetSoaLearnerRecordDetails_IsCalled : StatementOfAchievementServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private SoaLearnerRecordDetails _actualResult;
        private List<long> _profilesWithResults;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner, IndustryPlacementStatus ipStatus)> _testCriteriaData;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Withdrawn },
                { 1111111115, RegistrationPathwayStatus.Withdrawn }
            };

            _testCriteriaData = new List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner, IndustryPlacementStatus ipStatus)>
            {
                (1111111111, false, true, true, true, true, true, IndustryPlacementStatus.Completed), // Lrs data with Send Qualification + IP
                (1111111112, true, false, false, false, false, null, IndustryPlacementStatus.NotSpecified), // Not from Lrs + No IP
                (1111111113, false, true, false, true, false, false, IndustryPlacementStatus.NotSpecified), // Lrs data without Send Qualification
                (1111111114, true, false, false, true, true, null, IndustryPlacementStatus.CompletedWithSpecialConsideration), // Not from Lrs + IP
                (1111111115, true, false, false, true, true, null, IndustryPlacementStatus.NotCompleted) // Not from Lrs + IP (Not Completed)
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            Qualifications = SeedQualificationData();

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            // Seed Assessments And Results
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();

            _profilesWithResults = new List<long> { 1111111111, 1111111112 };
            foreach (var profile in _profiles.Where(x => _profilesWithResults.Contains(x.UniqueLearnerNumber)))
            {
                var isLatestActive = _ulns[profile.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(profile.TqRegistrationPathways.ToList(), isLatestActive);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Seed Pathway results
                foreach (var assessment in pathwayAssessments)
                {
                    var hasHitoricData = new List<long> { 1111111112 };
                    var isHistoricAssessent = hasHitoricData.Any(x => x == profile.UniqueLearnerNumber);
                    var isLatestActiveResult = !isHistoricAssessent;

                    var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, isLatestActiveResult, isHistoricAssessent);
                    tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                }
            }

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, true);

            foreach (var (uln, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner, ipStatus) in _testCriteriaData)
            {
                var profile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                BuildLearnerRecordCriteria(profile, isRcFeed, seedQualificationAchieved, isSendQualification, isEngishAndMathsAchieved, seedIndustryPlacement, isSendLearner, ipStatus);
            }

            TransferRegistration(1111111113, Provider.WalsallCollege);

            StatementOfAchievementRepositoryLogger = new Logger<StatementOfAchievementRepository>(new NullLoggerFactory());
            StatementOfAchievementRepository = new StatementOfAchievementRepository(DbContext, StatementOfAchievementRepositoryLogger);
            StatementOfAchievementServiceLogger = new Logger<StatementOfAchievementService>(new NullLoggerFactory());

            BatchRepositoryLogger = new Logger<GenericRepository<Batch>>(new NullLoggerFactory());
            BatchRepository = new GenericRepository<Batch>(BatchRepositoryLogger, DbContext);

            StatementOfAchievementService = new StatementOfAchievementService(StatementOfAchievementRepository, BatchRepository, TrainingProviderMapper, StatementOfAchievementServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long providerUkprn, long uln)
        {
            if (_actualResult != null)
                return;

            var profileId = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln)?.Id ?? 0;

            _actualResult = await StatementOfAchievementService.GetSoaLearnerRecordDetailsAsync(providerUkprn, profileId);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long uln, Provider provider, RegistrationPathwayStatus expectedStatus, bool isRecordFound)
        {
            await WhenAsync((long)provider, uln);

            if (isRecordFound == false)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            expectedProfile.Should().NotBeNull();

            var expectedProvider = TlProviders.FirstOrDefault(p => p.UkPrn == (long)provider);
            var expectedProviderName = expectedProvider != null ? expectedProvider.Name : null;
            var expectedProviderUkprn = expectedProvider != null ? expectedProvider.UkPrn : (long?)null;
            var expectedTlevelTitle = Pathway.TlevelTitle;
            var expectedIsLearnerRegistered = expectedStatus == RegistrationPathwayStatus.Active || expectedStatus == RegistrationPathwayStatus.Withdrawn;
            var expecedIpStatus = _testCriteriaData.FirstOrDefault(x => x.uln == expectedProfile.UniqueLearnerNumber).ipStatus;
            var expectedIsIndustryPlacementCompleted = expecedIpStatus == IndustryPlacementStatus.Completed || expecedIpStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration;
            var expectedHasResult = _profilesWithResults.Contains(expectedProfile.UniqueLearnerNumber);
            var expectedHasLrsEnglishAndMaths = expectedProfile.IsRcFeed == false && expectedProfile.QualificationAchieved.Any();
            var providerAddress = expectedProvider?.TlProviderAddresses?.OrderByDescending(ad => ad.CreatedOn)?.FirstOrDefault();
            var expectedProviderAddress = new Address { OrganisationName = providerAddress?.OrganisationName, DepartmentName = providerAddress?.DepartmentName, AddressLine1 = providerAddress?.AddressLine1, AddressLine2 = providerAddress?.AddressLine2, Town = providerAddress?.Town, Postcode = providerAddress?.Postcode };

            var expectedPathway = (expectedStatus == RegistrationPathwayStatus.Withdrawn || expectedStatus == RegistrationPathwayStatus.Transferred)
                ? expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == expectedStatus && p.EndDate != null)
                : expectedProfile.TqRegistrationPathways.FirstOrDefault(p => p.Status == expectedStatus && p.EndDate == null);

            expectedPathway.Should().NotBeNull();

            var expectedSpecialim = (expectedStatus == RegistrationPathwayStatus.Withdrawn || expectedStatus == RegistrationPathwayStatus.Transferred)
                ? expectedPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin && s.EndDate != null)
                : expectedPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin && s.EndDate == null);

            var expectedPathwayAssessment = (expectedStatus == RegistrationPathwayStatus.Withdrawn || expectedStatus == RegistrationPathwayStatus.Transferred)
                ? expectedPathway.TqPathwayAssessments.FirstOrDefault(x => x.IsOptedin && x.EndDate != null)
                : expectedPathway.TqPathwayAssessments.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);

            var expectedPathwayResult = (expectedStatus == RegistrationPathwayStatus.Withdrawn || expectedStatus == RegistrationPathwayStatus.Transferred)
                ? (expectedPathwayAssessment?.TqPathwayResults?.FirstOrDefault(x => x.IsOptedin && x.EndDate != null))
                : (expectedPathwayAssessment?.TqPathwayResults?.FirstOrDefault(x => x.IsOptedin && x.EndDate == null));

            var expectedPathwayGrade = expectedPathwayResult?.TlLookup.Value;

            _actualResult.Should().NotBeNull();
            _actualResult.ProfileId.Should().Be(expectedProfile.Id);
            _actualResult.Uln.Should().Be(uln);
            _actualResult.Firstname.Should().Be(expectedProfile.Firstname);
            _actualResult.Lastname.Should().Be(expectedProfile.Lastname);
            _actualResult.DateofBirth.Should().Be(expectedProfile.DateofBirth);
            _actualResult.ProviderName.Should().Be(expectedProviderName);
            _actualResult.ProviderUkprn.Should().Be(expectedProviderUkprn);
            _actualResult.TlevelTitle.Should().Be(expectedTlevelTitle);

            _actualResult.PathwayName.Should().Be(expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.Name);
            _actualResult.PathwayCode.Should().Be(expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId);
            _actualResult.PathwayGrade.Should().Be(expectedPathwayGrade);
            _actualResult.SpecialismName.Should().Be(expectedSpecialim.TlSpecialism.Name);
            _actualResult.SpecialismCode.Should().Be(expectedSpecialim.TlSpecialism.LarId);
            _actualResult.SpecialismGrade.Should().BeNull();

            _actualResult.IsEnglishAndMathsAchieved.Should().Be(expectedProfile.IsEnglishAndMathsAchieved ?? false);
            _actualResult.IsSendLearner.Should().Be(expectedProfile.IsSendLearner);
            _actualResult.HasLrsEnglishAndMaths.Should().Be(expectedHasLrsEnglishAndMaths);
            _actualResult.IndustryPlacementStatus.Should().Be(expecedIpStatus);
            _actualResult.ProviderAddress.Should().BeEquivalentTo(expectedProviderAddress);
            _actualResult.Status.Should().Be(expectedStatus);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 9999999999, Provider.WalsallCollege, null, false }, // Invalid Uln
                    new object[] { 1111111111, Provider.BarsleyCollege, RegistrationPathwayStatus.Active, true }, // Active
                    new object[] { 1111111111, Provider.WalsallCollege, RegistrationPathwayStatus.Active, false }, // Uln not from WalsallCollege
                    new object[] { 1111111112, Provider.BarsleyCollege, RegistrationPathwayStatus.Active, true }, // Active
                    new object[] { 1111111113, Provider.BarsleyCollege, RegistrationPathwayStatus.Transferred, true }, // Transferred
                    new object[] { 1111111113, Provider.WalsallCollege, RegistrationPathwayStatus.Active, true }, // Active
                    new object[] { 1111111114, Provider.BarsleyCollege, RegistrationPathwayStatus.Withdrawn, true }, // Withdrawn
                    new object[] { 1111111115, Provider.BarsleyCollege, RegistrationPathwayStatus.Withdrawn, true } // Withdrawn
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

            var industryPlacement = profile.TqRegistrationPathways.FirstOrDefault()?.IndustryPlacements?.FirstOrDefault();
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, profile, transferToTqProvider);
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (industryPlacement != null)
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, tqRegistrationPathway.Id, industryPlacement.Status);

            DbContext.SaveChanges();
        }
    }
}
