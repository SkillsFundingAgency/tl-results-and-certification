using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressMissingReminderTests
{
    public abstract class ProviderAddressMissingReminderTestBase : BaseTest<ProviderAddressMissingReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;

        protected IProviderAddressNotificationService ProviderAddressNotificationService;
        protected ResultsAndCertificationConfiguration Configuration;
        protected ICommonService CommonService;

        // Actual function instance
        protected ProviderAddressMissingReminder ProviderAddressMissingReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            ProviderAddressNotificationService = Substitute.For<IProviderAddressNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                ProviderAddressMissingReminderSettings= new ProviderAddressMissingReminderSettings
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

            ProviderAddressMissingReminderFunction = new ProviderAddressMissingReminder(ProviderAddressNotificationService, CommonService, Configuration);
        }
    }
}