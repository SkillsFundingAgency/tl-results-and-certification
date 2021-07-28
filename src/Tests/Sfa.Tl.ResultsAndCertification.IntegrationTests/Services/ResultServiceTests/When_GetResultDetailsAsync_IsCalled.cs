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

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.ResultServiceTests
{
    public class When_GetResultDetailsAsync_IsCalled : ResultServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private ResultDetails _result;

        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;

        public override void Given()
        {
            // Parameters
            AoUkprn = 10011881;
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn }, { 1111111114, RegistrationPathwayStatus.Active } };

            // Create mapper
            CreateMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent));
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData);

            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var profilesWithResultsAndPrsStatus = new List<(long, PrsStatus?)> { (1111111111, null), (1111111112, null), (1111111113, null), (1111111114, PrsStatus.BeingAppealed) };
            foreach (var assessment in _pathwayAssessments)
            {
                var inactiveResultUlns = new List<long> { 1111111112 };
                var isLatestResultActive = !inactiveResultUlns.Any(x => x == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber);
                var prsStatus = profilesWithResultsAndPrsStatus.FirstOrDefault(p => p.Item1 == assessment.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                tqPathwayResultsSeedData.AddRange(GetPathwayResultsDataToProcess(new List<TqPathwayAssessment> { assessment }, isLatestResultActive, false, prsStatus));
            }

            SeedPathwayResultsData(tqPathwayResultsSeedData);

            //DbContext.SaveChanges();

            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            ResultServiceLogger = new Logger<ResultService>(new NullLoggerFactory());

            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, ResultMapper, ResultServiceLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, int profileId)
        {
            if (_result != null)
                return;

            _result = await ResultService.GetResultDetailsAsync(aoUkprn, profileId);
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

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate != null);
            }
            else
            {
                expectedPathwayAssessment = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln && x.IsOptedin && x.EndDate == null);
            }

            TqPathwayResult expectedPathwayResult = null;

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                expectedPathwayResult = expectedPathwayAssessment?.TqPathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == expectedPathwayAssessment.Id && x.IsOptedin && x.EndDate != null);
            }
            else
            {
                expectedPathwayResult = expectedPathwayAssessment?.TqPathwayResults.FirstOrDefault(x => x.TqPathwayAssessmentId == expectedPathwayAssessment.Id && x.IsOptedin && x.EndDate == null);
            }

            var expectedResultDetails = new ResultDetails
            {
                ProfileId = expectedRegistration.Id,
                Uln = expectedRegistration.UniqueLearnerNumber,
                Firstname = expectedRegistration.Firstname,
                Lastname = expectedRegistration.Lastname,
                DateofBirth = expectedRegistration.DateofBirth,
                ProviderName = expectedPathway.TqProvider.TlProvider.Name,
                ProviderUkprn = expectedPathway.TqProvider.TlProvider.UkPrn,
                TlevelTitle = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle,
                PathwayLarId = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                PathwayName = expectedPathway.TqProvider.TqAwardingOrganisation.TlPathway.Name,
                PathwayAssessmentSeries = expectedPathwayAssessment?.AssessmentSeries?.Name,
                AppealEndDate = expectedPathwayAssessment?.AssessmentSeries?.AppealEndDate,
                PathwayAssessmentId = expectedPathwayAssessment?.Id,
                PathwayResultId = expectedPathwayResult?.Id,
                PathwayResult = expectedPathwayResult?.TlLookup?.Value,
                PathwayResultCode = expectedPathwayResult?.TlLookup?.Code,
                PathwayPrsStatus = expectedPathwayResult?.PrsStatus,
                Status = expectedPathway.Status
            };

            // Assert
            _result.ProfileId.Should().Be(expectedResultDetails.ProfileId);
            _result.Uln.Should().Be(expectedResultDetails.Uln);
            _result.Firstname.Should().Be(expectedResultDetails.Firstname);
            _result.Lastname.Should().Be(expectedResultDetails.Lastname);
            _result.DateofBirth.Should().Be(expectedResultDetails.DateofBirth);
            _result.ProviderName.Should().Be(expectedResultDetails.ProviderName);
            _result.ProviderUkprn.Should().Be(expectedResultDetails.ProviderUkprn);
            _result.TlevelTitle.Should().Be(expectedResultDetails.TlevelTitle);
            _result.PathwayLarId.Should().Be(expectedResultDetails.PathwayLarId);
            _result.PathwayName.Should().Be(expectedResultDetails.PathwayName);
            _result.PathwayAssessmentSeries.Should().Be(expectedResultDetails.PathwayAssessmentSeries);
            _result.AppealEndDate.Should().Be(expectedResultDetails.AppealEndDate);
            _result.PathwayAssessmentId.Should().Be(expectedResultDetails.PathwayAssessmentId);
            _result.PathwayResultId.Should().Be(expectedResultDetails.PathwayResultId);
            _result.PathwayResult.Should().Be(expectedResultDetails.PathwayResult);
            _result.PathwayResultCode.Should().Be(expectedResultDetails.PathwayResultCode);
            _result.PathwayPrsStatus.Should().Be(expectedResultDetails.PathwayPrsStatus);
            _result.Status.Should().Be(expectedResultDetails.Status);
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

                    // Uln: 1111111114 - Registration(Active), TqPathwayAssessments(Active) and Results
                    new object[] { 10011881, 1111111114, 4, RegistrationPathwayStatus.Active, true },
                };
            }
        }
    }
}
