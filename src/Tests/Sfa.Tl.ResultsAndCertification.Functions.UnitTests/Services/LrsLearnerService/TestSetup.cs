using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LrsLearnerService
{
    public abstract class TestSetup : BaseTest<Functions.Services.LrsLearnerService>
    {
        protected IMapper Mapper;
        protected ILogger<ILrsLearnerService> Logger;
        protected ILrsService LrsService;
        protected ILrsLearnerServiceApiClient LrsLearnerServiceApiClient;
        protected Functions.Services.LrsLearnerService Service;
        protected LrsLearnerGenderResponse ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ILrsLearnerService>>();
            LrsService = Substitute.For<ILrsService>();
            LrsLearnerServiceApiClient = Substitute.For<ILrsLearnerServiceApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.LrsLearnerService(Mapper, Logger, LrsLearnerServiceApiClient, LrsService);
        }

        public async override Task When()
        {
            ActualResult = await Service.FetchLearnerGenderAsync();
        }
    }
}
