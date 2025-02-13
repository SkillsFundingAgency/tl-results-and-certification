using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementOneOutstandingUlnReminderTests
{
    public abstract class IndustryPlacementOneOutstandingUlnReminderTestBase : BaseTest<IndustryPlacementOneOutstandingUlnReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementOneOutstandingUlnReminder IndustryPlacementOneOutstandingUlnReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementOneOutstandingUlnReminderFunction = new IndustryPlacementOneOutstandingUlnReminder(IndustryPlacementNotificationService, CommonService);
        }

    }
}
