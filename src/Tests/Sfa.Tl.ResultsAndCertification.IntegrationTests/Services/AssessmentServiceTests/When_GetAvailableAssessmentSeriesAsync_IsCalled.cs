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
    public class When_GetAvailableAssessmentSeriesAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AvailableAssessmentSeries _actualResult;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;
        private List<TqSpecialismAssessment> _specialismAssessments;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Active }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Withdrawn } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqPathwayAssessmentsSeedData.AddRange(GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive, isHistoricAssessent));

                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent));
                }
            }

            _pathwayAssessments = SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
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

        public async Task WhenAsync(long aoUkprn, int profileId, ComponentType componentType)
        {
            if (_actualResult != null)
                return;

            _actualResult = await AssessmentService.GetAvailableAssessmentSeriesAsync(aoUkprn, profileId, componentType);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(RequestParameter request, AvailableAssessmentSeries expectedResult)
        {
            await WhenAsync(request.AoUkprn, request.ProfileId, request.ComponentType);

            if (_actualResult == null)
            {
                expectedResult.Should().BeNull();
                return;
            }

            // Assert
            _actualResult.ProfileId.Should().Be(expectedResult.ProfileId);
            _actualResult.AssessmentSeriesId.Should().Be(expectedResult.AssessmentSeriesId);
            _actualResult.AssessmentSeriesName.Should().Be(expectedResult.AssessmentSeriesName);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // core assessment window opend
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Core },
                      new AvailableAssessmentSeries { ProfileId = 1, AssessmentSeriesId = 1, AssessmentSeriesName = "Summer 2021" } },

                    // specialism assessment window not opened
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Specialism },
                      new AvailableAssessmentSeries { ProfileId = 1, AssessmentSeriesId = 7, AssessmentSeriesName = "Summer 2022" } },

                    //// Has an active assessment - TODO: Ravi
                    //new object[]
                    //{ new RequestParameter { AoUkprn = 10011881, ProfileId = 2, ComponentType = ComponentType.Core },
                    //  null },

                    // registration is withdrawn
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core },
                      null },

                    // invalid profil id
                    new object[]
                    { new RequestParameter { AoUkprn = 10011881, ProfileId = 99, ComponentType = ComponentType.Core },
                      null },
                };
            }
        }
    }

    public class RequestParameter
    {
        public long AoUkprn { get; set; }
        public int ProfileId { get; set; }
        public ComponentType ComponentType { get; set; }
    }
}
