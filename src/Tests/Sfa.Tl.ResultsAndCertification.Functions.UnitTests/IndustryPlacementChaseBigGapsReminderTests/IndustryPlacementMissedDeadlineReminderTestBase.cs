using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class IndustryPlacementChaseBigGapsReminderTestBase : BaseTest<IndustryPlacementChaseBigGapsReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementChaseBigGapsReminder IndustryPlacementChaseBigGapsReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();

            IndustryPlacementChaseBigGapsReminderFunction = new IndustryPlacementChaseBigGapsReminder(IndustryPlacementNotificationService, CommonService);
        }

    }
}
