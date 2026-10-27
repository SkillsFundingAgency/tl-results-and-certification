using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Services;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressExtractTests
{
    public class When_Today_Is_InvalidDate : TestSetup
    {
        public override void Setup()
        {
            DateTime today = DateTime.UtcNow.Date;

            var config = new ResultsAndCertificationConfiguration
            {
                ProviderAddressExtractSettings = new ProviderAddressExtractSettings
                {
                    ProviderAddressAcademicYearsToProcess = AcademicYearsToProcess,
                    ProviderAddressValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(1),
                            To = today.AddDays(10)
                        }
                    }
                }
            };

            ProviderAddressExtraction = new ProviderAddressExtraction(config, ProviderAddressExtractionService, CommonService);
        }

        public override void Given()
        {
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(0).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            ProviderAddressExtractionService.Received(0).ProcessProviderAddressExtractionAsync(AcademicYearsToProcess);
            CommonService.Received(0).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}