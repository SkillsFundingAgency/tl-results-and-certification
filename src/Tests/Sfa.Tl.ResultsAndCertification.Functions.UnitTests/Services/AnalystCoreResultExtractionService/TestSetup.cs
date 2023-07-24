using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.AnalystCoreResultExtractionService;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystCoreResultExtractionService
{
    public abstract class TestSetup : AnalystCoreResultExtractFunctionTestBase
    {
        protected int[] AcademicYearsToProcess = new int[] { 2022 };

        public override void Setup()
        {
            var config = new ResultsAndCertificationConfiguration
            {
                AnalystCoreResultExtractSettings = new  AnalystCoreResultExtractSettings
                {
                    AcademicYearsToProcess = AcademicYearsToProcess
                }
            };

            TimerSchedule = Substitute.For<TimerSchedule>();
            AnalystCoreResultExtractionService = Substitute.For<IAnalystCoreResultExtractionService>();
            CommonService = Substitute.For<ICommonService>();

            AnalystCoreResultExtractionFunction = new AnalystCoreResultExtraction(config, AnalystCoreResultExtractionService, CommonService);
        }

        public async override Task When()
        {
            await AnalystCoreResultExtractionFunction.AnalystCoreResultExtractionServiceAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystCoreResultExtraction>());
        }
    }
}
