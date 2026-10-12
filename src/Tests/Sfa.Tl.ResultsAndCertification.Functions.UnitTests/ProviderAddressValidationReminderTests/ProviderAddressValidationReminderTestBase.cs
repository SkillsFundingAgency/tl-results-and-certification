using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.ProviderAddressValidationReminderTests
{
    public abstract class ProviderAddressValidationReminderTestBase : BaseTest<ProviderAddressValidationReminder>
    {
        // Depedencies
        protected TimerSchedule TimerSchedule;

        protected IProviderAddressNotificationService ProviderAddressNotificationService;
        protected ResultsAndCertificationConfiguration Configuration;
        protected ICommonService CommonService;

        // Actual function instance
        protected ProviderAddressValidationReminder ProviderAddressValidationReminderFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            ProviderAddressNotificationService = Substitute.For<IProviderAddressNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                ProviderAddressValidationReminderSettings= new ProviderAddressValidationReminderSettings
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

            ProviderAddressValidationReminderFunction = new ProviderAddressValidationReminder(ProviderAddressNotificationService, CommonService, Configuration);
        }
    }
}