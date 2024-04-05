using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminChangeLogServiceTests
{
    public abstract class AdminChangeLogServiceBaseTest : BaseTest<AdminChangeLogService>
    {
        protected IAdminChangeLogRepository AdminChangeLogRepository;
        protected AdminChangeLogService AdminChangeLogService;
        protected IMapper Mapper;

        public override void Setup()
        {
            Mapper = CreateMapper();

            AdminChangeLogRepository = Substitute.For<IAdminChangeLogRepository>();
            AdminChangeLogService = new AdminChangeLogService(AdminChangeLogRepository, Mapper);
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(ChangeLogMapper).Assembly));
            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}