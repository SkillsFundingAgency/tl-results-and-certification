using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Certificates;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.CertificateRepositoryTests
{
    public class When_SaveCertificatesPrintingDataAsync_IsCalled : CertificateRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;
        private CertificateDataResponse _actualResult;
        private List<OverallGradeLookup> _overallGradeLookup;
        private DateTime _printAvailableFrom;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active }, // With result
                { 1111111112, RegistrationPathwayStatus.Active }, // With result
                { 1111111113, RegistrationPathwayStatus.Active }, // With result                
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in _registrations)
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Build Pathway results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));

                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessments, isLatestActive, isHistoricAssessent));
            }

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            // Seed Ip
            _ulnWithIndustryPlacements = new Dictionary<long, IndustryPlacementStatus>
            {
                { 1111111111, IndustryPlacementStatus.Completed },
                { 1111111112, IndustryPlacementStatus.NotCompleted },
                { 1111111113, IndustryPlacementStatus.NotCompleted },
            };
            SeedIndustyPlacementData(_ulnWithIndustryPlacements);
            DbContext.SaveChanges();

            // Seed OverallResultLookup
            _overallGradeLookup = new List<OverallGradeLookup>();
            foreach (var uln in _ulns.Select(x => x.Key))
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
                var coreResultId = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
                var splResultId = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 1, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
            }
            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

            _printAvailableFrom = DateTime.Now.Date.AddMonths(7);

            var publishDate = DateTime.Today.AddDays(-1);
            var printAvailableFrom = DateTime.Now.Date.AddMonths(5).AddDays(1);

            // Seed Overall results
            var regPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111111).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = regPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.Certificate, CertificateStatus = CertificateStatus.AwaitingProcessing, PublishDate = publishDate, PrintAvailableFrom = printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

            regPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111112).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = regPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                ResultAwarded = "Partial achievement", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.StatementOfAchievement, CertificateStatus = CertificateStatus.AwaitingProcessing, PublishDate = publishDate, PrintAvailableFrom = printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

            regPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111113).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = regPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                ResultAwarded = "Partial achievement", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.StatementOfAchievement, CertificateStatus = CertificateStatus.AwaitingProcessing, PublishDate = publishDate, PrintAvailableFrom = printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

            DbContext.SaveChanges();

            CertificateRepositoryLogger = new Logger<CertificateRepository>(new NullLoggerFactory());
            CertificateRepository = new CertificateRepository(CertificateRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(Batch batchRequest, List<OverallResult> overallResults)
        {
            if (_actualResult != null)
                return;

            _actualResult = await CertificateRepository.SaveCertificatesPrintingDataAsync(batchRequest, overallResults);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {

            var batchRequest = GetBatchRequest();
            var overallResults = _registrations.SelectMany(x => x.TqRegistrationPathways).SelectMany(x => x.OverallResults).ToList();
            await WhenAsync(batchRequest, overallResults);

            var expectedResponse = new CertificateDataResponse
            {
                IsSuccess = true,
                BatchId = batchRequest.Id,
                TotalBatchRecordsCreated = 5,
                OverallResultsUpdatedCount = 3
            };

            _actualResult.Should().NotBeNull();

            _actualResult.Should().BeEquivalentTo(expectedResponse);

            foreach (var registration in _registrations)
            {
                var expectedPerformedBy = "System";
                var expectedProfile = registration;
                var expectedPathway = expectedProfile.TqRegistrationPathways.FirstOrDefault();
                var expectedProviderAddress = expectedPathway.TqProvider.TlProvider.TlProviderAddresses.FirstOrDefault();
                var expectedPrintCertificate = batchRequest.PrintBatchItems.FirstOrDefault().PrintCertificates.FirstOrDefault(p => p.Uln == registration.UniqueLearnerNumber);

                var expectedLearnerDetails = expectedPrintCertificate.LearningDetails;

                var actualPrintCertificate = DbContext.PrintCertificate.Where(p => p.Uln == expectedProfile.UniqueLearnerNumber && p.TqRegistrationPathwayId == expectedPathway.Id)
                                                                       .Include(p => p.PrintBatchItem)
                                                                            .ThenInclude(p => p.Batch)
                                                                       .OrderByDescending(p => p.CreatedOn).FirstOrDefault();

                var actualOverallResult = actualPrintCertificate.TqRegistrationPathway.OverallResults.FirstOrDefault(o => o.TqRegistrationPathwayId == expectedPathway.Id);

                // Assert PrintCertificate
                actualPrintCertificate.Should().NotBeNull();
                actualPrintCertificate.TqRegistrationPathwayId.Should().Be(expectedPathway.Id);
                actualPrintCertificate.Uln.Should().Be(expectedProfile.UniqueLearnerNumber);
                actualPrintCertificate.LearnerName.Should().Be($"{expectedProfile.Firstname} {expectedProfile.Lastname}");
                actualPrintCertificate.Type.Should().Be(PrintCertificateType.Certificate);
                actualPrintCertificate.LearningDetails.Should().BeEquivalentTo(expectedLearnerDetails);
                actualPrintCertificate.DisplaySnapshot.Should().BeNull();
                actualPrintCertificate.CreatedBy.Should().Be(expectedPerformedBy);

                // Assert PrintBatchItem
                var actualPrintBatchItem = actualPrintCertificate.PrintBatchItem;
                actualPrintBatchItem.Should().NotBeNull();
                actualPrintBatchItem.TlProviderAddressId.Should().Be(expectedProviderAddress.Id);
                actualPrintBatchItem.CreatedBy.Should().Be(expectedPerformedBy);

                // Assert Batch
                var actualBatch = actualPrintBatchItem.Batch;
                actualBatch.Should().NotBeNull();
                actualBatch.Type.Should().Be(BatchType.Printing);
                actualBatch.Status.Should().Be(BatchStatus.Created);
                actualBatch.CreatedBy.Should().Be(expectedPerformedBy);

                // Assert OverallResult
                actualOverallResult.CertificateStatus.Should().Be(CertificateStatus.Processed);
            }
        }

        private Batch GetBatchRequest()
        {
            var printCertificates = new List<PrintCertificate>();
            foreach (var registration in _registrations)
            {
                var pathway = registration.TqRegistrationPathways.FirstOrDefault();
                var overallResult = pathway.OverallResults.FirstOrDefault();
                var overallResultDetail = JsonConvert.DeserializeObject<OverallResultDetail>(overallResult.Details);

                var specialisms = new List<OccupationalSpecialism>();
                if (overallResultDetail.SpecialismDetails == null || !overallResultDetail.SpecialismDetails.Any())
                    specialisms.Add(new OccupationalSpecialism { Specialism = string.Empty, Grade = Constants.NotCompleted });
                else
                {
                    overallResultDetail.SpecialismDetails.ForEach(x =>
                    {
                        specialisms.Add(new OccupationalSpecialism { Specialism = x.SpecialismName, Grade = !string.IsNullOrWhiteSpace(x.SpecialismResult) ? x.SpecialismResult : Constants.NotCompleted });
                    });
                }

                var learningDetails = new LearningDetails
                {
                    TLevelTitle = overallResultDetail.TlevelTitle.Replace(Constants.TLevelIn, string.Empty, StringComparison.InvariantCultureIgnoreCase),
                    Core = overallResultDetail.PathwayName,
                    CoreGrade = !string.IsNullOrWhiteSpace(overallResultDetail.PathwayResult) ? overallResultDetail.PathwayResult : Constants.NotCompleted,
                    OccupationalSpecialism = specialisms,
                    IndustryPlacement = overallResultDetail.IndustryPlacementStatus,
                    Grade = overallResult.ResultAwarded,
                    EnglishAndMaths = "The named recipient has also achieved a qualification at Level 2 in both maths and English.",
                    Date = DateTime.UtcNow.ToCertificateDateFormat(),
                    StartYear = overallResult.TqRegistrationPathway.AcademicYear.ToString()
                };

                printCertificates.Add(new PrintCertificate
                {
                    Uln = registration.UniqueLearnerNumber,
                    LearnerName = $"{registration.Firstname} {registration.Lastname}",
                    TqRegistrationPathwayId = registration.TqRegistrationPathways.FirstOrDefault().Id,
                    Type = PrintCertificateType.Certificate,
                    LearningDetails = JsonConvert.SerializeObject(learningDetails),
                    DisplaySnapshot = null,
                    CreatedBy = "System"
                });
            }

            var provider = _registrations.FirstOrDefault().TqRegistrationPathways.FirstOrDefault().TqProvider.TlProvider;

            var batchItem = new PrintBatchItem
            {
                TlProviderAddressId = provider.TlProviderAddresses.FirstOrDefault().Id,
                PrintCertificates = printCertificates,
                CreatedBy = "System"
            };

            var batch = new Batch
            {
                Type = BatchType.Printing,
                Status = BatchStatus.Created,
                PrintBatchItems = new List<PrintBatchItem> { batchItem },
                CreatedBy = "System",
            };

            return batch;
        }

        public void SeedIndustyPlacementData(Dictionary<long, IndustryPlacementStatus> ipUlns)
        {
            foreach (var ipUln in ipUlns)
            {
                var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == ipUln.Key).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipUln.Value);
            }
        }
    }
}
