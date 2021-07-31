using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.PrintingServiceTests
{
    public abstract class PrintingServiceBaseTest : BaseTest<Batch>
    {
        // Seed objects.
        protected TlAwardingOrganisation TlAwardingOrganisation;
        protected TlRoute Route;
        protected TlPathway Pathway;
        protected TlSpecialism Specialism;
        protected TqAwardingOrganisation TqAwardingOrganisation;
        protected IEnumerable<TlProvider> TlProviders;
        protected TqProvider TqProvider;
        protected IList<TlLookup> TlLookup;
        protected IList<TqProvider> TqProviders;

        protected ResultsAndCertificationConfiguration Configuration;
        protected IMapper PrintingServiceMapper;

        // Dependencies.
        protected IRepository<Batch> BatchRepository;
        protected ILogger<GenericRepository<Batch>> BatchRepositoryLogger;
        
        protected IRepository<PrintBatchItem> PrintBatchItemRepository;
        protected ILogger<GenericRepository<PrintBatchItem>> PrintBatchItemRepositoryLogger;

        protected IPrintingRepository PrintingRepository;
        protected ILogger<IPrintingRepository> PrintingRepositoryLogger;

        protected IAsyncNotificationClient NotificationsClient;
        protected ILogger<NotificationService> NotificationLogger;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected ILogger<GenericRepository<NotificationTemplate>> NotificationTemplateRepositoryLogger;
        protected ILogger<INotificationService> NotificationServiceLogger;
        protected INotificationService NotificationService;

        protected ILogger<IPrintingService> PrintingServiceLogger;
        protected IPrintingService PrintingService;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(PrintingServiceMapper).Assembly));
            PrintingServiceMapper = new Mapper(mapperConfig);
        }

        protected virtual void SeedTestData(EnumAwardingOrganisation awardingOrganisation = EnumAwardingOrganisation.Pearson, bool seedMultipleProviders = false)
        {
            TlAwardingOrganisation = TlevelDataProvider.CreateTlAwardingOrganisation(DbContext, awardingOrganisation);
            Route = TlevelDataProvider.CreateTlRoute(DbContext, awardingOrganisation);
            Pathway = TlevelDataProvider.CreateTlPathway(DbContext, awardingOrganisation, Route);
            Specialism = TlevelDataProvider.CreateTlSpecialisms(DbContext, awardingOrganisation, Pathway).First();
            TqAwardingOrganisation = TlevelDataProvider.CreateTqAwardingOrganisation(DbContext, Pathway, TlAwardingOrganisation);
            TlProviders = ProviderDataProvider.CreateTlProviders(DbContext);
            TqProvider = ProviderDataProvider.CreateTqProvider(DbContext, TqAwardingOrganisation, TlProviders.First());

            foreach (var provider in TlProviders)
            {
                TlProviderAddressDataProvider.CreateTlProviderAddress(DbContext, new TlProviderAddressBuilder().Build(provider));
            }

            DbContext.SaveChangesAsync();
        }

        public TqRegistrationProfile SeedRegistrationDataByStatus(long uln, RegistrationPathwayStatus status = RegistrationPathwayStatus.Active, TqProvider tqProvider = null)
        {
            var profile = new TqRegistrationProfileBuilder().BuildList().FirstOrDefault(p => p.UniqueLearnerNumber == uln);
            var tqRegistrationProfile = RegistrationsDataProvider.CreateTqRegistrationProfile(DbContext, profile);
            var tqRegistrationPathway = RegistrationsDataProvider.CreateTqRegistrationPathway(DbContext, tqRegistrationProfile, tqProvider ?? TqProviders.First());
            tqRegistrationPathway.Status = status;

            var tqRegistrationSpecialism = RegistrationsDataProvider.CreateTqRegistrationSpecialism(DbContext, tqRegistrationPathway, Specialism);

            if (status == RegistrationPathwayStatus.Withdrawn)
            {
                tqRegistrationPathway.EndDate = DateTime.UtcNow.AddDays(-1);
                tqRegistrationSpecialism.IsOptedin = true;
                tqRegistrationSpecialism.EndDate = DateTime.UtcNow.AddDays(-1);
            }

            DbContext.SaveChanges();
            return profile;
        }

        public PrintCertificate BuildPrintCertificate(TqRegistrationPathway tqRegistrationPathway)
        {
            var printCertificate = PrintCertificateDataProvider.CreatePrintCertificate(DbContext, new PrintCertificateBuilder().Build(null, tqRegistrationPathway));
            printCertificate.Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber;
            printCertificate.LearningDetails = JsonConvert.SerializeObject(new LearningDetails());
            return printCertificate;
        }

        public PrintCertificate SeedPrintCertificate(TqRegistrationPathway tqRegistrationPathway)
        {
            var printCertificate = PrintCertificateDataProvider.CreatePrintCertificate(DbContext, new PrintCertificateBuilder().Build(null, tqRegistrationPathway));
            printCertificate.Uln = tqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber;
            printCertificate.LearningDetails = JsonConvert.SerializeObject(new LearningDetails());
            DbContext.SaveChanges();
            return printCertificate;
        }

        public List<PrintCertificate> SeedPrintCertificates(IList<TqRegistrationPathway> tqRegistrationPathway)
        {
            var printCertificates = PrintCertificateDataProvider.CreatePrintCertificate(DbContext, new PrintCertificateBuilder().BuildList(tqRegistrationPathway));
            DbContext.SaveChanges();
            return printCertificates.ToList();
        }
    }
}
