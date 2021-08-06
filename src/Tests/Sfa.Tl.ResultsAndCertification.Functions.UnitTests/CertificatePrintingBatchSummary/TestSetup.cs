using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrintingBatchSummary
{
    public abstract class TestSetup : BaseTest<Functions.CertificatePrintingBatchSummary>
    {
        protected IMapper Mapper;
        protected ILogger<ICertificatePrintingService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ICertificatePrintingService CertificatePrintingService;
        protected Functions.CertificatePrintingBatchSummary CertificatePrintingBatchSummaryFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ICertificatePrintingService>>();
            CertificatePrintingService = Substitute.For<ICertificatePrintingService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            CertificatePrintingBatchSummaryFunction = new Functions.CertificatePrintingBatchSummary(CommonService, CertificatePrintingService);
        }

        public async override Task When()
        {
            await CertificatePrintingBatchSummaryFunction.FetchCertificatePrintingBatchSummaryAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.CertificatePrintingRequest>());
        }
    }
}
