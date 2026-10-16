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
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ILrsPersonalLearningRecordService>>();
            LrsService = Substitute.For<ILrsService>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            LrsPersonalLearningRecordApiClient = Substitute.For<ILrsPersonalLearningRecordServiceApiClient>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Program).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.LrsPersonalLearningRecordService(Mapper, Logger, LrsPersonalLearningRecordApiClient, LrsService);
        }

        public override async Task When()
        {
            ActualResult = await Service.ProcessLearnerVerificationAndLearningEventsAsync();
        }
    }
}