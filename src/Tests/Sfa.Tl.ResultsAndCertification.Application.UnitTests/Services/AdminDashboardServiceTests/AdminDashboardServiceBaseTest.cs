using AutoMapper;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using IndustryPlacement = Sfa.Tl.ResultsAndCertification.Domain.Models.IndustryPlacement;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests
{
    public abstract class AdminDashboardServiceBaseTest : BaseTest<AdminDashboardService>
    {
        protected IAdminDashboardRepository AdminDashboardRepository;
        protected IRepository<TqRegistrationPathway> RegistrationPathwayRepo;
        protected IRepository<IndustryPlacement> IndustryPlacementRepo;
        protected ISystemProvider SystemProvider;
        protected ICommonService CommonService;
        protected IMapper Mapper;

        protected AdminDashboardService AdminDashboardService;

        public override void Setup()
        {
            var today = new DateTime(2023, 1, 1);

            AdminDashboardRepository = Substitute.For<IAdminDashboardRepository>();
            RegistrationPathwayRepo = Substitute.For<IRepository<TqRegistrationPathway>>();
            IndustryPlacementRepo = Substitute.For<IRepository<IndustryPlacement>>();

            SystemProvider = Substitute.For<ISystemProvider>();
            SystemProvider.UtcToday.Returns(today);

            CommonService = Substitute.For<ICommonService>();
            Mapper = CreateMapper();

            AdminDashboardService = new AdminDashboardService(AdminDashboardRepository, RegistrationPathwayRepo, IndustryPlacementRepo, SystemProvider, CommonService, Mapper);
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(LearnerMapper).Assembly));
            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}