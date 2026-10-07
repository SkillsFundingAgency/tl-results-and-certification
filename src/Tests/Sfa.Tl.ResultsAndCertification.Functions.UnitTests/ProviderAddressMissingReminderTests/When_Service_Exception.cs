using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressMissingReminderTests
{
    public class When_Service_Exception : TestSetup
    {
        public override void Given()
        {
            var todayDate = "19/08/2022".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);

            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            ProviderAddressNotificationService.ProcessProviderAddressMissingReminderAsync(Arg.Any<int>()).Returns(x => Task.FromException(new Exception()));
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(2).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            ProviderAddressNotificationService.Received(1).ProcessProviderAddressMissingReminderAsync(Arg.Any<int>());
            CommonService.DidNotReceive().UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}