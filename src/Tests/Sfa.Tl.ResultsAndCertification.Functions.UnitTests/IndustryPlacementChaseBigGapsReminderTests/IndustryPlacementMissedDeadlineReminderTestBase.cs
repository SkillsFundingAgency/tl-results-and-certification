using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class IndustryPlacementChaseBigGapsReminderTestBase : BaseTest<IndustryPlacementChaseBigGapsReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration Configuration;

        // Actual function instance
        protected IndustryPlacementChaseBigGapsReminder IndustryPlacementChaseBigGapsReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                IPChaseBigGapsReminderExtractSettings = new IPChaseBigGapsReminderExtractSettings
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

            IndustryPlacementChaseBigGapsReminderFunction = new IndustryPlacementChaseBigGapsReminder(IndustryPlacementNotificationService, CommonService, Configuration);
        }

    }
}
