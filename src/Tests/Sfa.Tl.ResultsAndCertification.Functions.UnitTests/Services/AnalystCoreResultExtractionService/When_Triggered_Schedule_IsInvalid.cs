using AutoMapper.Configuration.Annotations;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.AnalystCoreResultExtractionService
{
    public class When_Triggered_Schedule_IsInvalid : TestSetup
    {
        public override void Given()
        {
            var todayDate = "15/05/2022".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.DidNotReceive().CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            AnalystCoreResultExtractionService.DidNotReceive().ProcessAnalystCoreResultExtractsAsync();
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
