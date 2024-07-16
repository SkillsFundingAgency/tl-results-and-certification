using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Mappers;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.WithdrawalServiceTests
{
    public abstract class TestBase : BaseTest<WithdrawalService>
    {
        protected IProviderRepository ProviderRepository;
        protected IRegistrationRepository RegistrationRepository;
        protected IRepository<TqRegistrationPathway> TqRegistrationPathwayRepository;
        protected ICommonService CommonService;

        protected WithdrawalService WithdrawalService;

        public override void Setup()
        {
            ProviderRepository = Substitute.For<IProviderRepository>();
            RegistrationRepository = Substitute.For<IRegistrationRepository>();
            TqRegistrationPathwayRepository = Substitute.For<IRepository<TqRegistrationPathway>>();
            CommonService = Substitute.For<ICommonService>();

            IMapper mapper = CreateMapper();
            ILogger<WithdrawalService> logger = Substitute.For<ILogger<WithdrawalService>>();

            WithdrawalService = new WithdrawalService(ProviderRepository, RegistrationRepository, TqRegistrationPathwayRepository, CommonService, mapper, logger);
        }

        private static AutoMapper.Mapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(RegistrationMapper).Assembly));
            return new AutoMapper.Mapper(mapperConfig);
        }
    }
}