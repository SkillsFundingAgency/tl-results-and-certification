using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressValidationReminderTests
{
    public class When_Response_Is_Failed : TestSetup
    {
        public override void Given()
        {
            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            ProviderAddressNotificationService.ProcessProviderAddressValidationReminderAsync(Arg.Any<int>()).Returns(new ProviderAddressNotificationResponse { IsSuccess = false });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            ProviderAddressNotificationService.Received(1).ProcessProviderAddressValidationReminderAsync(Arg.Any<int>());
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
            CommonService.Received(1).SendFunctionJobFailedNotification(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}