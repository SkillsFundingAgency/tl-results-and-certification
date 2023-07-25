using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystDataExtractionTests
{
    public abstract class TestSetup : BaseTest<AnalystDataExtraction>
    {
        private readonly TimerSchedule _timerSchedule = Substitute.For<TimerSchedule>();
        protected AnalystDataExtraction AnalystDataExtraction;

        protected IAnalystResultExtractionService AnalystResultExtractionService = Substitute.For<IAnalystResultExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        protected int[] AcademicYearsToProcess = new[] { 2020, 2021 };

        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                AnalystOverallResultExtractSettings = new AnalystOverallResultExtractSettings
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

            AnalystDataExtraction = new AnalystDataExtraction(config, AnalystResultExtractionService, CommonService);
        }

        public override Task When()
        {
            return AnalystDataExtraction.AnalystOverallResultExtractAsync(new TimerInfo(_timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystDataExtraction>());
        }
    }
}