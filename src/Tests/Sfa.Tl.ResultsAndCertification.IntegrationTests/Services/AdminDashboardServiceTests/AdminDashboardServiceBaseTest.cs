using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Service;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public abstract class AdminDashboardServiceBaseTest : BaseTest<TqRegistrationPathway>
    {
        protected TqProvider TqProvider;

        private IList<TlSpecialism> _specialisms;
        private IList<AcademicYear> _academicYears;

        protected IAdminDashboardService AdminDashboardService;

        protected void CreateAdminDasboardService()
        {
            var adminDashboardRepository = new AdminDashboardRepository(DbContext);
            var repositoryFactory = new RepositoryFactory(new NullLoggerFactory(), DbContext);
            var systemProvider = new SystemProvider();
            var mapper = CreateMapper();

            AdminDashboardService = new AdminDashboardService(adminDashboardRepository, repositoryFactory, systemProvider, mapper);
        }

        private static Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(LearnerMapper).Assembly));
            return new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            var tlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            var route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            var _pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, route);
            _specialisms = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, _pathway);
            var tqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, _pathway, tlAwardingOrganisation);
            var tlProvider = ProviderDataProvider.CreateTlProvider(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, tqAwardingOrganisation, tlProvider);
            AssessmentSeriesDataProvider.CreateAssessmentSeriesList(DbContext, null, true);
            TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            _academicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);

            DbContext.SaveChangesAsync();
        }

        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider, isCouplet));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null, bool isCouplet = false)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProvider);
            var tqRegistrationSpecialisms = isCouplet ? RegistrationsDataProvider.CreateTqRegistrationSpecialisms(DbContext, tqRegistrationPathway)
                : new List<TqRegistrationSpecialism> { RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, _specialisms.First()) };

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                foreach (var tqRegistrationSpecialism in tqRegistrationSpecialisms)
                {
                    tqRegistrationSpecialism.IsOptedin = true;
                    tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
                }
            }

            DbContext.SaveChanges();
            return profile;
        }

        public void RegisterUlnForNextAcademicYear(List<TqRegistrationProfile> _registrations, IList<long> ulns)
        {
            ulns.ToList().ForEach(uln =>
            {
                var registration = _registrations.FirstOrDefault(x => x.UniqueLearnerNumber == uln);
                registration.TqRegistrationPathways.FirstOrDefault().AcademicYear = _academicYears.FirstOrDefault(x => DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate).Year + 1;
            });
        }

        public void SeedSpecialConsiderationsLookupData()
        {
            IpLookupDataProvider.CreateIpLookupList(DbContext, null, IpLookupType.SpecialConsideration, true);
            DbContext.SaveChanges();
        }
    }
}