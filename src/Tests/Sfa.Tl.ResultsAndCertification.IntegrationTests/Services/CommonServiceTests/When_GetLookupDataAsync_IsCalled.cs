using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CommonServiceTests
{
    public class When_GetLookupDataAsync_IsCalled : CommonServiceBaseTest
    {
        private IEnumerable<LookupData> _lookupResponse;

        public override void Given()
        {
            CreateMapper();
            SeedLookupData();

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

            CommonService = new CommonService(CommonServiceLogger, CommonMapper, TlLookupRepository, FunctionLogRepository, CommonRepository, NotificationService, Configuration);
        }

        public async override Task When()
        {
            _lookupResponse = await CommonService.GetLookupDataAsync(LookupCategory.PathwayComponentGrade);
        }

        [Fact]
        public void Expected_LookupData_Returned() 
        {
            _lookupResponse.Should().NotBeEmpty();
            _lookupResponse.Count().Should().Be(TlLookup.Count);
            var actualResponse = _lookupResponse.ToList();

            for (var i=0; i < _lookupResponse.Count(); i++)
            {
                actualResponse[i].Id.Should().Be(TlLookup[i].Id);
                actualResponse[i].Code.Should().Be(TlLookup[i].Code);
                actualResponse[i].Value.Should().Be(TlLookup[i].Value);
            }
        }
    }
}
