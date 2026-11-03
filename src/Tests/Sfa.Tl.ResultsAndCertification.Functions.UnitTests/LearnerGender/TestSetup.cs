using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Azure.Functions.Worker;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
        protected ILogger<ILrsLearnerService> Logger;
        protected ICommonService CommonService;
        protected ILrsLearnerService LrsLearnerService;
        protected Functions.LearnerGender LearnerGenderFunction;

        public override void Setup()
        {
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILrsLearnerService>>();
            LrsLearnerService = Substitute.For<ILrsLearnerService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly), NullLoggerFactory.Instance);
            Mapper = new AutoMapper.Mapper(mapperConfig);
            LearnerGenderFunction = new Functions.LearnerGender(CommonService, LrsLearnerService);
        }

        public async override Task When()
        {
            await LearnerGenderFunction.FetchLearnerGenderAsync(new TimerInfo { ScheduleStatus = new ScheduleStatus() }, FunctionContextBuilder.Create());
        }
    }
}
