using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferAmendments
{
    public class When_Triggered_Schedule_IsInvalid : TestSetup
    {
        public override void Setup()
        {
            var validDateRange = new DateTimeRange
            {
                From = Today.AddDays(1),
                To = Today.AddDays(10)
            };

            Setup(validDateRange);
        }

        public override void Given()
        {
            CommonService.CurrentDate.Returns(Today);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.DidNotReceive().CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            UcasDataTransferService.DidNotReceive().ProcessUcasDataRecordsAsync(UcasDataType.Amendments);
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
