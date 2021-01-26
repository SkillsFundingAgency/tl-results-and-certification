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
    public class When_AddResultAsync_IsCalled : ResultServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AddResultResponse _actualResult;
        private List<TqRegistrationProfile> _registrations;
        private List<TqPathwayAssessment> _pathwayAssessments;

        public override void Given()
        {
            // Create mapper
            CreateMapper();

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus> { { 1111111111, RegistrationPathwayStatus.Withdrawn }, { 1111111112, RegistrationPathwayStatus.Active }, { 1111111113, RegistrationPathwayStatus.Active } };

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

            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, ResultMapper);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(AddResultRequest request)
        {
            if (_actualResult != null)
                return;

            _actualResult = await ResultService.AddResultAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(AddResultRequest request, AddResultResponse expectedResult)
        {
           var assessmentId = _pathwayAssessments.FirstOrDefault(x => x.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId)?.Id;

            if (assessmentId != null)
                request.TqPathwayAssessmentId = assessmentId.Value;

            await WhenAsync(request);

            // Assert
            _actualResult.IsSuccess.Should().Be(expectedResult.IsSuccess);
            if (_actualResult.IsSuccess)
            {
                _actualResult.Uln.Should().Be(expectedResult.Uln);
                _actualResult.ProfileId.Should().Be(expectedResult.ProfileId);
            }
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                     //Profile not-found - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 999  },
                      new AddResultResponse { IsSuccess = false } },

                    // Registration not in active status - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 1  },
                      new AddResultResponse { IsSuccess = false } },

                    // Reg has an active assessment already - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 2  },
                      new AddResultResponse { IsSuccess = false } },

                    // When specialism entry type - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 3, AssessmentEntryType = AssessmentEntryType.Specialism },
                      new AddResultResponse { IsSuccess = false } },                    

                    // valid request - returns true
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 3, TlLookupId = 1, AssessmentEntryType = AssessmentEntryType.Core },
                      new AddResultResponse { IsSuccess = true, Uln = 1111111113, ProfileId = 3 } }
                };
            }
        }
    }
}
