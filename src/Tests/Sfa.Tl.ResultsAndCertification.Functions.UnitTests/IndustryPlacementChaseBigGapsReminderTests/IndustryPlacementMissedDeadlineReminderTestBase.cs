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


namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.IndustryPlacementChaseBigGapsReminderTests
{
    public abstract class IndustryPlacementChaseBigGapsReminderTestBase : BaseTest<IndustryPlacementChaseBigGapsReminder>
    {
        // Depedencies
        protected IIndustryPlacementNotificationService IndustryPlacementNotificationService;
        protected ICommonService CommonService;
        protected ResultsAndCertificationConfiguration Configuration;
        protected ILogger<IndustryPlacementNotificationService> Logger;
        protected FunctionContext FunctionContext;


        // Actual function instance
        protected IndustryPlacementChaseBigGapsReminder IndustryPlacementChaseBigGapsReminderFunction;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<IndustryPlacementNotificationService>>();
            FunctionContext = Substitute.For<FunctionContext>();
            IndustryPlacementNotificationService = Substitute.For<IIndustryPlacementNotificationService>();
            CommonService = Substitute.For<ICommonService>();
            DateTime today = DateTime.UtcNow.Date;

            Configuration = new ResultsAndCertificationConfiguration
            {
                IPChaseBigGapsReminderSettings = new IPChaseBigGapsReminderSettings
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

            IndustryPlacementChaseBigGapsReminderFunction = new IndustryPlacementChaseBigGapsReminder(IndustryPlacementNotificationService, CommonService, Configuration, Logger);
        }

    }
}
