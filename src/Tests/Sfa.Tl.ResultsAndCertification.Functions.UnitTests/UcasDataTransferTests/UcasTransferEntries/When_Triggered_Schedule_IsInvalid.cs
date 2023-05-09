using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferEntries
{
    public class When_Triggered_Schedule_IsInvalid : TestSetup
    {
        public override void Given()
        {
            var todayDate = "12/05/2023".ParseStringToDateTimeWithFormat();
            CommonService.IsUcasTransferEntriesTriggerDateValid().Returns(false);
            CommonService.CurrentDate.Returns(todayDate);
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
