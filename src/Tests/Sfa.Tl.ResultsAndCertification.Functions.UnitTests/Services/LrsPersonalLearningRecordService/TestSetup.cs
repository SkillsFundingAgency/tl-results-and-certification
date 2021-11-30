using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.LrsPersonalLearningRecordService
{
    public abstract class TestSetup : BaseTest<Functions.Services.LrsPersonalLearningRecordService>
    {
        protected IMapper Mapper;
        protected ILogger<ILrsPersonalLearningRecordService> Logger;
        protected ILrsService LrsService;
        protected ILrsPersonalLearningRecordServiceApiClient LrsPersonalLearningRecordApiClient;
        protected Functions.Services.LrsPersonalLearningRecordService Service;
        protected LrsLearnerVerificationAndLearningEventsResponse ActualResult;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ILrsPersonalLearningRecordService>>();
            LrsService = Substitute.For<ILrsService>();
            LrsPersonalLearningRecordApiClient = Substitute.For<ILrsPersonalLearningRecordServiceApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.LrsPersonalLearningRecordService(Mapper, Logger, LrsPersonalLearningRecordApiClient, LrsService);
        }

        public async override Task When()
        {
            ActualResult = await Service.ProcessLearnerVerificationAndLearningEventsAsync();
        }
    }
}
