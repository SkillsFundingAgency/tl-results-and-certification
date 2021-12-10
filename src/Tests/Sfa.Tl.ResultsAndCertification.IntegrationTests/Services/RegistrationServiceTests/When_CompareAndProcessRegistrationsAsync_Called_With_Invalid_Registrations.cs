using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations.ValidationErrorsBuilder;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.RegistrationServiceTests
{
    public class When_CompareAndProcessRegistrationsAsync_Called_With_Invalid_Registrations : RegistrationServiceBaseTest
    {
        private RegistrationProcessResponse _result;
        private IList<TqRegistrationProfile> _tqRegistrationProfilesData;
        private IList<BulkProcessValidationError> _expectedValidationErrors;

        public override void Given()
        {
            CreateMapper();
            // Seed Tlevel data for pearson
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedRegistrationData(1111111111);
            SeedRegistrationData(1111111115, RegistrationPathwayStatus.Withdrawn);

            // Seed Tlevel data for ncfe
            SeedTestData(EnumAwardingOrganisation.Ncfe, true);
            SeedRegistrationData(1111111112);
            SeedRegistrationData(1111111113, RegistrationPathwayStatus.Withdrawn);
            
            // Seed Profile with specialisms and assessment entry
            var registrationProfileWithSpecialism = SeedRegistrationData(1111111116, RegistrationPathwayStatus.Active, TqProviders[TqProviders.Count - 1]);
            SeedSpecialismAssessmentsData(GetSpecialismAssessmentsDataToProcess(registrationProfileWithSpecialism.TqRegistrationPathways.SelectMany(p => p.TqRegistrationSpecialisms).ToList()));

            ProviderRepositoryLogger = new Logger<ProviderRepository>(new NullLoggerFactory());
            RegistrationRepositoryLogger = new Logger<RegistrationRepository>(new NullLoggerFactory());
            TqRegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            TqRegistrationSpecialismRepositoryLogger = new Logger<GenericRepository<TqRegistrationSpecialism>>(new NullLoggerFactory());
            ProviderRepository = new ProviderRepository(ProviderRepositoryLogger, DbContext);
            RegistrationRepository = new RegistrationRepository(RegistrationRepositoryLogger, DbContext);
            TqRegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(TqRegistrationPathwayRepositoryLogger, DbContext);
            TqRegistrationSpecialismRepository = new GenericRepository<TqRegistrationSpecialism>(TqRegistrationSpecialismRepositoryLogger, DbContext);
            CreateCommonService();
            RegistrationService = new RegistrationService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, TqRegistrationSpecialismRepository, CommonService, RegistrationMapper, RegistrationRepositoryLogger);

            _tqRegistrationProfilesData = GetRegistrationsDataToProcess(new List<long> { 1111111111, 1111111112, 1111111113, 1111111114, 1111111115, 1111111116 });
            _expectedValidationErrors = new BulkRegistrationValidationErrorsBuilder().BuildStage4ValidationErrorsList();
        }

        public async override Task When()
        {
            _result = await RegistrationService.CompareAndProcessRegistrationsAsync(_tqRegistrationProfilesData);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _result.Should().NotBeNull();
            _result.ValidationErrors.Should().NotBeNullOrEmpty();
            _result.ValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            _result.ValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }

        protected override void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            if(AcademicYears == null || AcademicYears.Count == 0)
                AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);

            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var routes = TlevelDataProvider.CreateTlRoutes(DbContext, awardingOrganisation);
            var pathways = TlevelDataProvider.CreateTlPathways(DbContext, awardingOrganisation, routes);
            Specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, pathways.First());

            var tqAwardingOrganisations = TlevelDataProvider.CreateTqAwardingOrganisations(DbContext, awardingOrganisation, TlAwardingOrganisation, pathways);

            TqProviders = new List<TqProvider>();
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext).ToList();

            var tlProvider = TlProviders.FirstOrDefault();
            foreach (var tqAwardingOrganisation in tqAwardingOrganisations)
            {
                TqProviders.Add(ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider));
            }
            AssessmentSeries = AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }

        private TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialisms.First());

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);

                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChangesAsync();
            return tqRegistrationProfile;
        }

        private List<TqRegistrationProfile> GetRegistrationsDataToProcess(List<long> ulns)
        {
            var tqRegistrationProfiles = new List<TqRegistrationProfile>();

            foreach (var (uln, index) in ulns.Select((value, i) => (value, i)))
            {
                var tqProvider = index < TqProviders.Count ? TqProviders[index] : TqProviders[TqProviders.Count - 1];
                var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
                var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
                var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider);

                tqRegistrationProfile.TqRegistrationPathways = new List<TqRegistrationPathway> { tqRegistrationPathway };
                tqRegistrationProfiles.Add(tqRegistrationProfile);
            }
            return tqRegistrationProfiles;
        }
    }
}
