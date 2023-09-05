using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.SpecialismRommExtractTests
{
    public class When_Today_Is_InvalidDate : TestSetup
    {
        public override void Setup() 
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                SpecialismRommExtractSetting = new SpecialismRommExtractSetting
                {
                    AssesmentSeriesYearsToProcess = AssessmentSeriesYearsToProcess,
                    SpecialismRommValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(1),
                            To = today.AddDays(10)
                        }
                    }
                }
            };

            SpecialismRommExtract = new SpecialismRommExtract(config, SpecialismRommExtractService, CommonService);
        }

        public override void Given()
        {
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(0).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            SpecialismRommExtractService.Received(0).ProcessSpecialismRommExtractsAsync(AssessmentSeriesYearsToProcess);
            CommonService.Received(0).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}