using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificateTrackingExtractionTests
{
    public abstract class TestSetup : BaseTest<CertificateTrackingExtraction>
    {
        private readonly TimerSchedule _timerSchedule = Substitute.For<TimerSchedule>();
        protected CertificateTrackingExtraction CertificateTrackingExtraction;

        protected ICertificateTrackingExtractionService Service = Substitute.For<ICertificateTrackingExtractionService>();
        protected ICommonService CommonService = Substitute.For<ICommonService>();

        public override void Setup()
        {
            CertificateTrackingExtraction = new CertificateTrackingExtraction(Service, CommonService);
        }

        public override Task When()
        {
            return CertificateTrackingExtraction.CertificateTrackingExtractAsync(new TimerInfo(_timerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<CertificateTrackingExtraction>());
        }
    }
}