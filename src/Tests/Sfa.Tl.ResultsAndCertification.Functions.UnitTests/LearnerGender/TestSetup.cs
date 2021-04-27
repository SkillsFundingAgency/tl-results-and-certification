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

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.LearnerGender
{
    public abstract class TestSetup : BaseTest<Functions.LearnerGender>
    {
        protected IMapper Mapper;
        protected ILogger<ILearnerService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ILearnerService LearnerService;
        protected Functions.LearnerGender LearnerGenderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILearnerService>>();
            LearnerService = Substitute.For<ILearnerService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            LearnerGenderFunction = new Functions.LearnerGender(CommonService, LearnerService);
        }

        public async override Task When()
        {
            await LearnerGenderFunction.FetchLearnerGenderAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.LearnerGender>());
        }
    }
}
