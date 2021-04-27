using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_GetAssessmentDetailsAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AssessmentDetails _result;

        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;

        public override void Given()
        {
            // Parameters
            AoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> 
            { 
                { 1111111111, RegistrationPathwayStatus.Active }, 
                { 1111111112, RegistrationPathwayStatus.Active }, 
                { 1111111113, RegistrationPathwayStatus.Withdrawn },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
            };

            // Create mapper
            CreateMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var industryPlacementUln = 1111111115;

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111 && x.UniqueLearnerNumber != industryPlacementUln))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent));
                }

                // Build Pathway results
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(pathwayAssessments, isLatestActive, isHistoricAssessent));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            SeedIndustyPlacementData(industryPlacementUln);

            DbContext.SaveChanges();

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId)
        {
            if (_result != null)
                return;

            _result = await AssessmentService.GetAssessmentDetailsAsync(aoUkprn, profileId);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, long uln, int profileId, RegistrationPathwayStatus status, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, profileId);

            if (_result == null)
            {
                expectedResponse.Should().BeFalse();
                return;
            }

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

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedSpecialismAssessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedSpecialim.Id && x.IsOptedin && x.EndDate != null);
            }
            else
            {
                expectedSpecialismAssessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialismId == expectedSpecialim.Id && x.IsOptedin && x.EndDate == null);
            }

            var expectedAssessmentDetails = new AssessmentDetails
            {
                ProfileId = expectedRegistration.Id,
                Uln = expectedRegistration.UniqueLearnerNumber,
                Firstname = expectedRegistration.Firstname,
                Lastname = expectedRegistration.Lastname,
                ProviderUkprn = expectedPathway.TqProvider.TlProvider.UkPrn,
                ProviderName = expectedPathway.TqProvider.TlProvider.Name,
                PathwayLarId = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                PathwayName = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                PathwayAssessmentSeries = expectedPathwayAssessment?.AssessmentSeries.Name,
                PathwayAssessmentId = expectedPathwayAssessment != null ? expectedPathwayAssessment.Id : (int?)null,

                SpecialismLarId = expectedSpecialim?.TlSpecialism.LarId,
                SpecialismName = expectedSpecialim?.TlSpecialism.Name,
                SpecialismAssessmentSeries = expectedSpecialismAssessment?.AssessmentSeries.Name,
                SpecialismAssessmentId = expectedSpecialismAssessment != null ? expectedSpecialismAssessment.Id : (int?)null,
                Status = expectedPathway.Status,

                PathwayResultId = expectedPathwayResult?.Id
            };

            var expectedIsIndustryPlacementExist = expectedRegistration.TqRegistrationPathways.FirstOrDefault().IndustryPlacements.Any();

            // Assert
            _result.ProfileId.Should().Be(expectedAssessmentDetails.ProfileId);
            _result.Uln.Should().Be(expectedAssessmentDetails.Uln);
            _result.Firstname.Should().Be(expectedAssessmentDetails.Firstname);
            _result.Lastname.Should().Be(expectedAssessmentDetails.Lastname);
            _result.ProviderUkprn.Should().Be(expectedAssessmentDetails.ProviderUkprn);
            _result.ProviderName.Should().Be(expectedAssessmentDetails.ProviderName);
            _result.PathwayLarId.Should().Be(expectedAssessmentDetails.PathwayLarId);
            _result.PathwayName.Should().Be(expectedAssessmentDetails.PathwayName);
            _result.PathwayAssessmentSeries.Should().Be(expectedAssessmentDetails.PathwayAssessmentSeries);
            _result.PathwayAssessmentId.Should().Be(expectedAssessmentDetails.PathwayAssessmentId);
            _result.SpecialismLarId.Should().Be(expectedAssessmentDetails.SpecialismLarId);
            _result.SpecialismName.Should().Be(expectedAssessmentDetails.SpecialismName);
            _result.SpecialismAssessmentSeries.Should().Be(expectedAssessmentDetails.SpecialismAssessmentSeries);
            _result.SpecialismAssessmentId.Should().Be(expectedAssessmentDetails.SpecialismAssessmentId);
            _result.Status.Should().Be(expectedAssessmentDetails.Status);
            _result.PathwayResultId.Should().Be(expectedAssessmentDetails.PathwayResultId);
            _result.IsIndustryPlacementExist.Should().Be(expectedIsIndustryPlacementExist);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Uln not found
                    new object[] { 10011881, 0000000000, 0, RegistrationPathwayStatus.Active, false },

                    // Uln not found for registered AoUkprn
                    new object[] { 00000000, 1111111111, 1, RegistrationPathwayStatus.Active, false },
                    
                    // Uln: 1111111111 - Registration(Active) but no asessment entries for pathway and specialism
                    new object[] { 10011881, 1111111111, 1, RegistrationPathwayStatus.Active, true },

                    // Uln: 1111111112 - Registration(Active), TqPathwayAssessments(Active + History) and TqSpecialismAssessments(Active + History)
                    new object[] { 10011881, 1111111112, 2, RegistrationPathwayStatus.Active, true },

                    // Uln: 1111111113 - Registration(Withdrawn), TqPathwayAssessments(Withdrawn) and TqSpecialismAssessments(Withdrawn)
                    new object[] { 10011881, 1111111113, 3, RegistrationPathwayStatus.Withdrawn, true },

                    // Uln: 1111111114 - Registration(Active), TqPathwayAssessments(Active), TqResult (Active)
                    new object[] { 10011881, 1111111114, 4, RegistrationPathwayStatus.Active, true },

                    // Uln: 1111111115 - Registration(Active) + IndustryPlacement(Completed)
                    new object[] { 10011881, 1111111115, 5, RegistrationPathwayStatus.Active, true },
                };
            }
        }

        private void SeedIndustyPlacementData(int uln)
        {
            var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
            IndustryPlacementProvider.CreateQualificationAchieved(DbContext, pathway.Id, IndustryPlacementStatus.Completed);
        }
    }
}
