using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CoreRommExtractTests
{
    public class When_Today_Is_InvalidDate : TestSetup
    {
        public override void Setup() 
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                CoreRommExtractSettings = new CoreRommExtractSettings
                {
                    AssessmentSeriesYearsToProcess = AssessmentSeriesYearsToProcess,
                    CoreRommValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(1),
                            To = today.AddDays(10)
                        }
                    }
                }
            };

            CoreRommExtract = new CoreRommExtract(config, CoreRommExtractService, CommonService);
        }

        public override void Given()
        {
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(0).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            CoreRommExtractService.Received(0).ProcessCoreRommExtractAsync(AssessmentSeriesYearsToProcess);
            CommonService.Received(0).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}