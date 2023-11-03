using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Utils.Ranges;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.UcasDataTransferTests
{
    public abstract class UcasDataTransferTestBase : BaseTest<UcasDataTransfer>
    {
        protected TimerSchedule TimerSchedule;

        // Dependencies
        protected IUcasDataTransferService UcasDataTransferService;
        protected ICommonService CommonService;

        // Actual function instance
        protected UcasDataTransfer UcasDataTransferFunction;

        protected DateTime Today => new(2022, 8, 19);

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            UcasDataTransferService = Substitute.For<IUcasDataTransferService>();
            CommonService = Substitute.For<ICommonService>();

            UcasDataTransferFunction = new UcasDataTransfer(UcasDataTransferService, CommonService, new ResultsAndCertificationConfiguration());
        }

        public void Setup(DateTimeRange dateTimeRange)
        {
            var config = new ResultsAndCertificationConfiguration
            {
                UcasTransferAmendmentsSettings = new UcasTransferAmendmentsSettings
                {
                    ValidDateRanges = new[] { dateTimeRange }
                }
            };

            TimerSchedule = Substitute.For<TimerSchedule>();
            UcasDataTransferService = Substitute.For<IUcasDataTransferService>();
            CommonService = Substitute.For<ICommonService>();
            UcasDataTransferFunction = new UcasDataTransfer(UcasDataTransferService, CommonService, config);
        }
    }
}
