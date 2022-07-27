using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferResults
{
    public class When_Timer_Function_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            UcasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Results).Returns(new UcasDataTransferResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact(Skip ="TODO: Test")]
        public void Then_Expected_Methods_Are_Called()
        {
            if (DateTime.UtcNow.IsNthWeekdayOfMonth(DayOfWeek.Thursday, Months.August, 2))
            {
                CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
                UcasDataTransferService.Received(1).ProcessUcasDataRecordsAsync(UcasDataType.Results);
                CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            }
        }
    }
}