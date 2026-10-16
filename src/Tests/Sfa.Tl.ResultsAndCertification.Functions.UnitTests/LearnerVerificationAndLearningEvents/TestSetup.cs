using AutoMapper;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.LearnerVerificationAndLearningEvents
{
    public abstract class TestSetup : BaseTest<Functions.LearnerVerificationAndLearningEvents>
    {
        protected IMapper Mapper;
        protected ILogger<ILrsPersonalLearningRecordService> Logger;
        protected FunctionContext FunctionContext;
        protected ICommonService CommonService;
        protected ILrsPersonalLearningRecordService LrsPersonalLearningRecordService;
        protected Functions.LearnerVerificationAndLearningEvents LearningEventsFunction;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            FunctionContext = Substitute.For<FunctionContext>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILrsPersonalLearningRecordService>>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            LrsPersonalLearningRecordService = Substitute.For<ILrsPersonalLearningRecordService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Program).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
            LearningEventsFunction = new Functions.LearnerVerificationAndLearningEvents(CommonService, LrsPersonalLearningRecordService, Logger);
        }

        public override async Task When()
        {
            await LearningEventsFunction.VerifyLearnerAndFetchLearningEventsAsync(new TimerInfo(), FunctionContext);
        }
    }
}