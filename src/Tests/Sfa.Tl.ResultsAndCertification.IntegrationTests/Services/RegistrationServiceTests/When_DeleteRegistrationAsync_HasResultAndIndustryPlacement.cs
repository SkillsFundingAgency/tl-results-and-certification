using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_DeleteRegistrationAsync_HasResultAndIndustryPlacement : RegistrationServiceBaseTest
    {
        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<TqRegistrationProfile> _registrations;

        public override void Given()
        {
            _ulns = new Dictionary<long, RegistrationPathwayStatus> 
            { 
                { 1111111111, RegistrationPathwayStatus.Active }, 
                { 1111111112, RegistrationPathwayStatus.Withdrawn },
                { 1111111113, RegistrationPathwayStatus.Active },
                { 1111111114, RegistrationPathwayStatus.Active },
                { 1111111115, RegistrationPathwayStatus.Active }
            };

            // Seed data
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            _registrations = SeedRegistrationsDataByStatus(_ulns, TqProvider);

            var tqPathwayAssessmentsSeedData = new List<TqPathwayAssessment>();
            var tqPathwayResultsSeedData = new List<TqPathwayResult>();
            var industryPlacementUln = 1111111114;

            foreach (var registration in _registrations.Where(x => x.UniqueLearnerNumber != industryPlacementUln))
            {
                var isLatestActive = _ulns[registration.UniqueLearnerNumber] != RegistrationPathwayStatus.Withdrawn;
                var pathwayAssessments = GetPathwayAssessmentsDataToProcess(registration.TqRegistrationPathways.ToList(), isLatestActive);
                tqPathwayAssessmentsSeedData.AddRange(pathwayAssessments);

                // Seed Pathway results
                foreach (var assessment in pathwayAssessments)
                {
                    var hasHitoricData = new List<long> { 1111111113 };
                    var isHistoricAssessent = hasHitoricData.Any(x => x == registration.UniqueLearnerNumber);
                    var isLatestActiveResult = !isHistoricAssessent; 

                    var tqPathwayResultSeedData = GetPathwayResultDataToProcess(assessment, isLatestActiveResult, isHistoricAssessent);
                    tqPathwayResultsSeedData.AddRange(tqPathwayResultSeedData);
                }

                // Specialism Assessments seed
                if (registration.UniqueLearnerNumber == 1111111115)
                {
                    var tqSpecialismAssessmentsSeedData = new List<TqSpecialismAssessment>();
                    var specialismAssessments = GetSpecialismAssessmentsDataToProcess(registration.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList());
                    tqSpecialismAssessmentsSeedData.AddRange(specialismAssessments);
                    SeedSpecialismAssessmentsData(tqSpecialismAssessmentsSeedData, false);

                    var tqSpecialismResultsSeedData = new List<TqSpecialismResult>();
                    foreach (var assessment in specialismAssessments)
                        tqSpecialismResultsSeedData.AddRange(GetSpecialismResultsDataToProcess(new List<TqSpecialismAssessment> { assessment }, isBulkUpload: false));

                    SeedSpecialismResultsData(tqSpecialismResultsSeedData, false);
                }
            }

            SeedPathwayAssessmentsData(tqPathwayAssessmentsSeedData, true);
            SeedIndustryPlacementData(industryPlacementUln, addToDbContext: true);

            foreach (var profile in _registrations)
            {
                SeedQualificationAchievedData(profile);
            }

            CreateMapper();

            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);
        }

        public override Task When()
        {
            return Task.CompletedTask;
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Then_Returns_Expected_Results(long aoUkprn, int profileId, bool expectedResponse)
        {
            bool actualResult = await RegistrationService.DeleteRegistrationAsync(aoUkprn, profileId);
            actualResult.Should().Be(expectedResponse);
        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {
                    // params { AoUkprn, profileId, expectedResult }
                    new object[] { 10011881, 1, false }, // PathwayActive  + ResultActive
                    new object[] { 10011881, 2, false }, // PathwayWidrawn + ResultInactive 
                    new object[] { 10011881, 3, true },  // PathwayActive  + ResultInactive
                    new object[] { 10011881, 4, false},  // PathwayActive  + NoResult         + IP_Exist
                    new object[] { 10011881, 5, false},  // PathwayActive  + Specialism has result
                };
            }
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, TlProvider);
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            Qualifications = SeedQualificationData();
            DbContext.SaveChangesAsync();
        }

        private void SeedIndustryPlacementData(int uln, bool addToDbContext)
        {
            var pathway = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln).TqRegistrationPathways.FirstOrDefault();
            IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, IndustryPlacementStatus.Completed, addToDbContext);
        }
    }
}
