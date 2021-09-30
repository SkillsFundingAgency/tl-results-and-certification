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
    public class When_AddAssessmentEntryAsync_IsCalled : AssessmentServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AddAssessmentEntryResponse _actualResult;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Withdrawn }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Active }, { 1111111114, RegistrationPathwayStatus.Active } };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            var currentYearUln = new List<long> { 1111111114 };
            RegisterUlnForNextAcademicYear(_registrations, currentYearUln);

            // Assessments seed
            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber == 1111111112))
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

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, false);
            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);
            DbContext.SaveChanges();

            AssessmentRepositoryLogger = new Logger<AssessmentRepository>(new NullLoggerFactory());
            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentRepository = new AssessmentRepository(AssessmentRepositoryLogger, DbContext);
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);
            PathwayAssessmentRepositoryLogger = new Logger<GenericRepository<TqPathwayAssessment>>(new NullLoggerFactory());
            PathwayAssessmentRepository = new GenericRepository<TqPathwayAssessment>(PathwayAssessmentRepositoryLogger, DbContext);

            AssessmentService = new AssessmentService(AssessmentRepository, PathwayAssessmentRepository, SpecialismAssessmentRepository, AssessmentSeriesRepository, AssessmentMapper, AssessmentRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AddAssessmentEntryRequest request)
        {
            if (_actualResult != null)
                return;

            _actualResult = await AssessmentService.AddAssessmentEntryAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(AddAssessmentEntryRequest request, AddAssessmentEntryResponse expectedResult)
        {
            await WhenAsync(request);

            // Assert
            _actualResult.IsSuccess.Should().Be(expectedResult.IsSuccess);
            if (_actualResult.IsSuccess)
                _actualResult.Uln.Should().Be(expectedResult.Uln);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                     //Profile not-found - returns false
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 999  },
                      new AddAssessmentEntryResponse { IsSuccess = false } },

                    // Registration not in active status - returns false
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 1  },
                      new AddAssessmentEntryResponse { IsSuccess = false } },

                    // Reg has an active assessment already - returns false
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 2  },
                      new AddAssessmentEntryResponse { IsSuccess = false } },

                    // When specialism entry type - returns false
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Specialism },
                      new AddAssessmentEntryResponse { IsSuccess = false } },

                    // valid request - returns true
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core },
                      new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1111111113 } },

                     // There is no assessment entry window open.
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Core },
                      new AddAssessmentEntryResponse { IsSuccess = false, Uln = 1111111114} }

                };
            }
        }
    }
}
