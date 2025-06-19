using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.IndustryPlacementNotificationServiceTests
{
    public abstract class IndustryPlacementNotificationServiceBaseTest : BaseTest<IndustryPlacementNotificationService>
    {
        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected IRepository<TlProvider> ProviderRepository;
        protected IDfeSignInApiClient DfeSignInApiClient;
        protected INotificationService NotificationService;
        protected IAsyncNotificationClient NotificationClient;
        protected ICommonRepository CommonRepository;
        protected IndustryPlacementNotificationService IndustryPlacementNotificationService;
        ILogger<IndustryPlacementNotificationService> Logger;

        public override void Setup()
        {
            RegistrationPathwayRepository = Substitute.For<IRepository<TqRegistrationPathway>>();
            NotificationTemplateRepository = Substitute.For<IRepository<NotificationTemplate>>();
            ProviderRepository = Substitute.For<IRepository<TlProvider>>();
            DfeSignInApiClient = Substitute.For<IDfeSignInApiClient>();
            NotificationService = Substitute.For<INotificationService>();
            NotificationClient = Substitute.For<IAsyncNotificationClient>();
            CommonRepository = Substitute.For<ICommonRepository>();
            Logger = Substitute.For<ILogger<IndustryPlacementNotificationService>>();

            IndustryPlacementNotificationService = new(
                    RegistrationPathwayRepository,
                    NotificationTemplateRepository,
                    ProviderRepository,
                    CommonRepository,
                    DfeSignInApiClient,
                    NotificationService,
                    NotificationClient,
                    Logger);
        }
    }
}
