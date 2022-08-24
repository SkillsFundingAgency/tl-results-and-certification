using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_CalculateOverallResults_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private Dictionary<long, IndustryPlacementStatus> _ulnWithIndustryPlacements;
        private List<OverallResultResponse> _actualResult;
        private List<long> _ulnsAlreadyWithCalculatedResult;
        private List<OverallResultResponse> _expectedResult;
        private List<OverallGradeLookup> _overallGradeLookup;
        private List<OverallResult> _expectedOverallResults;
        private DateTime _printAvailableFrom;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active }, // Without result
                { 1111111112, RegistrationPathwayStatus.Active }, // With result
                { 1111111113, RegistrationPathwayStatus.Active }, // With result + Already Calculated OverallResult - Same Outcome
                { 1111111114, RegistrationPathwayStatus.Active }, // With result + Already Calculated OverallResult - Different Outcome
                { 1111111115, RegistrationPathwayStatus.Active }, // With result
                { 1111111116, RegistrationPathwayStatus.Active }, // With result
            };

            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, null);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
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
                { 1111111112, IndustryPlacementStatus.NotCompleted },
                { 1111111113, IndustryPlacementStatus.Completed },
                { 1111111114, IndustryPlacementStatus.NotCompleted },
            };
            SeedIndustyPlacementData(_ulnWithIndustryPlacements);
            DbContext.SaveChanges();

            // Seed OverallResultLookup
            _ulnsAlreadyWithCalculatedResult = new List<long> { 1111111113, 1111111114 };
            _overallGradeLookup = new List<OverallGradeLookup>();
            foreach (var uln in _ulnsAlreadyWithCalculatedResult)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
                var coreResultId = DbContext.TqPathwayResult.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
                var splResultId = DbContext.TqSpecialismResult.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 1, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
            }
            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);

            _printAvailableFrom = DateTime.Now.Date.AddMonths(5).AddDays(1);

            // Seed Overall results
            var sameResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111113).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = sameResultPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.Certificate, PrintAvailableFrom = _printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

            var differentCalcResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114).TqRegistrationPathways.FirstOrDefault().Id;
            OverallResultDataProvider.CreateOverallResult(DbContext, new List<OverallResult> { new OverallResult { TqRegistrationPathwayId = differentCalcResultPathwayId,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                ResultAwarded = "Partial achievement", CalculationStatus = CalculationStatus.Completed, IsOptedin = true, CertificateType = PrintCertificateType.StatementOfAchievement, PrintAvailableFrom = _printAvailableFrom, StartDate = DateTime.Now, CreatedOn = DateTime.Now } }, true);

            DbContext.SaveChanges();

            // Change pathway result and Industry placement status
            ChangePathwayResultAndIPStatus(1111111114, "B");

            CreateService();
        }

        private void ChangePathwayResultAndIPStatus(long uln, string newPathwayGrade)
        {
            var tlLookup = TlLookup.FirstOrDefault(l => l.Value == newPathwayGrade);
            var pathwayToChange = DbContext.TqRegistrationPathway.FirstOrDefault(p => p.TqRegistrationProfile.UniqueLearnerNumber == 1111111114);
            var pathwayAssessment = pathwayToChange.TqPathwayAssessments.FirstOrDefault(a => a.IsOptedin && a.EndDate == null);
            var pathwayResult = pathwayAssessment.TqPathwayResults.FirstOrDefault(r => r.IsOptedin && r.EndDate == null);

            pathwayResult.EndDate = DateTime.Now;
            pathwayResult.IsOptedin = false;
            pathwayResult.ModifiedOn = DateTime.Now;
            pathwayResult.ModifiedBy = "System";

            pathwayAssessment.TqPathwayResults.Add(new TqPathwayResult
            {
                TqPathwayAssessmentId = pathwayAssessment.Id,
                TlLookupId = tlLookup.Id,
                IsOptedin = true,
                StartDate = DateTime.Now.AddMinutes(3),
                EndDate = null,
                IsBulkUpload = false,
                CreatedBy = "System"
            });

            // update Ip Status to CompletedToSpecialConsideration

            var ipToChange = pathwayToChange.IndustryPlacements.FirstOrDefault();
            ipToChange.Status = IndustryPlacementStatus.CompletedWithSpecialConsideration;

            DbContext.SaveChanges();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(DateTime runDate)
        {
            var assessmentSeries = await OverallResultCalculationService.GetResultCalculationAssessmentAsync(runDate);

            _expectedOverallResults = new List<OverallResult>
            {
                new OverallResult
                {
                    TqRegistrationPathwayId = 1,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":null,\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":null}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"X - no result\"}",
                    CalculationStatus = CalculationStatus.NoResult,
                    ResultAwarded = "X - no result",
                    PrintAvailableFrom = null,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = null,
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 2,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.StatementOfAchievement,
                },                
                new OverallResult
                {
                    TqRegistrationPathwayId = 4,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.Completed,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
                    PublishDate = null,
                    EndDate = DateTime.UtcNow,
                    IsOptedin = false,
                    CertificateType = PrintCertificateType.StatementOfAchievement,
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 4,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Distinction*\"}",
                    CalculationStatus = CalculationStatus.Completed,
                    ResultAwarded = "Distinction*",
                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.Certificate,
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 5,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.StatementOfAchievement,
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = 6,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Partial achievement\"}",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    ResultAwarded = "Partial achievement",
                    PrintAvailableFrom = assessmentSeries.PrintAvailableDate,
                    PublishDate = assessmentSeries.ResultPublishDate,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.StatementOfAchievement,
                }
            };

            _expectedResult = new List<OverallResultResponse>
            {
                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 2, UpdatedRecords = 0, UnChangedRecords = 0, SavedRecords = 2 },
                new OverallResultResponse { IsSuccess = true, TotalRecords = 2, NewRecords = 1, UpdatedRecords = 1, UnChangedRecords = 0, SavedRecords = 3 },
                new OverallResultResponse { IsSuccess = true, TotalRecords = 1, NewRecords = 1, UpdatedRecords = 0, UnChangedRecords = 0, SavedRecords = 1 }
            };

            _actualResult = await OverallResultCalculationService.CalculateOverallResultsAsync(runDate);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(DateTime runDate)
        {
            await WhenAsync(runDate);
            _actualResult.Should().BeEquivalentTo(_expectedResult);

            var newOverallResultUlns = new List<long> { 1111111111, 1111111112, 1111111114, 1111111115, 1111111116 };
            foreach (var uln in newOverallResultUlns)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault().Id;
                var actualOverallResult = DbContext.OverallResult.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId && x.IsOptedin && x.EndDate == null);
                var expectedOverallResult = _expectedOverallResults.FirstOrDefault(x => x.TqRegistrationPathwayId == pathwayId && x.IsOptedin && x.EndDate == null);

                AssertOverallResult(actualOverallResult, expectedOverallResult);
            }

            var prevOverallResultPathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111114).TqRegistrationPathways.FirstOrDefault().Id;
            var prevOverallResult = DbContext.OverallResult.FirstOrDefault(x => x.TqRegistrationPathwayId == prevOverallResultPathwayId && x.EndDate != null);
            var expectedPrevOverallResult = _expectedOverallResults.FirstOrDefault(x => x.TqRegistrationPathwayId == prevOverallResultPathwayId && x.EndDate != null);
            AssertOverallResult(prevOverallResult, expectedPrevOverallResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { DateTime.Today.AddMonths(4) }
                };
            }
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
