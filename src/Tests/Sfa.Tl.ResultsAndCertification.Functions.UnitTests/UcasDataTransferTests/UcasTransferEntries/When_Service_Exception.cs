using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests.UcasTransferEntries
{
    public class When_Service_Exception : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            UcasDataTransferService.ProcessUcasDataRecordsAsync(UcasDataType.Entries).Returns(x => Task.FromException(new Exception()));
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            if (DateTime.UtcNow.IsLastWeekdayOfMonth(DayOfWeek.Wednesday))
            {
                CommonService.Received(2).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
                UcasDataTransferService.Received(1).ProcessUcasDataRecordsAsync(UcasDataType.Entries);
                CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
                CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
            }
        }
    }
}