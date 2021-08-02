using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.StatementOfAchievementServiceTests
{
    public class When_CreateSoaPrintingRequest_IsCalled : StatementOfAchievementServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private IList<TqRegistrationProfile> _profiles;
        private SoaPrintingResponse _actualResult;
        private List<long> _profilesWithResults;
        private List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner, IndustryPlacementStatus ipStatus)> _testCriteriaData;

        public override void Given()
        {
            _profiles = new List<TqRegistrationProfile>();
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Withdrawn }                
            };

            _testCriteriaData = new List<(long uln, bool isRcFeed, bool seedQualificationAchieved, bool isSendQualification, bool isEngishAndMathsAchieved, bool seedIndustryPlacement, bool? isSendLearner, IndustryPlacementStatus ipStatus)>
            {
                (1111111111, false, true, true, true, true, true, IndustryPlacementStatus.Completed), // Lrs data with Send Qualification + IP
                (1111111112, true, false, false, true, true, null, IndustryPlacementStatus.CompletedWithSpecialConsideration), // Not from Lrs + IP
                (1111111113, true, false, false, true, true, null, IndustryPlacementStatus.NotCompleted) // Not from Lrs + IP (Not Completed)
            };

            // Create Mapper
            CreateMapper();

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

        public async Task WhenAsync(SoaPrintingRequest soaPrintingRequest)
        {
            if (_actualResult != null)
                return;
            
            _actualResult = await StatementOfAchievementService.CreateSoaPrintingRequestAsync(soaPrintingRequest);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long providerUkprn, long uln, string performedBy, SoaPrintingResponse expectedResult)
        {
            var profile = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
            var providerAddress = TlProviders.FirstOrDefault(p => p.UkPrn == providerUkprn)?.TlProviderAddresses.FirstOrDefault();

            var printRequest = GetSoaPrintingRequest(profile, providerAddress, performedBy);

            await WhenAsync(printRequest);

            if (expectedResult == null)
            {
                _actualResult.Should().BeNull();
                return;
            }

            var expectedProfile = _profiles.FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var expectedPathway = expectedProfile.TqRegistrationPathways.FirstOrDefault();

            var expectedLearnerDetails = JsonConvert.SerializeObject(printRequest.LearningDetails);
            var expectedSoaPrintingDetails = JsonConvert.SerializeObject(printRequest.SoaPrintingDetails);

            expectedProfile.Should().NotBeNull();
            _actualResult.Should().NotBeNull();
            _actualResult.IsSuccess.Should().Be(expectedResult.IsSuccess);

            if (expectedResult.IsSuccess)
            {
                _actualResult.Uln.Should().Be(expectedResult.Uln);
                _actualResult.LearnerName.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");

                var actualPrintCertificate = DbContext.PrintCertificate.Where(p => p.Uln == expectedProfile.UniqueLearnerNumber && p.TqRegistrationPathwayId == expectedPathway.Id)
                                                                   .Include(p => p.PrintBatchItem)
                                                                        .ThenInclude(p => p.Batch)
                                                                   .OrderByDescending(p => p.CreatedOn).FirstOrDefault();

                // Assert PrintCertificate
                actualPrintCertificate.Should().NotBeNull();                
                actualPrintCertificate.TqRegistrationPathwayId.Should().Be(expectedPathway.Id);
                actualPrintCertificate.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
                actualPrintCertificate.LearnerName.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
                actualPrintCertificate.Type.Should().Be(PrintCertificateType.StatementOfAchievement);
                actualPrintCertificate.LearningDetails.Should().BeEquivalentTo(expectedLearnerDetails);
                actualPrintCertificate.DisplaySnapshot.Should().BeEquivalentTo(expectedSoaPrintingDetails);
                actualPrintCertificate.CreatedBy.Should().Be(performedBy);

                // Assert PrintBatchItem
                var actualPrintBatchItem = actualPrintCertificate.PrintBatchItem;
                actualPrintBatchItem.Should().NotBeNull();
                actualPrintBatchItem.TlProviderAddressId.Should().Be(providerAddress.Id);
                actualPrintBatchItem.CreatedBy.Should().Be(performedBy);

                // Assert Batch
                var actualBatch = actualPrintBatchItem.Batch;
                actualBatch.Should().NotBeNull();
                actualBatch.Type.Should().Be(BatchType.Printing);
                actualBatch.Status.Should().Be(BatchStatus.Created);
                actualBatch.CreatedBy.Should().Be(performedBy);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {                     
                    new object[] {  Provider.BarsleyCollege, 1111111111, "Test Provider", new SoaPrintingResponse { Uln = 1111111111, IsSuccess = true } },
                    new object[] {  Provider.BarsleyCollege, 1111111112, "Test Provider", new SoaPrintingResponse { Uln = 1111111112, IsSuccess = true } },
                    new object[] {  Provider.BarsleyCollege, 1111111113, "Test Provider", new SoaPrintingResponse { Uln = 1111111113, IsSuccess = true } },
                };
            }
        }

        private SoaPrintingRequest GetSoaPrintingRequest(TqRegistrationProfile profile, TlProviderAddress providerAddress, string performedBy)
        {
            var pathway = profile.TqRegistrationPathways.FirstOrDefault();
            var pathwayResult = pathway.TqPathwayAssessments.FirstOrDefault()?.TqPathwayResults.FirstOrDefault();
            var specialism = pathway.TqRegistrationSpecialisms.FirstOrDefault();            
            var industryPlacement = pathway.IndustryPlacements.FirstOrDefault();

            var request = new SoaPrintingRequest
            {
                AddressId = providerAddress.Id,
                RegistrationPathwayId = pathway.Id,
                Uln = profile.UniqueLearnerNumber,
                LearnerName = $"{profile.Firstname} {profile.Lastname}",
                LearningDetails = new LearningDetails
                {
                    TLevelTitle = pathway.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                    Grade = null,
                    Date = DateTime.UtcNow.ToSoaFormat(),
                    Core = pathway.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    CoreGrade = pathwayResult?.TlLookup.Value,
                    OccupationalSpecialism = new List<OccupationalSpecialismDetails>
                    {
                        new OccupationalSpecialismDetails
                        {
                            Specialism = specialism.TlSpecialism.Name,
                            Grade = null
                        }
                    },
                    IndustryPlacement = industryPlacement.Status == IndustryPlacementStatus.Completed || industryPlacement.Status == IndustryPlacementStatus.CompletedWithSpecialConsideration ? Constants.IndustryPlacementCompleted : Constants.IndustryPlacementNotCompleted,
                    EnglishAndMaths = profile.IsEnglishAndMathsAchieved == true ? Constants.EnglishAndMathsMet : Constants.EnglishAndMathsNotMet
                },
                SoaPrintingDetails = new SoaPrintingDetails
                {
                    Uln = profile.UniqueLearnerNumber,
                    Name = $"{profile.Firstname} {profile.Lastname}",
                    Dateofbirth = profile.DateofBirth.ToDobFormat(),
                    ProviderName = $"{pathway.TqProvider.TlProvider.Name} ({pathway.TqProvider.TlProvider.UkPrn})",
                    TlevelTitle = pathway.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                    Core = $"{pathway.TqProvider.TqAwardingOrganisation.TlPathway.Name} ({pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId})",
                    CoreGrade = pathwayResult?.TlLookup.Value,
                    Specialism = $"{specialism.TlSpecialism.Name} ({specialism.TlSpecialism.LarId})",
                    SpecialismGrade = null,
                    EnglishAndMaths = "Achieved minimum standard",
                    IndustryPlacement = "Not completed",
                    ProviderAddress = new Address
                    {
                        DepartmentName = providerAddress.DepartmentName,
                        OrganisationName = providerAddress.OrganisationName,
                        AddressLine1 = providerAddress.AddressLine1,
                        AddressLine2 = providerAddress.AddressLine2,
                        Town = providerAddress.Town,
                        Postcode = providerAddress.Postcode
                    }
                },
                PerformedBy = performedBy
            };
            return request;
        }
    }
}
