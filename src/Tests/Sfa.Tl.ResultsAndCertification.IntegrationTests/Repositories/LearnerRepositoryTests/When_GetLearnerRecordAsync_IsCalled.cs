﻿using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Repositories.LearnerRepositoryTests
{
    public class When_GetLearnerRecordAsync_IsCalled : LearnerRepositoryBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private TqRegistrationPathway _result;

        public override void Given()
        {
            AoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Active },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
            };

            /// Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();
            var industryPlacementUln = 1111111115;
            var profilesWithPrsStatus = new List<(long, PrsStatus?)> { (1111111111, null), (1111111112, null), (1111111113, null), (1111111114, PrsStatus.BeingAppealed), (1111111115, null) };

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111 && x.UniqueLearnerNumber != industryPlacementUln))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                // Pathway Assessments
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);
                
                // Pathway Results
                foreach (var pathwayAssessment in pathwayAssessments)
                {
                    var prsStatus = profilesWithPrsStatus.FirstOrDefault(p => p.Item1 == pathwayAssessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                    tqPathwayResultsSeedData.AddRange(GetPathwayResultDataToProcess(pathwayAssessment, isLatestActive, isHistoricAssessent, prsStatus));
                }

                // Specialism Assesments
                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    var specialismAssessments = GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent);
                    tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                    foreach (var specialismAssessment in specialismAssessments)
                    {
                        // Specialism Results
                        var prsStatus = profilesWithPrsStatus.FirstOrDefault(p => p.Item1 == specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                        tqSpecialismResultsSeedData.AddRange(GetSpecialismResultDataToProcess(specialismAssessment, isLatestActive, isHistoricAssessent, prsStatus));
                    }
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            SeedIndustyPlacementData(industryPlacementUln);

            DbContext.SaveChanges();
            DetachAll();

            LearnerRepository = new LearnerRepository(DbContext);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId)
        {
            if (_result != null)
                return;

            _result = await LearnerRepository.GetLearnerRecordAsync(aoUkprn, profileId);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, long uln, int profileId, RegistrationPathwayStatus status, bool hasAssessments, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, profileId);

            if (_result == null)
            {
                expectedResponse.Should().BeFalse();
                return;
            }

            // Expected result
            var expectedRegistration = _registrations.SingleOrDefault(x => x.UniqueLearnerNumber == uln);

            expectedRegistration.Should().NotBeNull();

            TqRegistrationPathway expectedPathway = null;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedPathway = expectedRegistration.TqRegistrationPathways.FirstOrDefault(p => p.Status == status && p.EndDate != null);
            }
            else
            {
                expectedPathway = expectedRegistration.TqRegistrationPathways.FirstOrDefault(p => p.Status == status && p.EndDate == null);
            }

            expectedPathway.Should().NotBeNull();

            TqRegistrationSpecialism expectedSpecialim = null;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedSpecialim = expectedPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin && s.EndDate != null);
            }
            else
            {
                expectedSpecialim = expectedPathway.TqRegistrationSpecialisms.FirstOrDefault(s => s.IsOptedin && s.EndDate == null);
            }

            TqPathwayAssessment expectedPathwayAssessment = null;
            TqPathwayResult expectedPathwayResult = null;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate != null);
                expectedPathwayResult = expectedPathwayAssessment?.TqPathwayResults.FirstOrDefault(x => x.IsOptedin && x.EndDate != null);
            }
            else
            {
                expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate == null);
                expectedPathwayResult = expectedPathwayAssessment?.TqPathwayResults.FirstOrDefault(x => x.IsOptedin && x.EndDate == null);
            }

            TqSpecialismAssessment expectedSpecialismAssessment = null;
            TqSpecialismResult expectedSpecialismResult = null;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedSpecialismAssessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedSpecialim.Id && x.IsOptedin && x.EndDate != null);
                expectedSpecialismResult = expectedSpecialismAssessment?.TqSpecialismResults.FirstOrDefault(x => x.TqSpecialismAssessmentId == expectedSpecialismAssessment.Id && x.IsOptedin && x.EndDate != null);
            }
            else
            {
                expectedSpecialismAssessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedSpecialim.Id && x.IsOptedin && x.EndDate == null);
                expectedSpecialismResult = expectedSpecialismAssessment?.TqSpecialismResults.FirstOrDefault(x => x.TqSpecialismAssessmentId == expectedSpecialismAssessment.Id && x.IsOptedin && x.EndDate == null);
            }

            _result.Should().NotBeNull();

            // Actual result
            var actualPathway = _result;
            var actualPathwayAssessment = actualPathway.TqPathwayAssessments.FirstOrDefault();
            var actualPathwayResult = actualPathwayAssessment?.TqPathwayResults.FirstOrDefault();
            
            var actualSpecialism = actualPathway.TqRegistrationSpecialisms.FirstOrDefault();
            var actualSpecialismAssessment = actualSpecialism.TqSpecialismAssessments.FirstOrDefault();
            var actualSpecialismResult = actualSpecialismAssessment?.TqSpecialismResults.FirstOrDefault();

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

            // Assert Assessments
            if (hasAssessments)
            {
                AssertPathwayAssessment(actualPathwayAssessment, expectedPathwayAssessment);
                AssertSpecialismAssessment(actualSpecialismAssessment, expectedSpecialismAssessment);

                AssertPathwayResult(actualPathwayResult, expectedPathwayResult);
                AssertSpecialismResult(actualSpecialismResult, expectedSpecialismResult);
            }

            // Industry Placement
            var expectedIndustryPlacement = expectedPathway.IndustryPlacements.FirstOrDefault();
            var actualIndustryPlacement = actualPathway.IndustryPlacements.FirstOrDefault();
            AssertIndustryPlacement(actualIndustryPlacement, expectedIndustryPlacement);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Uln not found
                    new object[] { 10011881, 0000000000, 0, RegistrationPathwayStatus.Active, false, false },

                    // Uln not found for registered AoUkprn
                    new object[] { 00000000, 1111111111, 1, RegistrationPathwayStatus.Active, false, false },
                    
                    // Uln: 1111111111 - Registration(Active) but no asessment entries for pathway and specialism
                    new object[] { 10011881, 1111111111, 1, RegistrationPathwayStatus.Active, false, true },

                    // Uln: 1111111112 - Registration(Active), TqPathwayAssessments(Active + History) and TqSpecialismAssessments(Active + History)
                    new object[] { 10011881, 1111111112, 2, RegistrationPathwayStatus.Active, true, true },

                    // Uln: 1111111113 - Registration(Withdrawn), TqPathwayAssessments(Withdrawn) and TqSpecialismAssessments(Withdrawn)
                    new object[] { 10011881, 1111111113, 3, RegistrationPathwayStatus.Withdrawn, true, true },

                    // Uln: 1111111114 - Registration(Active), TqPathwayAssessments(Active), TqResult (Active)
                    new object[] { 10011881, 1111111114, 4, RegistrationPathwayStatus.Active, true, true },

                    // Uln: 1111111115 - Registration(Active) + Assessment(None) + IndustryPlacement(Completed)
                    new object[] { 10011881, 1111111115, 5, RegistrationPathwayStatus.Active, false, true }
                };
            }
        }

        private void SeedIndustyPlacementData(int uln)
        {
            var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
        }
    }
}
