using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.NotificationServiceTests
{
    public abstract class NotificationServiceBaseTest : BaseTest<NotificationTemplate>
    {
        protected IMapper Mapper;
        protected ILogger<NotificationService> NotificationLogger;
        protected NotificationService NotificationService;
        protected NotificationTemplate NotificationTemplate;
        protected IAsyncNotificationClient NotificationsClient;
        protected string ToAddress;
        protected bool Result;

        protected virtual void SeedNotificationTestData()
        {
            NotificationTemplate = NotificationDataProvider.CreateNotificationTemplate(DbContext);
            DbContext.SaveChangesAsync();
        }
    }
}
