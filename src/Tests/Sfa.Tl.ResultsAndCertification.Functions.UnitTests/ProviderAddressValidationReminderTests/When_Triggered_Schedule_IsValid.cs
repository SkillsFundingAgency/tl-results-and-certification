using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressValidationReminderTests
{
    public class When_Triggered_Schedule_IsValid : TestSetup
    {
        public override void Given()
        {
            var todayDate = "19/08/2022".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);

            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            ProviderAddressNotificationService.ProcessProviderAddressValidationReminderAsync(Arg.Any<int>()).Returns(new ProviderAddressNotificationResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            ProviderAddressNotificationService.Received(1).ProcessProviderAddressValidationReminderAsync(Arg.Any<int>());
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}