using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.RegistrationRepositoryTests
{
    public class When_GetRegistrationLite_IsCalled : RegistrationRepositoryBaseTest
    {
        // Seed related
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private List<long> _ulnsWithCalculatedResult;
        private List<OverallGradeLookup> _overallGradeLookup;

        // Input or Output
        private TqRegistrationPathway _actualResult;
        private OverallResult _expectedOverallResult;

        public override void Given()
        {
            // Seed data
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
                { 1111111114, RegistrationPathwayStatus.Active },
            };
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();

            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                var specialismsAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(x => x.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);

                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);
                tqSpecialismAssessmentsSeedData.AddRange(specialismsAssessments);

                // Build results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));
                tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(specialismsAssessments, isLatestActive, isHistoricAssessent));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            var pathwayResults = SeedPathwayResultsData(tqPathwayResultsSeedData, false);
            var specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111 && x.UniqueLearnerNumber != 1111111114))
                SeedIndustyPlacementData(registration.UniqueLearnerNumber);

            // Seed OverallResultLookup
            _expectedOverallResult = new OverallResult
            {
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction*",
                CalculationStatus = CalculationStatus.Completed
            };

           _ulnsWithCalculatedResult = new List<long> { 1111111112 };
            _overallGradeLookup = new List<OverallGradeLookup>();
            foreach (var uln in _ulnsWithCalculatedResult)
            {
                var pathwayId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111112).TqRegistrationPathways.FirstOrDefault().Id;
                var coreResultId = pathwayResults.FirstOrDefault(x => x.TqPathwayAssessment.TqRegistrationPathwayId == pathwayId).TlLookupId;
                var splResultId = specialismResults.FirstOrDefault(x => x.TqSpecialismAssessment.TqRegistrationSpecialism.TqRegistrationPathwayId == pathwayId).TlLookupId;
                _overallGradeLookup.Add(new OverallGradeLookup { TlPathwayId = 3, TlLookupCoreGradeId = coreResultId, TlLookupSpecialismGradeId = splResultId, TlLookupOverallGradeId = 17 });
            }

            OverallGradeLookupProvider.CreateOverallGradeLookupList(DbContext, _overallGradeLookup);
            _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == 1111111112).TqRegistrationPathways.FirstOrDefault().OverallResults.Add(_expectedOverallResult);

            DbContext.SaveChanges();
            DetachAll();

            // Test class.
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId, bool includeProfile, bool includeIndustryPlacements, bool includeOverallResult)
        {
            if (_actualResult != null)
                return;

            _actualResult = await RegistrationRepository.GetRegistrationLiteAsync(aoUkprn, profileId, includeProfile, includeIndustryPlacements, includeOverallResult);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, int uln, bool includeProfile, bool includeIndustryPlacement, bool includeOverallResult,
            bool isRegistrationPathwayExpected, bool isRegistrationProfileExpected, bool isAssessmentExpected, bool isResultExpected, bool isIndustryPlacementExpected, bool isOverallResultExpected)
        {
            var profileId = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).Id;
            await WhenAsync(aoUkprn, profileId, includeProfile, includeIndustryPlacement, includeOverallResult);

            if (isRegistrationPathwayExpected)
                _actualResult.Should().NotBeNull();
            else
            {
                _actualResult.Should().BeNull();
                return;
            }

            // Assert RegistrationPathway
            var expectedPathway = GetExpectedRegistrationPathway(uln);
            AssertRegistrationPathway(_actualResult, expectedPathway);

            // Assert if Profile included
            if (isRegistrationProfileExpected)
            {
                _actualResult.TqRegistrationProfile.Should().NotBeNull();
                AssertRegistrationProfile(_actualResult.TqRegistrationProfile, expectedPathway.TqRegistrationProfile);
            }
            else
                _actualResult.TqRegistrationProfile.Should().BeNull();

            // Assert if PathwayAssessment included
            if (isAssessmentExpected)
            {
                _actualResult.TqPathwayAssessments.Any().Should().BeTrue();

                // Assert PathwayAssessment
                var actualPathwayAssessment = GetExpectedPathwayAssessment(uln, _actualResult);
                var expectedPathwayAssessment = GetExpectedPathwayAssessment(uln, expectedPathway);
                AssertPathwayAssessment(actualPathwayAssessment, expectedPathwayAssessment);
            }
            else
                _actualResult.TqPathwayAssessments.Any().Should().BeFalse();

            // Assert Specialism Assessments
            if (isAssessmentExpected)  // TODO: pass as param. 
            {
                _actualResult.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Any().Should().BeTrue();

                // Assert SpecialismAssessments
                var actualSpecialismAssessment = GetSpecialismsAssessments(_actualResult);
                var expectedSpecialismAssessment = GetSpecialismsAssessments(expectedPathway);
                AssertSpecialismAssessment(actualSpecialismAssessment, expectedSpecialismAssessment);
            }
            else
                _actualResult.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).Any().Should().BeFalse();

            if (isResultExpected)
            {
                // Assert Pathway results
                _actualResult.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).Any().Should().BeTrue();

                var actualPathwayResult = GetPathwayResult(_actualResult);
                var expectedPathwayResult = GetPathwayResult(expectedPathway);
                AssertPathwayResult(actualPathwayResult, expectedPathwayResult);

                // Assert Specialism results
                _actualResult.TqRegistrationSpecialisms.SelectMany(x => x.TqSpecialismAssessments).SelectMany(x => x.TqSpecialismResults).Any().Should().BeTrue();
                
                var actualSpecialismResults = GetSpecialismsResults(_actualResult);
                var expectedSpecialismResults = GetSpecialismsResults(expectedPathway);
                AssertSpecialismResults(actualSpecialismResults, expectedSpecialismResults);
            }

            // Assert Industry Placement
            if (isIndustryPlacementExpected)
            {
                _actualResult.IndustryPlacements.Any().Should().BeTrue();

                var actualIndustryPlacement = _actualResult.IndustryPlacements.FirstOrDefault();
                var expectedIndustryPlacement = expectedPathway.IndustryPlacements.FirstOrDefault(x => x.TqRegistrationPathwayId == expectedPathway.Id);
                AssertIndustryPlacement(actualIndustryPlacement, expectedIndustryPlacement);
            }
            else
                _actualResult.IndustryPlacements.Any().Should().BeFalse();

            // Assert Overall Results
            if (isOverallResultExpected)
            {
                _actualResult.OverallResults.Any().Should().BeTrue();

                var actualOverallResult = _actualResult.OverallResults.FirstOrDefault();
                AssertOverallResult(actualOverallResult, _expectedOverallResult);
            }
            else
                _actualResult.OverallResults.Any().Should().BeFalse();
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // InputParams: aoUkprn, Uln, inclProf, inclIp, inclOverallResult AND ExpectedResult: isRegistrationPathwayExpected, isRegistrationProfileExpected, isAssessmentExpected, isResultExpected, isIndustryPlacementExpected, isOverallResultExpected
                    new object[] { 99999999, 1111111111, false, false, false,
                                   false, false, false, false, false, false },
                    new object[] { 10011881, 1111111112, true, true, false,
                                   true, true, true, true, true, false },
                    new object[] { 10011881, 1111111113, true, true, false,
                                   true, true, true, true, true, false },
                    new object[] { 10011881, 1111111114, true, true, false,
                                   true, true, true, true, false, false },

                    new object[] { 10011881, 1111111112, false, false, false,
                                   true, false, true, true, false, false },
                    new object[] { 10011881, 1111111113, false, false, false,
                                   true, false, true, true, false, false },
                    new object[] { 10011881, 1111111114, false, false, false,
                                   true, false, true, true, false, false},

                    new object[] { 10011881, 1111111112, false, false, true,
                                   true, false, true, true, false, true }
                };
            }
        }

        private void SeedIndustyPlacementData(long uln)
        {
            var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
        }

        private TqRegistrationPathway GetExpectedRegistrationPathway(long uln)
        {
            var expectedRegistration = _registrations.SingleOrDefault(x => x.UniqueLearnerNumber == uln);

            if (expectedRegistration.TqRegistrationPathways.FirstOrDefault().Status == RegistrationPathwayStatus.Withdrawn)
                return expectedRegistration.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Withdrawn && p.EndDate != null);
            else
                return expectedRegistration.TqRegistrationPathways.FirstOrDefault(p => p.Status == RegistrationPathwayStatus.Active && p.EndDate == null);
        }

        private TqPathwayAssessment GetExpectedPathwayAssessment(long uln, TqRegistrationPathway expectedPathway)
        {
            if (expectedPathway.Status == RegistrationPathwayStatus.Withdrawn)
                return _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate != null);
            else
                return _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate == null);
        }

        private TqPathwayResult GetPathwayResult(TqRegistrationPathway pathway)
        {
            if (pathway.Status == RegistrationPathwayStatus.Withdrawn)
                return pathway.TqPathwayAssessments.Where(x => x.IsOptedin && x.EndDate != null)
                    .SelectMany(x => x.TqPathwayResults).FirstOrDefault(pr => pr.IsOptedin && pr.EndDate != null);
            else
                return pathway.TqPathwayAssessments.Where(x => x.IsOptedin && x.EndDate == null)
                   .SelectMany(x => x.TqPathwayResults).FirstOrDefault(pr => pr.IsOptedin && pr.EndDate == null);
        }

        private IList<TqSpecialismAssessment> GetSpecialismsAssessments(TqRegistrationPathway pathway)
        {
            if (pathway.Status == RegistrationPathwayStatus.Withdrawn)
                return pathway.TqRegistrationSpecialisms.Where(x => x.IsOptedin && x.EndDate != null)
                    .SelectMany(x => x.TqSpecialismAssessments).Where(sa => sa.IsOptedin && sa.EndDate != null).ToList();
            else
                return pathway.TqRegistrationSpecialisms.Where(x => x.IsOptedin && x.EndDate == null)
                    .SelectMany(x => x.TqSpecialismAssessments).Where(sa => sa.IsOptedin && sa.EndDate == null).ToList();
        }

        private IList<TqSpecialismResult> GetSpecialismsResults(TqRegistrationPathway pathway)
        {
            if (pathway.Status == RegistrationPathwayStatus.Withdrawn)
                return pathway.TqRegistrationSpecialisms.Where(x => x.IsOptedin && x.EndDate != null)
                    .SelectMany(x => x.TqSpecialismAssessments).Where(sa => sa.IsOptedin && sa.EndDate != null)
                    .SelectMany(x => x.TqSpecialismResults).Where(sr => sr.IsOptedin && sr.EndDate != null).ToList();
            else
                return pathway.TqRegistrationSpecialisms.Where(x => x.IsOptedin && x.EndDate == null)
                    .SelectMany(x => x.TqSpecialismAssessments).Where(sa => sa.IsOptedin && sa.EndDate == null)
                    .SelectMany(x => x.TqSpecialismResults).Where(sr => sr.IsOptedin && sr.EndDate == null).ToList();
        }
    }
}