using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.LearnerServiceTests
{
    public class When_GetLearnerRecordAsync_IsCalled : LearnerServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private LearnerRecord _result;

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
                { 1111111116, RegistrationPathwayStatus.Active }
            };

            // Create mapper
            CreateMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);


            var currentYearUln = new List<long> { 1111111116 };
            RegisterUlnForNextAcademicYear(_registrations, currentYearUln);

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

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Build Pathway results
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

            LearnerRepositoryLogger = new Logger<LearnerRepository>(new NullLoggerFactory());
            LearnerRepository = new LearnerRepository(DbContext);
            LearnerService = new LearnerService(LearnerRepository, LearnerMapper, LearnerRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId)
        {
            if (_result != null)
                return;

            _result = await LearnerService.GetLearnerRecordAsync(aoUkprn, profileId);
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

            var expectedIndustryPlacement = expectedPathway.IndustryPlacements?.FirstOrDefault();

            var expectedLearnerRecord = new LearnerRecord
            {
                ProfileId = expectedRegistration.Id,
                Uln = expectedRegistration.UniqueLearnerNumber,
                Firstname = expectedRegistration.Firstname,
                Lastname = expectedRegistration.Lastname,
                DateofBirth = expectedRegistration.DateofBirth,
                Gender = expectedRegistration.Gender,
                Pathway = new Pathway
                {
                    Id = expectedPathway.Id,
                    LarId = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                    Name = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                    Title = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                    StartYear = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.StartYear,
                    AcademicYear = expectedPathway.AcademicYear,                    
                    Status = expectedPathway.Status,
                    Provider = new Provider
                    {
                        Id = expectedPathway.TqProvider.TlProvider.Id,
                        Ukprn = expectedPathway.TqProvider.TlProvider.UkPrn,
                        Name = expectedPathway.TqProvider.TlProvider.Name,
                        DisplayName = expectedPathway.TqProvider.TlProvider.DisplayName,
                    },
                    PathwayAssessments = expectedPathwayAssessment != null ? new List<Assessment>
                    {
                        new Assessment
                        {
                            Id = expectedPathwayAssessment.Id,
                            SeriesId = expectedPathwayAssessment.AssessmentSeries.Id,
                            SeriesName = expectedPathwayAssessment.AssessmentSeries.Name,
                            ComponentType = ComponentType.Core,
                            ResultEndDate = expectedPathwayAssessment.AssessmentSeries.EndDate,
                            RommEndDate = expectedPathwayAssessment.AssessmentSeries.RommEndDate,
                            AppealEndDate = expectedPathwayAssessment.AssessmentSeries.AppealEndDate,
                            LastUpdatedBy = expectedPathwayAssessment.CreatedBy,
                            LastUpdatedOn = expectedPathwayAssessment.CreatedOn,
                            Result = expectedPathwayResult != null ?
                                new Result
                                {
                                    Id = expectedPathwayResult.Id,
                                    Grade = expectedPathwayResult.TlLookup.Value,
                                    GradeCode = expectedPathwayResult.TlLookup.Code,
                                    PrsStatus = expectedPathwayResult.PrsStatus,
                                    LastUpdatedBy = expectedPathwayResult.CreatedBy,
                                    LastUpdatedOn = expectedPathwayResult.CreatedOn
                                }
                                : null
                        }
                    } : new List<Assessment>(),
                    Specialisms = new List<Specialism>
                    {
                        new Specialism
                        {
                            Id = expectedSpecialim.Id,
                            LarId = expectedSpecialim.TlSpecialism.LarId,
                            Name = expectedSpecialim.TlSpecialism.Name,
                            Assessments = expectedSpecialismAssessment != null ? new List<Assessment>
                            {
                                new Assessment
                                {
                                    Id = expectedSpecialismAssessment.Id,
                                    SeriesId = expectedSpecialismAssessment.AssessmentSeries.Id,
                                    SeriesName = expectedSpecialismAssessment.AssessmentSeries.Name,
                                    ComponentType = ComponentType.Specialism,
                                    ResultEndDate = expectedPathwayAssessment.AssessmentSeries.EndDate,
                                    RommEndDate = expectedSpecialismAssessment.AssessmentSeries.RommEndDate,
                                    AppealEndDate = expectedSpecialismAssessment.AssessmentSeries.AppealEndDate,
                                    LastUpdatedBy = expectedSpecialismAssessment.CreatedBy,
                                    LastUpdatedOn = expectedSpecialismAssessment.CreatedOn,
                                    Result = expectedSpecialismResult != null ?
                                        new Result
                                        {
                                            Id = expectedSpecialismResult.Id,
                                            Grade = expectedSpecialismResult.TlLookup.Value,
                                            GradeCode = expectedSpecialismResult.TlLookup.Code,
                                            PrsStatus = expectedSpecialismResult.PrsStatus,
                                            LastUpdatedBy = expectedSpecialismResult.CreatedBy,
                                            LastUpdatedOn = expectedSpecialismResult.CreatedOn
                                        }
                                        : null
                                        }
                            } : new List<Assessment>()
                        }
                    },
                    IndustryPlacements = expectedIndustryPlacement != null ? new List<Models.Contracts.Learner.IndustryPlacement>
                    {
                        new Models.Contracts.Learner.IndustryPlacement
                        {
                            Id = expectedIndustryPlacement.Id,
                            Status = expectedIndustryPlacement.Status
                        }
                    } : new List<Models.Contracts.Learner.IndustryPlacement>()
                }
            };

            // Assert
            _result.ProfileId.Should().Be(expectedLearnerRecord.ProfileId);
            _result.Uln.Should().Be(expectedLearnerRecord.Uln);
            _result.Firstname.Should().Be(expectedLearnerRecord.Firstname);
            _result.Lastname.Should().Be(expectedLearnerRecord.Lastname);
            _result.DateofBirth.Should().Be(expectedLearnerRecord.DateofBirth);
            _result.Gender.Should().Be(expectedLearnerRecord.Gender);
            _result.Pathway.Should().BeEquivalentTo(expectedLearnerRecord.Pathway);
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

                    // IsCoreEntryEligible 
                    new object[] { 10011881, 1111111116, 6, RegistrationPathwayStatus.Active, true },
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
