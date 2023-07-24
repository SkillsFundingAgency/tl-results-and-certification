using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystDataExtractionTests
{
    public abstract class TestSetup : BaseTest<AnalystDataExtraction>
    {
        private readonly TimerSchedule _timerSchedule = Substitute.For<TimerSchedule>();
        private AnalystDataExtraction _analystDataExtraction;

        protected IAnalystResultExtractionService AnalystResultExtractionService = Substitute.For<IAnalystResultExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        protected int[] AcademicYearsToProcess = new[] { 2020, 2021 };
        
        public override void Setup()
        {
            var config = new ResultsAndCertificationConfiguration
            {
                AnalystOverallResultExtractSettings = new AnalystOverallResultExtractSettings
                {
                    AcademicYearsToProcess = AcademicYearsToProcess
                }
            };

            _analystDataExtraction = new AnalystDataExtraction(config, AnalystResultExtractionService, CommonService);
        }

        public override Task When()
        {
            return _analystDataExtraction.AnalystOverallResultExtractAsync(new TimerInfo(_timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystDataExtraction>());
        }
    }
}