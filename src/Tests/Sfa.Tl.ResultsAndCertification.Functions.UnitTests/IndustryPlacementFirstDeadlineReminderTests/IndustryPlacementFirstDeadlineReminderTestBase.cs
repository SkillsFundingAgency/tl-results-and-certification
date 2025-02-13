using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementFirstDeadlineReminderTests
{
    public abstract class IndustryPlacementFirstDeadlineReminderTestBase : BaseTest<IndustryPlacementFirstDeadlineReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementFirstDeadlineReminder IndustryPlacementFirstDeadlineReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementFirstDeadlineReminderFunction = new IndustryPlacementFirstDeadlineReminder(IndustryPlacementNotificationService, CommonService);
        }

    }
}
