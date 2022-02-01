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
    public class When_AddResultAsync_IsCalled_For_Specialism : ResultServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AddResultResponse _actualResult;
        private List<TqRegistrationProfile> _registrations;
        private List<TqSpecialismAssessment> _specialismAssessments;

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

            // Assessments seed
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                tqSpecialismAssessmentsSeedData.AddRange(GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList(), isLatestActive, isHistoricAssessent));
            }

            _specialismAssessments = SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            var assessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == 1111111114);
            if (assessment != null)
            {
                SeedSpecialismResultsData(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }), false);
            }

            DbContext.SaveChanges();

            PathwayResultRepositoryLogger = new Logger<GenericRepository<TqPathwayResult>>(new NullLoggerFactory());
            PathwayResultRepository = new GenericRepository<TqPathwayResult>(PathwayResultRepositoryLogger, DbContext);

            SpecialismResultRepositoryLogger = new Logger<GenericRepository<TqSpecialismResult>>(new NullLoggerFactory());
            SpecialismResultRepository = new GenericRepository<TqSpecialismResult>(SpecialismResultRepositoryLogger, DbContext);

            AssessmentSeriesRepositoryLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesRepositoryLogger, DbContext);

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            ResultRepositoryLogger = new Logger<ResultRepository>(new NullLoggerFactory());
            ResultRepository = new ResultRepository(ResultRepositoryLogger, DbContext);

            ResultServiceLogger = new Logger<ResultService>(new NullLoggerFactory());

            ResultService = new ResultService(AssessmentSeriesRepository, TlLookupRepository, ResultRepository, PathwayResultRepository, SpecialismResultRepository, ResultMapper, ResultServiceLogger);
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
            var assessmentId = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId)?.Id;

            if (assessmentId != null)
                request.AssessmentId = assessmentId.Value;

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

                    // When core entry type - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 3, ComponentType = ComponentType.Core },
                      new AddResultResponse { IsSuccess = false } },                    

                    // valid request - returns true
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 3, LookupId = 1, ComponentType = ComponentType.Specialism },
                      new AddResultResponse { IsSuccess = true, Uln = 1111111113, ProfileId = 3 } },

                    // invalid request (Active result exists) - returns false
                    new object[]
                    { new AddResultRequest { AoUkprn = 10011881, ProfileId = 4, LookupId = 1, ComponentType = ComponentType.Specialism },
                      new AddResultResponse { IsSuccess = false } }
                };
            }
        }
    }
}
