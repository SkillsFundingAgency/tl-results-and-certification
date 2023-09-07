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

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.SpecialismRommExtractTests
{
    public abstract class TestSetup : BaseTest<SpecialismRommExtract>
    {
        private readonly TimerSchedule _timerSchedule = Substitute.For<TimerSchedule>();
        protected SpecialismRommExtract SpecialismRommExtract;

        protected ISpecialismRommExtractionService SpecialismRommExtractService = Substitute.For<ISpecialismRommExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        protected int[] SpecialismAssessmentSeriesYearsToProcess = new[] { 2023 };

        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                SpecialismRommExtractSettings = new SpecialismRommExtractSettings
                {
                    SpecialismAssessmentSeriesYearsToProcess = SpecialismAssessmentSeriesYearsToProcess,
                    SpecialismRommValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(-1),
                            To = today.AddDays(1)
                        }
                    }
                }
            };

            SpecialismRommExtract = new SpecialismRommExtract(config, SpecialismRommExtractService, CommonService);
        }

        public override Task When()
        {
            return SpecialismRommExtract.SpecialismRommExtractAsync(new TimerInfo(_timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<AnalystOverallResultExtraction>());
        }
    }
}