using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Data.Repositories;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
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
        protected IRepository<TlLookup> TlLookupRepository;
        protected IRepository<FunctionLog> FunctionLogRepository;
        protected ICommonRepository CommonRepository;

        protected IMapper CommonMapper;
        protected ILogger<GenericRepository<TlLookup>> TlLookupRepositoryLogger;
        protected ILogger<GenericRepository<FunctionLog>> FunctionLogRepositoryLogger;

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
    }
}
