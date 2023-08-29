using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CoreRommExtractTests
{
    public abstract class TestSetup : BaseTest<CoreRommExtract>
    {
        private readonly TimerSchedule _timerSchedule = Substitute.For<TimerSchedule>();
        protected CoreRommExtract CoreRommExtract;

        protected ICoreRommExtractService CoreRommExtractService = Substitute.For<ICoreRommExtractService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        protected int[] AssesmentSeriesYearsToProcess = new[] { 2023 };

        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                CoreRommExtractSettings = new CoreRommExtractSettings
                {
                    AssesmentSeriesYearsToProcess = AssesmentSeriesYearsToProcess,
                    CoreRommValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(-1),
                            To = today.AddDays(1)
                        }
                    }
                }
            };

            CoreRommExtract = new CoreRommExtract(config, CoreRommExtractService, CommonService);
        }

        public override Task When()
        {
            return CoreRommExtract.CoreRommExtractAsync(new TimerInfo(_timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystOverallResultExtraction>());
        }
    }
}