using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminNotificationServiceTests
{
    public abstract class AdminNotificationServiceBaseTest : BaseTest<Notification>
    {
        protected IAdminNotificationService AdminNotificationService => CreateAdminNotificationService();

        private IAdminNotificationService CreateAdminNotificationService()
        {
            var bannerRepository = new AdminNotificationRepository(DbContext);
            var logger = new Logger<GenericRepository<Notification>>(new NullLoggerFactory());
            var repository = new GenericRepository<Notification>(logger, DbContext);
            IMapper mapper = CreateMapper();

            return new AdminNotificationService(bannerRepository, repository, mapper);
        }

        private static Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(AdminProviderMapper).Assembly));
            return new Mapper(mapperConfig);
        }

        protected IList<Notification> NotificationsDb = new List<Notification> { FirstNotification };

        protected static Notification FirstNotification
            => new()
            {
                Id = 1,
                Title = "notification-title",
                Content = "notification-content",
                Target = NotificationTarget.Both,
                Start = new DateTime(2024, 1, 1),
                End = new DateTime(2024, 1, 31),
                CreatedBy = "notification-created-by",
                CreatedOn = new DateTime(2024, 1, 1),
                ModifiedBy = "notification-modified-by",
                ModifiedOn = null
            };

        protected void SeedTestData()
        {
            DbContext.Notification.Add(FirstNotification);
            DbContext.SaveChanges();
        }
    }
}