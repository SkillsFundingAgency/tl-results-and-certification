using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.OverallResultCalculationServiceTests
{
    public class When_HasAnySpecialismResultPrsStatusOutstanding_IsCalled : OverallResultCalculationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;
        private List<TqSpecialismResult> _specialismResults;
        private PrsStatus? _actualResult;

        public override void Given()
        {

            // Parameters
            _ulns = new Dictionary<long, RegistrationPathwayStatus>
            {
                { 1111111111, RegistrationPathwayStatus.Withdrawn },
                { 1111111112, RegistrationPathwayStatus.Active },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active },
                { 1111111116, RegistrationPathwayStatus.Active },
            };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsData(_ulns, TqProvider);

            // Assessments seed            
            var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
            _specialismResults = new List<TqSpecialismResult>();

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != 1111111111))
            {
                var hasHitoricData = new List<long> { 1111111112 };
                var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;

                // Results seed

                var profilesWithResults = new List<(long, PrsStatus?)> { (1111111112, null), (1111111113, PrsStatus.UnderReview), (1111111114, PrsStatus.Reviewed), (1111111115, PrsStatus.BeingAppealed), (1111111115, PrsStatus.Final) };

                foreach (var pathway in registration.TqRegistrationPathways)
                {
                    var specialismAssessments = GetSpecialismAssessmentsDataToProcess(pathway.TqRegistrationSpecialisms.ToList(), isLatestActive, isHistoricAssessent);
                    tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);

                    foreach (var specialismAssessment in specialismAssessments)
                    {
                        // Specialism Results
                        var prsStatus = profilesWithResults.FirstOrDefault(p => p.Item1 == specialismAssessment.TqRegistrationSpecialism.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber).Item2;
                        _specialismResults.AddRange(GetSpecialismResultDataToProcess(specialismAssessment, isLatestActive, isHistoricAssessent, prsStatus));
                    }
                }
            }

            SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

            DbContext.SaveChanges();

            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultBatchSettings = new OverallResultBatchSettings
                {
                    BatchSize = 10,
                    NoOfAcademicYearsToProcess = 4
                }
            };

            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);

            OverallGradeLookupLogger = new Logger<GenericRepository<OverallGradeLookup>>(new NullLoggerFactory());
            OverallGradeLookupRepository = new GenericRepository<OverallGradeLookup>(OverallGradeLookupLogger, DbContext);

            AssessmentSeriesLogger = new Logger<GenericRepository<AssessmentSeries>>(new NullLoggerFactory());
            AssessmentSeriesRepository = new GenericRepository<AssessmentSeries>(AssessmentSeriesLogger, DbContext);

            OverallResultCalculationRepository = new OverallResultCalculationRepository(DbContext);

            OverallResultCalculationService = new OverallResultCalculationService(ResultsAndCertificationConfiguration, TlLookupRepository, OverallGradeLookupRepository, OverallResultCalculationRepository, AssessmentSeriesRepository);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(TqRegistrationSpecialism request)
        {
            await Task.CompletedTask;
            _actualResult = OverallResultCalculationService.HasAnySpecialismResultPrsStatusOutstanding(request);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(long uln, PrsStatus? expectedResult)
        {
            var pathway = _registrations.Where(x => x.UniqueLearnerNumber == uln).SelectMany(x => x.TqRegistrationPathways).FirstOrDefault();
            var specialism = pathway.TqRegistrationSpecialisms.FirstOrDefault();
            await WhenAsync(specialism);

            _actualResult.Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    new object[] { 1111111111, null},
                    new object[] { 1111111112, null},
                    new object[] { 1111111113, PrsStatus.UnderReview},
                    new object[] { 1111111114, null},
                    new object[] { 1111111115, PrsStatus.BeingAppealed},
                    new object[] { 1111111116, null}
                };
            }
        }
    }
}
