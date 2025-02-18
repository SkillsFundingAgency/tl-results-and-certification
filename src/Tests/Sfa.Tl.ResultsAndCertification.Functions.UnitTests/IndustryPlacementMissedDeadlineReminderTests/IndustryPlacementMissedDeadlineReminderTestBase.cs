using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public abstract class IndustryPlacementMissedDeadlineReminderTestBase : BaseTest<IndustryPlacementMissedDeadlineReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementMissedDeadlineReminder IndustryPlacementMissedDeadlineReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementMissedDeadlineReminderFunction = new IndustryPlacementMissedDeadlineReminder(IndustryPlacementNotificationService, CommonService);
        }

    }
}
