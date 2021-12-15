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
    public class When_RemoveSpecialismAssessmentEntryAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private bool _result;

        private List<TqRegistrationProfile> _registrations;
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
                { 1111111114, RegistrationPathwayStatus.Active }
            };

            // Create mapper
            CreateMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent);
                tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);
            }

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData);

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            PathwayAssessmentRepositoryLogger = new Logger<GenericRepository<TqPathwayAssessment>>(new NullLoggerFactory());
            PathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(PathwayAssessmentRepositoryLogger, DbContext);
            SpecialismAssessmentRepositoryLogger = new Logger<GenericRepository<TqSpecialismAssessment>>(new NullLoggerFactory());
            SpecialismAssessmentRepository = new GenericRepository<TqSpecialismAssessment>(SpecialismAssessmentRepositoryLogger, DbContext);
            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(long aoUkprn, long uln, bool isActiveAssessment)
        {
            TqSpecialismAssessment specialismAssessment = null;

            if (isActiveAssessment)
            {
                specialismAssessment = _specialismAssessments.FirstOrDefault(pa => pa.IsOptedin && pa.EndDate == null && pa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }
            else
            {
                specialismAssessment = _specialismAssessments.FirstOrDefault(pa => !pa.IsOptedin && pa.EndDate != null && pa.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
            }

            var assessmentId = specialismAssessment != null ? specialismAssessment.Id : 0;

            var removeAssessmentEntryRequest = new RemoveAssessmentEntryRequest { AoUkprn = aoUkprn, SpecialismAssessmentIds = new List<int?> { assessmentId }, PerformedBy = "Test User" };
            _result = await AssessmentService.RemoveSpecialismAssessmentEntryAsync(removeAssessmentEntryRequest);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long aoUkprn, long uln, bool isActiveAssessment, bool expectedResponse)
        {
            await WhenAsync(aoUkprn, uln, isActiveAssessment);

            // Assert
            _result.Should().Be(expectedResponse);
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
                    
                    // Uln: 1111111111 - Registration(Active) but no asessment entries for specialism
                    new object[] { 10011881, 1111111111, true, false },

                    // Uln: 1111111112 - Registration(Active), TqSpecialismAssessments(Active + History)
                    new object[] { 10011881, 1111111112, false, false},
                    new object[] { 10011881, 1111111112, true, true},

                    // Uln: 1111111113 - Registration(Withdrawn), TqSpecialismAssessments(Withdrawn)
                    new object[] { 10011881, 1111111113, false, false },
                    new object[] { 10011881, 1111111113, true, false },

                    /// Uln: 1111111114 - Registration(Active), TqPathwayAssessments(Active)
                    new object[] { 10011881, 1111111114, true, true },
                };
            }
        }
    }
}
