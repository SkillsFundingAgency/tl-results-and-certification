using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Service;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.AdminDashboardServiceTests
{
    public class When_ProcessReviewIndustryPlacement_IsCalled : AdminDashboardServiceBaseTest
    {

        private Dictionary<long, RegistrationPathwayStatus> _ulns;
        private List<IndustryPlacementData> _industryPlacementDatas;
        private LearnerRecord _result;
        private IList<TqRegistrationProfile> _profiles;

        public override void Given()
        {
            // Parameters
            AoUkprn = 10011881;

            // Create mapper
            CreateMapper();
            CreateCommonMapper();

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);

            _ulns = new Dictionary<long, RegistrationPathwayStatus>
        {
            { 1111111111, RegistrationPathwayStatus.Active },
            { 1111111112, RegistrationPathwayStatus.Withdrawn },
            { 1111111113, RegistrationPathwayStatus.Active },
            { 1111111114, RegistrationPathwayStatus.Active },
            { 1111111115, RegistrationPathwayStatus.Active },
            { 1111111116, RegistrationPathwayStatus.Active },
        };

            // Registrations seed
            SeedTestData(EnumAwardingOrganisation.Pearson, true);
            SeedSpecialConsiderationsLookupData();

            _profiles = SeedRegistrationsData(_ulns, TqProvider);

            _industryPlacementDatas = new List<IndustryPlacementData>
        {
            new IndustryPlacementData { Uln = 1111111111, IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted, Details = null },
            new IndustryPlacementData { Uln = 1111111114, IndustryPlacementStatus = IndustryPlacementStatus.Completed, Details = null },
            new IndustryPlacementData { Uln = 1111111115, IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = new IndustryPlacementDetails { HoursSpentOnPlacement = 100, SpecialConsiderationReasons = new List<int?> { 1, 2 } } },

        };

            SeedIndustyPlacementData(_industryPlacementDatas);

            DbContext.SaveChanges();
            DetachAll();

            Configuration = new ResultsAndCertificationConfiguration
            {
                TlevelQueriedSupportEmailAddress = "test@test.com"
            };

            CommonServiceLogger = new Logger<CommonService>(new NullLoggerFactory());
            TlLookupRepositoryLogger = new Logger<GenericRepository<TlLookup>>(new NullLoggerFactory());
            TlLookupRepository = new GenericRepository<TlLookup>(TlLookupRepositoryLogger, DbContext);
            FunctionLogRepositoryLogger = new Logger<GenericRepository<FunctionLog>>(new NullLoggerFactory());
            FunctionLogRepository = new GenericRepository<FunctionLog>(FunctionLogRepositoryLogger, DbContext);
            CommonRepository = new CommonRepository(DbContext);


            NotificationsClient = Substitute.For<IAsyncNotificationClient>();
            NotificationLogger = new Logger<NotificationService>(new NullLoggerFactory());
            NotificationTemplateRepositoryLogger = new Logger<GenericRepository<NotificationTemplate>>(new NullLoggerFactory());
            NotificationTemplateRepository = new GenericRepository<NotificationTemplate>(NotificationTemplateRepositoryLogger, DbContext);
            NotificationService = new NotificationService(NotificationTemplateRepository, NotificationsClient, NotificationLogger);
            ChangeLogRepositoryLogger = new Logger<GenericRepository<ChangeLog>>(new NullLoggerFactory());
            ChangeLogRepository = new GenericRepository<ChangeLog>(ChangeLogRepositoryLogger, DbContext);
            CommonService = new CommonService(CommonServiceLogger, CommonMapper, TlLookupRepository, FunctionLogRepository, CommonRepository, NotificationService, Configuration, ChangeLogRepository);
            SystemProvider = new SystemProvider();

            AdminDashboardRepository = new AdminDashboardRepository(DbContext);
            RegistrationPathwayRepositoryLogger = new Logger<GenericRepository<TqRegistrationPathway>>(new NullLoggerFactory());
            RegistrationPathwayRepository = new GenericRepository<TqRegistrationPathway>(RegistrationPathwayRepositoryLogger, DbContext);
            var industryPlacementRepository = Substitute.For<IRepository<Domain.Models.IndustryPlacement>>();
            var pathwayAssessmentRepository = Substitute.For<IRepository<Domain.Models.TqPathwayAssessment>>();
            var specialismAssessmentRepository = Substitute.For<IRepository<Domain.Models.TqSpecialismAssessment>>();

            AdminDashboardService = new AdminDashboardService(
                AdminDashboardRepository, 
                RegistrationPathwayRepository, 
                industryPlacementRepository, 
                pathwayAssessmentRepository,
                specialismAssessmentRepository,
                SystemProvider, 
                CommonService, 
                Mapper);

        }

        private bool _actualResult;

        public override Task When()
        {
            return Task.CompletedTask;
        }

        public async Task WhenAsync(ReviewChangeIndustryPlacementRequest request)
        {
            _actualResult = await AdminDashboardService.ProcessChangeIndustryPlacementAsync(request);
        }

        [Theory()]
        [MemberData(nameof(Data))]
        public async Task Then_Expected_Results_Are_Returned(ReviewChangeIndustryPlacementRequest request, bool expectedResponse, long uln)
        {
            await WhenAsync(request);

            if (expectedResponse == false)
            {
                _actualResult.Should().BeFalse();
                return;
            }

            var actualIndustryPlacement = DbContext.IndustryPlacement.FirstOrDefault(ip => ip.Id == request.RegistrationPathwayId);


            request.RegistrationPathwayId.Should().Be(actualIndustryPlacement.Id);
            if (request.ChangeIPDetails.IndustryPlacementStatusTo == IndustryPlacementStatus.Completed || request.ChangeIPDetails.IndustryPlacementStatusTo == IndustryPlacementStatus.NotCompleted)
            {
                actualIndustryPlacement.Details.Should().BeNull();
            }
            request.ChangeIPDetails.IndustryPlacementStatusTo.ToString().Should().Be(actualIndustryPlacement.Status.ToString());

            if (actualIndustryPlacement.Status == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                var actualDetails = JsonConvert.DeserializeObject<IndustryPlacementDetails>(actualIndustryPlacement.Details);
                request.ChangeIPDetails.HoursSpentOnPlacementTo.Should().Be(actualDetails.HoursSpentOnPlacement);
            }

        }

        public static IEnumerable<object[]> Data
        {
            get
            {
                return new[]
                {

                    new object[] { new ReviewChangeIndustryPlacementRequest()
                    {
                ChangeReason = "Test Reason",
                ContactName = "Test User",
                RegistrationPathwayId = 1,
                ChangeIPDetails = new ChangeIPDetails(){ IndustryPlacementStatusTo = IndustryPlacementStatus.NotCompleted },
                RequestDate = DateTime.Now.ToShortDateString(),
                ZendeskId = "1234567890",
                CreatedBy = "System"
                    },
                        true,1111111114,
                    },
                     new object[] { new ReviewChangeIndustryPlacementRequest()
                    {
                ChangeReason = "Test Reason1",
                ContactName = "Test User1",
                RegistrationPathwayId = 2,
                ChangeIPDetails = new ChangeIPDetails(){ IndustryPlacementStatusTo = IndustryPlacementStatus.Completed },
                RequestDate = DateTime.Now.ToShortDateString(),
                ZendeskId = "768568909",
                CreatedBy = "System"
                    },
                        true,1111111115,
                    },
                     new object[] { new ReviewChangeIndustryPlacementRequest    ()
                    {
                ChangeReason = "Test Reason1",
                ContactName = "Test User1",
                RegistrationPathwayId = 3,
                Details = JsonConvert.SerializeObject(new ChangeIPDetails() { IndustryPlacementStatusTo=IndustryPlacementStatus.CompletedWithSpecialConsideration }),
                ChangeIPDetails = new ChangeIPDetails(){ IndustryPlacementStatusTo = IndustryPlacementStatus.CompletedWithSpecialConsideration, HoursSpentOnPlacementTo=100, SpecialConsiderationReasonsTo= new List<int?>(){1,2} },
                RequestDate = DateTime.Now.ToShortDateString(),
                ZendeskId = "768568909",
                CreatedBy = "System"
                    },
                        true,1111111115,
                    }


                };
            }
        }

        private void SeedIndustyPlacementData(List<IndustryPlacementData> ipDatas)
        {
            foreach (var ipData in ipDatas)
            {
                var pathway = _profiles.FirstOrDefault(x => x.UniqueLearnerNumber == ipData.Uln).TqRegistrationPathways.FirstOrDefault();
                IndustryPlacementProvider.CreateIndustryPlacement(DbContext, pathway.Id, ipData.IndustryPlacementStatus, ipData.Details != null ? JsonConvert.SerializeObject(ipData.Details) : null);
            }
        }

        public class IndustryPlacementData
        {
            public long Uln { get; set; }
            public IndustryPlacementStatus IndustryPlacementStatus { get; set; }
            public IndustryPlacementDetails Details { get; set; }
        }
    }
}