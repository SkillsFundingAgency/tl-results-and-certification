using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.IndustryPlacementNotificationServiceTests
{
    public abstract class IndustryPlacementNotificationServiceBaseTest : BaseTest<TqRegistrationProfile>
    {
        protected int? PathwayId;

        // Data Seed variables
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected TqProvider TqProvider;
        protected IList<AcademicYear> AcademicYears;
        protected IEnumerable<TlProvider> TlProviders;
        protected IList<TqProvider> TqProviders;
        protected IList<AssessmentSeries> AssessmentSeries;
        protected IList<TlLookup> TlLookup;
        protected IList<TlLookup> PathwayComponentGrades;
        protected IList<Qualification> Qualifications;

        protected IndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected IRepository<IndustryPlacement> IndustryPlacementRepository;
        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected IAsyncNotificationClient NotificationClient;
        protected IRepository<TlProvider> TlProviderRepository;
        protected ICommonRepository CommonRepository;

        // Dependencies
        protected ILogger<GenericRepository<TqRegistrationPathway>> RegistrationPathwayRepositoryLogger;
        protected ILogger<IndustryPlacementNotificationService> IndustryPlacementNotificationServiceLogger;

        protected INotificationService NotificationService = Substitute.For<INotificationService>();
        protected IDfeSignInApiClient DfeSignInApiClient = Substitute.For<IDfeSignInApiClient>();

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProviders.First());

            DbContext.SaveChanges();
        }
        public List<TqRegistrationProfile> SeedRegistrationsData(Dictionary<long, RegistrationPathwayStatus> ulns, TqProvider tqProvider = null)
        {
            var profiles = new List<TqRegistrationProfile>();

            foreach (var uln in ulns)
            {
                profiles.Add(SeedRegistrationData(uln.Key, uln.Value, tqProvider));
            }
            return profiles;
        }

        public TqRegistrationProfile SeedRegistrationData(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildListWithoutLrsData().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            var currentPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, 1, DateTime.Now.AddYears(-2), RegistrationPathwayStatus.Active, true);

            if (status == RegistrationPathwayStatus.Withdrawn || status == RegistrationPathwayStatus.Transferred)
            {
                tqRegistrationPathway.Status = status;
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }

        public enum Provider
        {
            BarsleyCollege = 10000536,
            WalsallCollege = 10007315
        }
    }
}