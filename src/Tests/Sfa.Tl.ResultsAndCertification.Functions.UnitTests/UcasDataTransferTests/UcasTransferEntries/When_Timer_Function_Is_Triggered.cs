using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferEntries
{
    public class When_Timer_Function_Is_Triggered : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            UcasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Entries).Returns(new UcasDataTransferResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }
                
        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            if (DateTime.UtcNow.IsLastWeekdayOfMonth(DayOfWeek.Wednesday))
            {
                CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
                UcasDataTransferService.Received(1).ProcessUcasDataRecordsAsync(UcasDataType.Entries);
                CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            }
        }
    }
}
