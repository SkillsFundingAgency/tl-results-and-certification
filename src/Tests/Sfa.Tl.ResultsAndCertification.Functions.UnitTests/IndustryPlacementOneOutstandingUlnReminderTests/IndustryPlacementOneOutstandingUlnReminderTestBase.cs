using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Services;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementOneOutstandingUlnReminderTests
{
    public abstract class IndustryPlacementOneOutstandingUlnReminderTestBase : BaseTest<IndustryPlacementOneOutstandingUlnReminder>
    {
        // Depedencies
        protected ILogger<IndustryPlacementNotificationService> Logger;

        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration Configuration;

        // Actual function instance
        protected IndustryPlacementOneOutstandingUlnReminder IndustryPlacementOneOutstandingUlnReminderFunction;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<IndustryPlacementNotificationService>>();
            
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                IPOneOutstandingUlnReminderSettings = new IPOneOutstandingUlnReminderSettings
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



            IndustryPlacementOneOutstandingUlnReminderFunction = new IndustryPlacementOneOutstandingUlnReminder(IndustryPlacementNotificationService, CommonService, Configuration);
        }

    }
}
