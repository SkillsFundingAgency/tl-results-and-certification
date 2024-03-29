﻿using FluentAssertions;
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
    public class When_ChangeResultAsync_For_Specialism_IsCalled : ResultServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private AddResultResponse _actualResult;
        private List<TqRegistrationProfile> _registrations;
        private List<TqSpecialismAssessment> _specialismAssessments;
        private List<TqSpecialismResult> _specialismResults;

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
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active }
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

            var ulnsToAddResults = new List<long> { 1111111113, 1111111114, 1111111115, 1111111116 };
            var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();

            foreach (var uln in ulnsToAddResults)
            {
                var assessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber == uln);
                if (assessment != null)
                {
                    tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }));
                }
            }

            _specialismResults = SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);

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

        public async Task WhenAsync(ChangeResultRequest request)
        {
            if (_actualResult != null)
                return;

            _actualResult = await ResultService.ChangeResultAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(ChangeResultRequest request, ChangeResultResponse expectedResult)
        {
            var assessment = _specialismAssessments.FirstOrDefault(x => x.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfileId == request.ProfileId && x.IsOptedin && x.EndDate == null);

            if (assessment != null)
            {
                request.Uln = assessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber;
                var resultId = _specialismResults?.FirstOrDefault(x => x.TqSpecialismAssessmentId == assessment.Id && x.IsOptedin && x.EndDate == null)?.Id;
                if (resultId != null)
                {
                    request.ResultId = resultId.Value;
                }
            }

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
                      // Result not-found - returns false
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 999, LookupId = 2, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = false } },

                    // Registration not in active status - returns false
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 1, LookupId = 2, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = false } },

                    // No active result - returns false
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 2, LookupId = 2, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = false } },

                    // When componenttype = notspecified - returns false
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 3, LookupId = 2, ComponentType = ComponentType.NotSpecified },
                      new ChangeResultResponse { IsSuccess = false } },                    

                    // valid request with Active result - returns true
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 3, LookupId = 2, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = true, Uln = 1111111113, ProfileId = 3 } },

                    // valid request with Active result and removing result - returns true
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 4, LookupId = null, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = true, Uln = 1111111114, ProfileId = 4 } },

                    // valid request - returns true
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 5, LookupId = 5, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = true, Uln = 1111111115, ProfileId = 5 } }, // LookupId = 5 - Q - pending result

                    // valid request - returns true
                    new object[]
                    { new ChangeResultRequest { AoUkprn = 10011881, ProfileId = 6, LookupId = 6, ComponentType = ComponentType.Specialism },
                      new ChangeResultResponse { IsSuccess = true, Uln = 1111111116, ProfileId = 6 } } // LookupId = 6 - X - no result
                };
            }
        }
    }
}
