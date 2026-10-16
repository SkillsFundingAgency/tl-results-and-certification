using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystOverallResultExtractionTests
{
    public abstract class TestSetup : BaseTest<AnalystOverallResultExtraction>
    {
        protected AnalystOverallResultExtraction AnalystDataExtraction;
        protected IAnalystOverallResultExtractionService AnalystResultExtractionService = Substitute.For<IAnalystOverallResultExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();
        protected ILogger<IAnalystOverallResultExtractionService> Logger = Substitute.For<ILogger<IAnalystOverallResultExtractionService>>();
        protected FunctionContext FunctionContext = Substitute.For<FunctionContext>();

        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                AnalystOverallResultExtractSettings = new AnalystOverallResultExtractSettings
                {
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

            AnalystDataExtraction = new AnalystOverallResultExtraction(config, AnalystResultExtractionService, CommonService, Logger);
        }

        public override Task When()
        {
            return AnalystDataExtraction.AnalystOverallResultExtractAsync(new TimerInfo(), FunctionContext);
        }
    }
}