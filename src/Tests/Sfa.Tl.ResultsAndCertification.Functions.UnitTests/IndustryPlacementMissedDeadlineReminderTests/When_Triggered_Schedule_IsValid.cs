using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public class When_Triggered_Schedule_IsValid : TestSetup
    {
        public override void Given()
        {
            var todayDate = "19/08/2022".ParseStringToDateTimeWithFormat();
            CommonService.CurrentDate.Returns(todayDate);

            CommonService.CreateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
            CommonService.IsIndustryPlacementTriggerDateValid().Returns(true);
            IndustryPlacementNotificationService.ProcessIndustryPlacementMissedDeadlineReminderAsync().Returns(new IndustryPlacementNotificationResponse { IsSuccess = true });
            CommonService.UpdateFunctionLog(Arg.Any<FunctionLogDetails>()).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            CommonService.Received(1).CreateFunctionLog(Arg.Any<FunctionLogDetails>());
            IndustryPlacementNotificationService.Received(1).ProcessIndustryPlacementMissedDeadlineReminderAsync();
            CommonService.Received(1).UpdateFunctionLog(Arg.Any<FunctionLogDetails>());
        }
    }
}
