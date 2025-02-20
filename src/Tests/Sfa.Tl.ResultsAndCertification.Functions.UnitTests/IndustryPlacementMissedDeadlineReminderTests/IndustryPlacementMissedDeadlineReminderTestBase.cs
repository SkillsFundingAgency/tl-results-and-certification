using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementMissedDeadlineReminderTests
{
    public abstract class IndustryPlacementMissedDeadlineReminderTestBase : BaseTest<IndustryPlacementMissedDeadlineReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ResultsAndCertificationConfiguration Configuration;
        protected ICommonService CommonService;

        // Actual function instance
        protected IndustryPlacementMissedDeadlineReminder IndustryPlacementMissedDeadlineReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                IPMissedDeadlineReminderSettings = new IPMissedDeadlineReminderSettings
                {
                    ValidDateRanges = new[]
                    {
                        new DateTimeRange
                        {
                            From = today.AddDays(-1),
                            To = today.AddDays(1)
                        }
                    }
                }
            };

            IndustryPlacementMissedDeadlineReminderFunction = new IndustryPlacementMissedDeadlineReminder(IndustryPlacementNotificationService, CommonService, Configuration);
        }

    }
}
