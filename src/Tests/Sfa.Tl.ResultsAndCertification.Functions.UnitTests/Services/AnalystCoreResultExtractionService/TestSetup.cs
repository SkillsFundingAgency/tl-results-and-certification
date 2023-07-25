using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.AnalystCoreResultExtractionService;
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
                    AcademicYearsToProcess = AcademicYearsToProcess,
                    ValidDateRanges = new[]
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
            await AnalystCoreResultExtractionFunction.AnalystCoreResultExtractionServiceAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystCoreResultExtraction>());
        }
    }
}
