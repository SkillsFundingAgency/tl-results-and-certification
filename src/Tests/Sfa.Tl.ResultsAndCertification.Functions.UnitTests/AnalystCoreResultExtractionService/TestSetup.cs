using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystCoreResultExtractionService
{
    public abstract class TestSetup : AnalystCoreResultExtractFunctionTestBase
    {
        protected int[] AcademicYearsToProcess = new int[] { 2022 };

        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                AnalystCoreResultExtractSettings = new AnalystCoreResultExtractSettings
                {
                    CoreAcademicYearsToProcess = AcademicYearsToProcess,
                    CoreValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(-1),
                            To = today.AddDays(1)
                        }
                    }
                }
            };

            AnalystCoreResultExtractionFunction = new AnalystCoreResultExtraction(config, AnalystCoreResultExtractionService, CommonService);
        }

        public async override Task When()
        {
            await AnalystCoreResultExtractionFunction.AnalystCoreResultExtractAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystCoreResultExtraction>());
        }
    }
}
