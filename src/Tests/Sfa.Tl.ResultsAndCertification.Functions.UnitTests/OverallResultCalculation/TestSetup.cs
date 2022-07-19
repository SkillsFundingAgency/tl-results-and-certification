using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.OverallResultCalculation
{
    public abstract class TestSetup : BaseTest<Functions.OverallResultCalculation>
    {
        protected ILogger<ILrsLearnerService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration ResultsAndCertificationConfiguration;
        protected IOverallResultCalculationFunctionService OverallResultCalculationFunctionService;
        protected Functions.OverallResultCalculation OverallResultCalculationFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ILrsLearnerService>>();
            OverallResultCalculationFunctionService = Substitute.For<IOverallResultCalculationFunctionService>();
            ResultsAndCertificationConfiguration = new ResultsAndCertificationConfiguration
            {
                OverallResultsCalculationDate = DateTime.UtcNow
            };
            OverallResultCalculationFunction = new Functions.OverallResultCalculation(ResultsAndCertificationConfiguration, OverallResultCalculationFunctionService, CommonService);
        }

        public async override Task When()
        {
            await OverallResultCalculationFunction.OverallResultCalculationAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.OverallResultCalculation>());
        }
    }
}
