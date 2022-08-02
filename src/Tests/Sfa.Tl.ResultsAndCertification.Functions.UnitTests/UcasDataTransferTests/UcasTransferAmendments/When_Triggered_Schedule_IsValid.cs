using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferAmendments
{
    public class When_Triggered_Schedule_IsValid : TestSetup
    {
        public override void Given()
        {
            var todayDate = "19/08/2022".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);

            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            UcasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Amendments).Returns(new UcasDataTransferResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            UcasDataTransferService.Received(1).ProcessUcasDataRecordsAsync(UcasDataType.Amendments);
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
