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
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

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

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            DbContext.SaveChanges();

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);

            ResultService = new ResultService(AssessmentRepository, ResultMapper);
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

            var expectedAssessmentDetails = new ResultDetails
            {
                ProfileId = expectedRegistration.Id,
                Uln = expectedRegistration.UniqueLearnerNumber,
                Firstname = expectedRegistration.Firstname,
                Lastname = expectedRegistration.Lastname,
                ProviderUkprn = expectedPathway.TqProvider.TlProvider.UkPrn,
                ProviderName = expectedPathway.TqProvider.TlProvider.Name,                
                Status = expectedPathway.Status
            };

            // Assert
            _result.ProfileId.Should().Be(expectedAssessmentDetails.ProfileId);
            _result.Uln.Should().Be(expectedAssessmentDetails.Uln);
            _result.Firstname.Should().Be(expectedAssessmentDetails.Firstname);
            _result.Lastname.Should().Be(expectedAssessmentDetails.Lastname);
            _result.ProviderUkprn.Should().Be(expectedAssessmentDetails.ProviderUkprn);
            _result.ProviderName.Should().Be(expectedAssessmentDetails.ProviderName);           
            _result.Status.Should().Be(expectedAssessmentDetails.Status);
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
                };
            }
        }
    }
}
