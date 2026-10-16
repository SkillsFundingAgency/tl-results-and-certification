using AutoMapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.LearnerGender
{
    public abstract class TestSetup : BaseTest<Functions.LearnerGender>
    {
        protected IMapper Mapper;
        protected ILogger<ILrsLearnerService> Logger;
        protected FunctionContext FunctionContext;
        protected ICommonService CommonService;
        protected ILrsLearnerService LrsLearnerService;
        protected Functions.LearnerGender LearnerGenderFunction;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            FunctionContext = Substitute.For<FunctionContext>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILrsLearnerService>>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            LrsLearnerService = Substitute.For<ILrsLearnerService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Program).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);
            LearnerGenderFunction = new Functions.LearnerGender(CommonService, LrsLearnerService, Logger);
        }

        public override async Task When()
        {
            await LearnerGenderFunction.FetchLearnerGenderAsync(new TimerInfo(), FunctionContext);
        }
    }
}