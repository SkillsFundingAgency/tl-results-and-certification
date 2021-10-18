using Microsoft.Azure.WebJobs.Extensions.Timers;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

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

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            UcasDataTransferService = Substitute.For<IUcasDataTransferService>();
            CommonService = Substitute.For<ICommonService>();

            UcasDataTransferFunction = new UcasDataTransfer(UcasDataTransferService, CommonService);
        }
    }
}
