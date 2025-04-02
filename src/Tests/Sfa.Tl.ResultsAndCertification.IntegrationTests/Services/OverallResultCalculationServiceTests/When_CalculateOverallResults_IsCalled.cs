//using FluentAssertions;
//using Sfa.Tl.ResultsAndCertification.Application.Services;
//using Sfa.Tl.ResultsAndCertification.Common.Enum;
//using Sfa.Tl.ResultsAndCertification.Domain.Models;
//using Sfa.Tl.ResultsAndCertification.Models.Functions;
//using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
//using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
//{
//    public class When_CalculateOverallResults_IsCalled : OverallResultCalculationServiceBaseTest
//    {
//        private Dictionary<long, RegistrationPathwayStatus> _ulns;
//        private List<TqRegistrationProfile> _registrations;
//        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;
//        private List<OverallResultResponse> _actualResult;
//        private List<long> _ulnsAlreadyWithCalculatedResult;
//        private List<OverallResultResponse> _expectedResult;
//        private List<OverallGradeLookup> _overallGradeLookup;
//        private List<OverallResult> _expectedOverallResults;
//        private DateTime _printAvailableFrom;

//        public override void Given()
//        {
//            _ulns = new Dictionary<long, RegistrationPathwayStatus>
//            {
//                { 1111111111, RegistrationPathwayStatus.Active }, // Without result
//                { 1111111112, RegistrationPathwayStatus.Active }, // With result
//                { 1111111113, RegistrationPathwayStatus.Active }, // With result + Already Calculated OverallResult - Same Outcome
//                { 1111111114, RegistrationPathwayStatus.Active }, // With result + Already Calculated OverallResult - Different Outcome
//                { 1111111115, RegistrationPathwayStatus.Active }, // With result
//                { 1111111116, RegistrationPathwayStatus.Active }, // With result
//                { 1111111117, RegistrationPathwayStatus.Active }, // With result
//                { 1111111118, RegistrationPathwayStatus.Active }, // With result
//                { 1111111119, RegistrationPathwayStatus.Active }  // With result
//            };

//            SeedTestData(EnumAwardingOrganisation.Pearson, true);
//            _registrations = SeedRegistrationsData(_ulns, null);

//            // Assessments seed
//            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
//            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
//            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
//            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

//            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
//            {
//                var hasHitoricData = new List<long> { 1111111112 };
//                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
//                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

//                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
//                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

//                // Build Pathway results
//                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));

//                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);
//                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

//                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismAssessments, isLatestActive, isHistoricAssessent));
//            }

//            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
//            SeedPathwayResultsData(tqPathwayResultsSeedData, false);

//            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
//            SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

//            // Seed Ip
//            _ulnWithIndustryPlacements = new Dictionary<long, IndustryPlacementStatus>
//            {
//                { 1111111112, IndustryPlacementStatus.NotCompleted },
//                { 1111111113, IndustryPlacementStatus.Completed },
//                { 1111111114, IndustryPlacementStatus.NotCompleted },
//                { 1111111117, IndustryPlacementStatus.NotCompleted },
//                { 1111111118, IndustryPlacementStatus.NotCompleted },
//                { 1111111119, IndustryPlacementStatus.NotCompleted }
//            };
//            SeedIndustyPlacementData(_ulnWithIndustryPlacements);
//            DbContext.SaveChanges();

//            // Seed OverallResultLookup
//            _ulnsAlreadyWithCalculatedResult = new List<long> { 1111111113, 1111111114 };
//            _overallGradeLookup = new List<OverallGradeLookup>();
//            foreach (var uln in _ulnsAlreadyWithCalculatedResult)
//            {
//                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
//                var coreResultId = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
//                var splResultId = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
//                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 1, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
//            }
//            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

//            _printAvailableFrom = DateTime.Now.Date.AddMonths(7);

//            // Seed Overall results
//            var sameResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111113).TqRegistrationPathways.FirstOrDefault().Id;
//            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = sameResultPathwayId,
//                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
//                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.Certificate, CertificateStatus = CertificateStatus.AwaitingProcessing, PrintAvailableFrom = _printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

//            var differentCalcResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114).TqRegistrationPathways.FirstOrDefault().Id;
//            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = differentCalcResultPathwayId,
//                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
//                ResultAwarded = "Partial achievement", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.StatementOfAchievement, CertificateStatus = CertificateStatus.AwaitingProcessing, PrintAvailableFrom = _printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

//            DbContext.SaveChanges();

//            // Change pathway result and Industry placement status
//            ChangePathwayResultAndIPStatus(1111111114, "B");
//            ChangePathwayResultAndIPStatus(1111111117, "Q - pending result");
//            ChangePathwayResultAndIPStatus(1111111118, "X - no result", false);
//            ChangeSpecialismResult(1111111118, "X - no result");

//            ChangePathwayResultAndIPStatus(1111111119, "Unclassified", false);
//            ChangeSpecialismResult(1111111119, "X - no result");

//            CreateService();
//        }

//        private void ChangePathwayResultAndIPStatus(long uln, string newPathwayGrade, bool changeIPStatus = true)
//        {
//            var tlLookup = TlLookup.FirstOrDefault(l => l.Category.Equals(LookupCategory.PathwayComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && l.Value == newPathwayGrade);
//            var pathwayToChange = DbContext.TqRegistrationPathway.FirstOrDefault(p => p.TqRegistrationProfile.UniqueLearnerNumber == uln);
//            var pathwayAssessment = pathwayToChange.TqPathwayAssessments.FirstOrDefault(a => a.IsOptedin && a.EndDate == null);
//            var pathwayResult = pathwayAssessment.TqPathwayResults.FirstOrDefault(r => r.IsOptedin && r.EndDate == null);

//            pathwayResult.EndDate = DateTime.Now;
//            pathwayResult.IsOptedin = false;
//            pathwayResult.ModifiedOn = DateTime.Now;
//            pathwayResult.ModifiedBy = "System";

//            pathwayAssessment.TqPathwayResults.Add(new TqPathwayResult
//            {
//                TqPathwayAssessmentId = pathwayAssessment.Id,
//                TlLookupId = tlLookup.Id,
//                IsOptedin = true,
//                StartDate = DateTime.Now.AddMinutes(3),
//                EndDate = null,
//                IsBulkUpload = false,
//                CreatedBy = "System"
//            });

//            if (changeIPStatus)
//            {
//                // update Ip Status to CompletedToSpecialConsideration

//                var ipToChange = pathwayToChange.IndustryPlacements.FirstOrDefault();
//                ipToChange.Status = IndustryPlacementStatus.CompletedWithSpecialConsideration;
//            }

//            DbContext.SaveChanges();
//        }

//        private void ChangeSpecialismResult(long uln, string newGrade)
//        {
//            var tlLookup = TlLookup.FirstOrDefault(l => l.Category.Equals(LookupCategory.SpecialismComponentGrade.ToString(), StringComparison.InvariantCultureIgnoreCase) && l.Value == newGrade);
//            var pathwayToChange = DbContext.TqRegistrationPathway.FirstOrDefault(p => p.TqRegistrationProfile.UniqueLearnerNumber == uln);
//            var specialismAssessment = pathwayToChange.TqRegistrationSpecialisms.SelectMany(sa => sa.TqSpecialismAssessments).FirstOrDefault(a => a.IsOptedin && a.EndDate == null);
//            var speicalismResult = specialismAssessment.TqSpecialismResults.FirstOrDefault(sr => sr.IsOptedin && sr.EndDate == null);

//            speicalismResult.EndDate = DateTime.Now;
//            speicalismResult.IsOptedin = false;
//            speicalismResult.ModifiedOn = DateTime.Now;
//            speicalismResult.ModifiedBy = "System";

//            specialismAssessment.TqSpecialismResults.Add(new TqSpecialismResult
//            {
//                TqSpecialismAssessmentId = specialismAssessment.Id,
//                TlLookupId = tlLookup.Id,
//                IsOptedin = true,
//                StartDate = DateTime.Now.AddMinutes(3),
//                EndDate = null,
//                IsBulkUpload = false,
//                CreatedBy = "System"
//            });           

//            DbContext.SaveChanges();
//        }

//        public override Task When()
//        {
//            return Task.CompletedTask;
//        }

//        public async Task WhenAsync(DateTime runDate)
//        {
//            var assessmentSeries = await OverallResultCalculationService.GetPreviousAssessmentSeriesAsync(runDate);

//            _expectedOverallResults = new List<OverallResult>
//            {
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 1,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":null,\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":null}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"X - no result\"}",
//                    CalculationStatus = CalculationStatus.NoResult,
//                    ResultAwarded = "X - no result",
//                    SpecialismResultAwarded = "X - no result",
//                    PrintAvailableFrom = null,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = null,
//                    CertificateStatus = null
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 2,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
//                    CalculationStatus = CalculationStatus.PartiallyCompleted,
//                    ResultAwarded = "Partial achievement",
//                    SpecialismResultAwarded = "X - no result",
//                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = PrintCertificateType.StatementOfAchievement,
//                    CertificateStatus = CertificateStatus.AwaitingProcessing
//                },                
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 4,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
//                    CalculationStatus = CalculationStatus.Completed,
//                    ResultAwarded = "Partial achievement",
//                    SpecialismResultAwarded = "X - no result",
//                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
//                    PublishDate = null,
//                    EndDate = DateTime.UtcNow,
//                    IsOptedin = false,
//                    CertificateType = PrintCertificateType.StatementOfAchievement,
//                    CertificateStatus = CertificateStatus.AwaitingProcessing
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 4,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Distinction*\"}",
//                    CalculationStatus = CalculationStatus.Completed,
//                    ResultAwarded = "Distinction*",
//                    SpecialismResultAwarded = "Distinction",
//                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = PrintCertificateType.Certificate,
//                    CertificateStatus = CertificateStatus.AwaitingProcessing
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 5,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
//                    CalculationStatus = CalculationStatus.PartiallyCompleted,
//                    ResultAwarded = "Partial achievement",
//                    SpecialismResultAwarded = "X - no result",
//                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = PrintCertificateType.StatementOfAchievement,
//                    CertificateStatus = CertificateStatus.AwaitingProcessing
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 6,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
//                    CalculationStatus = CalculationStatus.PartiallyCompleted,
//                    ResultAwarded = "Partial achievement",
//                    SpecialismResultAwarded = null,
//                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = PrintCertificateType.StatementOfAchievement,
//                    CertificateStatus = CertificateStatus.AwaitingProcessing
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 7,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"Q - pending result\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Q - pending result\"}",
//                    CalculationStatus = CalculationStatus.Qpending,
//                    ResultAwarded = "Q - pending result",
//                    SpecialismResultAwarded = "Q - pending result",
//                    PrintAvailableFrom = null,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = null,
//                    CertificateStatus = null
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 8,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"X - no result\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"X - no result\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"X - no result\"}",
//                    CalculationStatus = CalculationStatus.NoResult,
//                    ResultAwarded = "X - no result",
//                    SpecialismResultAwarded = "X - no result",
//                    PrintAvailableFrom = null,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = null,
//                    CertificateStatus = null
//                },
//                new OverallResult
//                {
//                    TqRegistrationPathwayId = 9,
//                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"Unclassified\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"X - no result\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Unclassified\"}",
//                    CalculationStatus = CalculationStatus.Unclassified,
//                    ResultAwarded = "Unclassified",
//                    SpecialismResultAwarded = "Unclassified",
//                    PrintAvailableFrom = null,
//                    PublishDate = assessmentSeries.ResultPublishDate,
//                    EndDate = null,
//                    IsOptedin = true,
//                    CertificateType = null,
//                    CertificateStatus = null
//                }
//            };

//            _expectedResult = new List<OverallResultResponse>
//            {
//                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 2, UpdatedRecords = 0, UnChangedRecords = 0, SavedRecords = 2 },
//                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 1, UpdatedRecords = 1, UnChangedRecords = 0, SavedRecords = 3 },
//                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 2, UpdatedRecords = 0, UnChangedRecords = 0, SavedRecords = 2 },
//                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 2, UpdatedRecords = 0, UnChangedRecords = 0, SavedRecords = 2 },
//            };

//            _actualResult = await OverallResultCalculationService.CalculateOverallResultsAsync(runDate);
//        }

//        [Theory]
//        [MemberData(nameof(Data))]
//        public async Task Then_Expected_Results_Are_Returned(DateTime runDate)
//        {
//            await WhenAsync(runDate);
//            _actualResult.Should().BeEquivalentTo(_expectedResult);

//            var newOverallResultUlns = new List<long> { 1111111111, 1111111112, 1111111114, 1111111115, 1111111116, 1111111117 };
//            foreach (var uln in newOverallResultUlns)
//            {
//                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
//                var actualOverallResult = DbContext.OverallResult.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId && x.IsOptedin && x.EndDate == null);
//                var expectedOverallResult = _expectedOverallResults.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId && x.IsOptedin && x.EndDate == null);

//                AssertOverallResult(actualOverallResult, expectedOverallResult);
//            }

//            var prevOverallResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114).TqRegistrationPathways.FirstOrDefault().Id;
//            var prevOverallResult = DbContext.OverallResult.FirstOrDefault(x => x.TqRegistrationPathwayId == prevOverallResultPathwayId && x.EndDate != null);
//            var expectedPrevOverallResult = _expectedOverallResults.FirstOrDefault(x => x.TqRegistrationPathwayId == prevOverallResultPathwayId && x.EndDate != null);
//            AssertOverallResult(prevOverallResult, expectedPrevOverallResult);
//        }

//        public static IEnumerable<object[]> Data
//        {
//            get
//            {
//                return new[]
//                {
//                    new object[] { DateTime.Today.AddMonths(4) }
//                };
//            }
//        }

//        public void SeedIndustyPlacementData(Dictionary<long, IndustryPlacementStatus> ipUlns)
//        {
//            foreach (var ipUln in ipUlns)
//            {
//                var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == ipUln.Key).TqRegistrationPathways.FirstOrDefault();
//                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipUln.Value);
//            }
//        }
//    }
//}
