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
            _ulns = new Dictionary<long, RegistrationPathwayStatus> 
            { 
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active } 
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            var coupletUln = 1111111115;
            _registrations.Add(SeedRegistrationData(coupletUln, RegistrationPathwayStatus.Active, null, true));

            var currentYearUln = new List<long> { 1111111114 };
            RegisterUlnForNextAcademicYear(_registrations, currentYearUln);

            var secondCohortUln = 1111111116;
            var secondCohortRegistration = SeedRegistrationData(secondCohortUln, RegistrationPathwayStatus.Active, null, true);
            secondCohortRegistration.TqRegistrationPathways.FirstOrDefault().AcademicYear = 2021;
            _registrations.Add(secondCohortRegistration);

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
            SpecialismAssessmentRepositoryLogger = new Logger<GenericRepository<TqSpecialismAssessment>>(new NullLoggerFactory());
            SpecialismAssessmentRepository = new GenericRepository<TqSpecialismAssessment>(SpecialismAssessmentRepositoryLogger, DbContext);

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
            var currentAssessmentSeries = AssessmentSeries.FirstOrDefault(a => a.ComponentType == request.ComponentType);

            if(currentAssessmentSeries != null)
            request.AssessmentSeriesId = currentAssessmentSeries.Id;

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
                    //// Profile not-found - returns false
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 999, ComponentType = ComponentType.Core  },
                    //  new AddAssessmentEntryResponse { IsSuccess = false } },

                    //// Registration not in active status - returns false
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 1, ComponentType = ComponentType.Core  },
                    //  new AddAssessmentEntryResponse { IsSuccess = false } },

                    //// Reg has an active assessment already - returns false
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 2, ComponentType = ComponentType.Core  },
                    //  new AddAssessmentEntryResponse { IsSuccess = false } },

                    //// When specialism entry type - returns false
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Specialism, SpecialismIds = new List<int?> { 1 } },
                    //  new AddAssessmentEntryResponse { IsSuccess = false } },

                    //// valid request - returns true
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core },
                    //  new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1111111113 } },

                    //// There is no assessment entry window open.
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 4, ComponentType = ComponentType.Core },
                    //  new AddAssessmentEntryResponse { IsSuccess = false, Uln = 1111111114} },

                    //// When specialism entry type couplet - returns true
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.Specialism, SpecialismIds = new List<int?> { 5, 6 } },
                    //  new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1111111115 } },

                    //// Component type not specified
                    //new object[]
                    //{ new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 5, ComponentType = ComponentType.NotSpecified, SpecialismIds = new List<int?> { 1, 2 } },
                    //  new AddAssessmentEntryResponse { IsSuccess = false } },

                    // When specialism entry type for 2nd Cohort - returns true
                    new object[]
                    { new AddAssessmentEntryRequest { AoUkprn = 10011881, ProfileId = 6, ComponentType = ComponentType.Specialism, SpecialismIds = new List<int?> { 7, 8 } },
                      new AddAssessmentEntryResponse { IsSuccess = true, Uln = 1111111116 } },
                };
            }
        }
    }
}
