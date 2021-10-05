using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataProvider;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.Services.CommonServiceTests
{
    public abstract class CommonServiceBaseTest : BaseTest<TlLookup>
    {
        protected CommonService CommonService;
        protected ILogger<CommonService> CommonServiceLogger;
        protected IList<TlLookup> TlLookup;
        protected IList<AcademicYear> AcademicYears;
        protected IRepository<TlLookup> TlLookupRepository;
        protected IRepository<FunctionLog> FunctionLogRepository;
        protected ICommonRepository CommonRepository;
        protected ResultsAndCertificationConfiguration Configuration;

        protected IMapper CommonMapper;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected ILogger<GenericRepository<FunctionLog>> FunctionLogRepositoryLogger;

        protected IAsyncNotificationClient NotificationsClient;
        protected ILogger<NotificationService> NotificationLogger;
        protected IRepository<NotificationTemplate> NotificationTemplateRepository;
        protected ILogger<GenericRepository<NotificationTemplate>> NotificationTemplateRepositoryLogger;
        protected ILogger<INotificationService> NotificationServiceLogger;
        protected INotificationService NotificationService;

        protected virtual void CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(CommonMapper).Assembly));
            CommonMapper = new Mapper(mapperConfig);
        }

        public void SeedLookupData()
        {
            TlLookup = TlLookupDataProvider.CreateTlLookupList(DbContext, null, true);
            DbContext.SaveChangesAsync();
        }

        public FunctionLog  SeedFunctionLog()
        {
            var functionLog = new FunctionLogBuilder().Build();
            var functionLogEntity = FunctionLogDataProvider.CreateFunctionLog(DbContext, functionLog);

            DbContext.SaveChanges();
            return functionLogEntity;
        }

        public void SeedAcademicYears()
        {
            AcademicYears = AcademicYearDataProvider.CreateAcademicYearList(DbContext, null);
            DbContext.SaveChanges();
        }
    }
}
