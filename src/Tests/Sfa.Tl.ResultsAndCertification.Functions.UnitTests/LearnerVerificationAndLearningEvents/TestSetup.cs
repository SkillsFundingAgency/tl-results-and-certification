using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.LearnerVerificationAndLearningEvents
{
    public abstract class TestSetup : BaseTest<Functions.LearnerVerificationAndLearningEvents>
    {
        protected IMapper Mapper;
        protected ILogger<ILrsPersonalLearningRecordService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ILrsPersonalLearningRecordService LrsPersonalLearningRecordService;
        protected Functions.LearnerVerificationAndLearningEvents LearningEventsFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILrsPersonalLearningRecordService>>();
            LrsPersonalLearningRecordService = Substitute.For<ILrsPersonalLearningRecordService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            LearningEventsFunction = new Functions.LearnerVerificationAndLearningEvents(CommonService, LrsPersonalLearningRecordService);
        }

        public async override Task When()
        {
            await LearningEventsFunction.VerifyLearnerAndFetchLearningEventsAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.LearnerVerificationAndLearningEvents>());
        }
    }
}
