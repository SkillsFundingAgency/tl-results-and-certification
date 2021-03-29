using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LearnerService
{
    public abstract class TestSetup : BaseTest<Functions.Services.LearnerService>
    {
        protected IMapper Mapper;
        protected ILogger<ILearnerService> Logger;
        protected ILearnerRecordService LearnerRecordService;
        protected ILearnerServiceApiClient LearnerServiceApiClient;
        protected Functions.Services.LearnerService Service;
        protected LearnerGenderResponse ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ILearnerService>>();
            LearnerRecordService = Substitute.For<ILearnerRecordService>();
            LearnerServiceApiClient = Substitute.For<ILearnerServiceApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.LearnerService(Mapper, Logger, LearnerServiceApiClient, LearnerRecordService);
        }

        public async override Task When()
        {
            ActualResult = await Service.FetchLearnerGenderAsync();
        }
    }
}
