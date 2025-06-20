using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Authentication;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Collections.Generic;
using System.Linq;
using ContractsAcademicYear = Sfa.Tl.ResultsAndCertification.Models.Contracts.Common.AcademicYear;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.IndustryPlacementNotificationServiceTests
{
    public abstract class IndustryPlacementNotificationServiceBaseTest : BaseTest<IndustryPlacementNotificationService>
    {

        protected ResultsAndCertificationDbContext DbContext;

        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepository;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected IRepository<TlProvider> ProviderRepository;
        protected IDfeSignInApiClient DfeSignInApiClient;
        protected INotificationService NotificationService;
        protected IAsyncNotificationClient NotificationClient;
        protected ICommonRepository CommonRepository;
        protected IndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ILogger<IndustryPlacementNotificationService> Logger;

        public override void Setup()
        {
            NotificationTemplateRepository = Substitute.For<IRepository<NotificationTemplate>>();
            ProviderRepository = Substitute.For<IRepository<TlProvider>>();
            DfeSignInApiClient = Substitute.For<IDfeSignInApiClient>();
            NotificationService = Substitute.For<INotificationService>();
            NotificationClient = Substitute.For<IAsyncNotificationClient>();
            CommonRepository = Substitute.For<ICommonRepository>();
            Logger = Substitute.For<ILogger<IndustryPlacementNotificationService>>();
        }
        public static ResultsAndCertificationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ResultsAndCertificationDbContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .EnableSensitiveDataLogging()
           .Options;

            return new ResultsAndCertificationDbContext(options);
        }

        protected void CreateData()
        {
            DbContext = CreateInMemoryDbContext();

            DbContext.Add(new TqRegistrationProfile()
            {
                Id = 1,
                UniqueLearnerNumber = 3333333333,
            });

            DbContext.Add(new TqRegistrationPathway()
            {
                Id = 1,
                TqRegistrationProfileId = 1,
                TqProviderId = 1,
                AcademicYear = 2021,
                EndDate = null,
                Status = RegistrationPathwayStatus.Active,
                IsPendingWithdrawal = false
            });

            DbContext.Add(new TqProvider()
            {
                Id = 1,
                TlProviderId = 1,
                TqAwardingOrganisationId = 1,
            });

            DbContext.Add(new TlProvider()
            {
                Id = 1,
                UkPrn = 10000001,
                Name = "Test Provider",
                DisplayName = "Test Provider"

            });
            DbContext.SaveChanges();
        }

        protected List<DfeUsers> CreateDfeUsers() =>
            new List<DfeUsers>()
            {
                new DfeUsers() {
                    Ukprn = "10000001",
                    Users = new List<ServiceUser>() {
                    new ServiceUser() {
                        Email = "test@test.com"}
                    }
                }
            };

        protected IQueryable<ContractsAcademicYear> CreateAcademicYears() =>
             new List<ContractsAcademicYear>
            {
                new ContractsAcademicYear { Id = 1, Year = 2022, Name = "Summer 2022" },
                new ContractsAcademicYear { Id = 2, Year = 2023, Name = "Summer 2023" }
            }.AsQueryable();
    }
}
