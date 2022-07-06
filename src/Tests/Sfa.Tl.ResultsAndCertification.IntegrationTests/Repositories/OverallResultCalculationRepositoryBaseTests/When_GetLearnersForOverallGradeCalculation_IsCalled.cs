using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.OverallResultCalculationRepositoryBaseTests
{
    public class When_GetLearnersForOverallGradeCalculation_IsCalled : OverallResultCalculationRepositoryBaseTest
    {
        private List<SeedRegistrationData> _seedRegistrationsData;
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<long> _ulnWithAcademicYear2021;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqPathwayResult> _pathwayResults;
        private List<TqSpecialismResult> _specialismResults;

        private IList<TqRegistrationPathway> _result;
        private List<long> _ulnWithPrevCalcResults;

        public override void Given()
        {
            _seedRegistrationsData = new List<SeedRegistrationData>
            {
                new SeedRegistrationData { Uln = 1111111111, Status = RegistrationPathwayStatus.Active, AcademicYear = 2020 },
                new SeedRegistrationData { Uln =1111111112, Status = RegistrationPathwayStatus.Active, AcademicYear = 2020 },
                new SeedRegistrationData { Uln =1111111113, Status = RegistrationPathwayStatus.Withdrawn, AcademicYear = 2020 },
                new SeedRegistrationData { Uln =1111111114, Status = RegistrationPathwayStatus.Active, AcademicYear = 2021 },
                new SeedRegistrationData { Uln =1111111115, Status = RegistrationPathwayStatus.Active, AcademicYear = 2021 },
                new SeedRegistrationData { Uln =1111111116, Status = RegistrationPathwayStatus.Withdrawn, AcademicYear = 2021 }
            };
            _ulns = _seedRegistrationsData.Select(x => new { x.Uln, x.Status }).ToDictionary(d => d.Uln, d => d.Status);

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);
            _ulnWithAcademicYear2021 = new List<long> { 1111111114, 1111111115, 1111111116 };
            SetAcademicYear(_registrations, 2021, _ulnWithAcademicYear2021);

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

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData, false);

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            _specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            SeedIndustyPlacementData(1111111114);

            // Overall Results seed
            var ulnsWithOverallResult = new List<long> { 1111111112, 1111111114 };
            var _overallResults = SeedOverallResultData(_registrations, ulnsWithOverallResult, false);
            _ulnWithPrevCalcResults = new List<long> { 1111111112 };
            SetOverallResultAsPrevCalculated(_overallResults, _ulnWithPrevCalcResults);

            DbContext.SaveChanges();

            // Create repository class to test. 
            CreateOverallResultCalculationRepository();
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(int fromAcademicYear, int toAcademicYear)
        {
            _result = await OverallResultCalculationRepository.GetLearnersForOverallGradeCalculation(fromAcademicYear, toAcademicYear);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(int fromAcademicYear, int toAcademicYear, int recCount)
        {
            await WhenAsync(fromAcademicYear, toAcademicYear);
            _result.Should().HaveCount(recCount);

            foreach (var data in _seedRegistrationsData)
            {
                var actualPathway = _result.FirstOrDefault(x => x.TqRegistrationProfile.UniqueLearnerNumber == data.Uln);
                // Only active status reg's should be present. 
                if (data.Status != RegistrationPathwayStatus.Active ||
                    !(data.AcademicYear >= fromAcademicYear && data.AcademicYear <= toAcademicYear) ||
                    _ulnWithPrevCalcResults.Contains(data.Uln))
                {
                    actualPathway.Should().BeNull();
                    continue;
                }
                actualPathway.Should().NotBeNull();

                var expectedRegistration = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == data.Uln);
                var expectedPathway = expectedRegistration.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active && p.EndDate == null);
                var expectedSpecialim = expectedPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin && s.EndDate == null);
                var expectedSpecialismAssessment = expectedSpecialim.TqSpecialismAssessments.FirstOrDefault(sa => sa.IsOptedin && sa.EndDate == null);
                var expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == data.Uln && x.IsOptedin && x.EndDate == null);

                var expectedPathwayResult = _pathwayResults.FirstOrDefault(x => expectedPathwayAssessment != null && x.TqPathwayAssessmentId == expectedPathwayAssessment.Id && x.IsOptedin && x.EndDate == null);
                var expectedSpecialismResult = _specialismResults.FirstOrDefault(x => expectedPathwayAssessment != null && x.TqSpecialismAssessmentId == expectedSpecialismAssessment.Id && x.IsOptedin && x.EndDate == null);

                // Actual result
                var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault();
                var actualPathwayAssessment = actualPathway.TqPathwayAssessments.FirstOrDefault();
                var actualPathwayResult = actualPathwayAssessment?.TqPathwayResults.FirstOrDefault();

                // Assert Registration Pathway
                actualPathway.TqRegistrationProfileId.Should().Be(expectedPathway.TqRegistrationProfileId);
                actualPathway.TqProviderId.Should().Be(expectedPathway.TqProviderId);
                actualPathway.AcademicYear.Should().Be(expectedPathway.AcademicYear);
                actualPathway.StartDate.Should().Be(expectedPathway.StartDate);
                actualPathway.EndDate.Should().Be(expectedPathway.EndDate);
                actualPathway.Status.Should().Be(expectedPathway.Status);

                // Assert Registration Specialism
                actualSpecialism.TqRegistrationPathwayId.Should().Be(expectedSpecialim.TqRegistrationPathwayId);
                actualSpecialism.TlSpecialismId.Should().Be(expectedSpecialim.TlSpecialismId);
                actualSpecialism.StartDate.Should().Be(expectedSpecialim.StartDate);
                actualSpecialism.EndDate.Should().Be(expectedSpecialim.EndDate);
                actualSpecialism.IsOptedin.Should().Be(expectedSpecialim.IsOptedin);

                // assert assessments
                var hasAssessment = data.Uln != 1111111111;
                if (hasAssessment)
                {
                    AssertPathwayAssessment(actualPathwayAssessment, expectedPathwayAssessment);
                    AssertPathwayResult(actualPathwayResult, expectedPathwayResult);

                    var actualSpecialismAssessment = actualSpecialism.TqSpecialismAssessments.FirstOrDefault();
                    var actualSpecialismResult = actualSpecialismAssessment.TqSpecialismResults.FirstOrDefault();

                    AssertSpecialismAssessment(actualSpecialismAssessment, expectedSpecialismAssessment);
                    AssertSpecialismResult(actualSpecialismResult, expectedSpecialismResult);
                }

                // Industry Placement
                var expectedIndustryPlacement = expectedPathway.IndustryPlacements.FirstOrDefault();
                var actualIndustryPlacement = actualPathway.IndustryPlacements.FirstOrDefault();
                AssertIndustryPlacement(actualIndustryPlacement, expectedIndustryPlacement);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1900, 1902, 0 },
                    new object[] { 2020, 2020, 1 },
                    new object[] { 2021, 2021, 2 },
                    new object[] { 2020, 2021, 3 }
                };
            }
        }

        private void SetAcademicYear(List<TqRegistrationProfile> _registrations, int academicYear, List<long> ulns)
        {
            _registrations.Where(x => ulns.Contains(x.UniqueLearnerNumber)).ToList()
                .ForEach(x => { x.TqRegistrationPathways.FirstOrDefault().AcademicYear = academicYear; });
        }

        private static void SetOverallResultAsPrevCalculated(List<OverallResult> _overallResults, List<long> ulnWithPrevCalcResults)
        {
            foreach (var ulnWithPrevCalcResult in ulnWithPrevCalcResults)
            {
                var resultToUpdate = _overallResults.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == ulnWithPrevCalcResult);
                resultToUpdate.CreatedOn = DateTime.UtcNow.AddHours(2);
            }
        }
        
        public void SeedIndustyPlacementData(int uln)
        {
            var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
        }
    }

    public class SeedRegistrationData
    {
        public long Uln { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public int AcademicYear { get; set; }
    }
}
