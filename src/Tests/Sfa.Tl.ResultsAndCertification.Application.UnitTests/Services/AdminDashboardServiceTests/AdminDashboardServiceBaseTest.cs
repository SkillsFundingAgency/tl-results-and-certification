using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Factory;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public abstract class AdminDashboardServiceBaseTest : BaseTest<AdminDashboardService>
    {
        protected IAdminDashboardRepository AdminDashboardRepository;
        protected IRepositoryFactory RepositoryFactory;
        protected ISystemProvider SystemProvider;
        protected ICommonService CommonService;
        protected IMapper Mapper;

        protected AdminDashboardService AdminDashboardService;

        public override void Setup()
        {
            var today = new DateTime(2023, 1, 1);

            AdminDashboardRepository = Substitute.For<IAdminDashboardRepository>();
            RepositoryFactory = Substitute.For<IRepositoryFactory>();

            SystemProvider = Substitute.For<ISystemProvider>();
            SystemProvider.UtcToday.Returns(today);

            Mapper = CreateMapper();

            AdminDashboardService = new AdminDashboardService(AdminDashboardRepository, RepositoryFactory, SystemProvider, Mapper);
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(LearnerMapper).Assembly));
            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}