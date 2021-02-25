using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AssessmentServiceTests
{
    public class When_GetActivePathwayAssessmentEntryDetailsAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AssessmentEntryDetails _result;

        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;

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
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                //Build Pathway results
                var ulnWithResult = new List<long> { 1111111114, 1111111115 };
                if (ulnWithResult.Any(x => x == registration.UniqueLearnerNumber))
                {
                    foreach (var assessment in pathwayAssessments)
                    {
                        var hasHitoricResult = new List<long> { 1111111115 };
                        var isHistoricResult = hasHitoricResult.Any(x => x == registration.UniqueLearnerNumber);
                        var isLatestActiveResult = !isHistoricResult;

                        var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, isLatestActiveResult, isHistoricResult);
                        tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                    }
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, true);

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

        public async Task WhenAsync(long aoUkprn, long uln, bool isActiveAssessment)
        {
            if (_result != null)
                return;

            TqPathwayAssessment pathwayAssessment = null; 

            if(isActiveAssessment)
            {
                pathwayAssessment = _pathwayAssessments.FirstOrDefault(pa => pa.IsOptedin && pa.EndDate == null && pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }
            else
            {
                pathwayAssessment = _pathwayAssessments.FirstOrDefault(pa => !pa.IsOptedin && pa.EndDate != null && pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }

            var assessmentId = pathwayAssessment != null ? pathwayAssessment.Id : 0;
            _result = await AssessmentService.GetActivePathwayAssessmentEntryDetailsAsync(aoUkprn, assessmentId);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, long uln, bool isActiveAssessment, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, uln, isActiveAssessment);

            if (_result == null)
            {
                expectedResponse.Should().BeFalse();
                return;
            }

            TqPathwayAssessment expectedAssessment = null;
            if (isActiveAssessment)
            {
                expectedAssessment = _pathwayAssessments.FirstOrDefault(pa => pa.IsOptedin && pa.EndDate == null && pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }
            else
            {
                expectedAssessment = _pathwayAssessments.FirstOrDefault(pa => !pa.IsOptedin && pa.EndDate != null && pa.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }

            expectedAssessment.Should().NotBeNull();

            var expectedAssessmentDetails = new AssessmentEntryDetails
            {
                ProfileId = expectedAssessment.TqRegistrationPathway.TqRegistrationProfileId,
                AssessmentId = expectedAssessment.Id,
                AssessmentSeriesName = expectedAssessment.AssessmentSeries.Name
            };

            // Assert
            _result.ProfileId.Should().Be(expectedAssessmentDetails.ProfileId);
            _result.AssessmentId.Should().Be(expectedAssessmentDetails.AssessmentId);
            _result.AssessmentSeriesName.Should().Be(expectedAssessmentDetails.AssessmentSeriesName);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // Uln not valid
                    new object[] { 10011881, 0000000000, true, false },

                    // Uln not found for registered AoUkprn
                    new object[] { 00000000, 1111111111, true, false },
                    
                    // Uln: 1111111111 - Registration(Active) but no asessment entries for pathway
                    new object[] { 10011881, 1111111111, true, false },

                    // Uln: 1111111112 - Registration(Active), TqPathwayAssessments(Active + History)
                    new object[] { 10011881, 1111111112, false, false},
                    new object[] { 10011881, 1111111112, true, true},

                    // Uln: 1111111113 - Registration(Withdrawn), TqPathwayAssessments(Withdrawn)
                    new object[] { 10011881, 1111111113, false, false },
                    new object[] { 10011881, 1111111113, true, false },

                    // Uln: 1111111114 - Registration(Active), TqPathwayAssessments(Active), Results(Active)
                    new object[] { 10011881, 1111111114, true, false },

                    // Uln: 1111111115 - Registration(Active), TqPathwayAssessments(Active), Results(Inactive)
                    new object[] { 10011881, 1111111115, true, true }
                };
            }
        }
    }
}
