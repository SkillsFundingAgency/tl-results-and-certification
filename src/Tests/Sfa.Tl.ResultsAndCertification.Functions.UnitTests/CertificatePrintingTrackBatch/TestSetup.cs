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

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.CertificatePrintingTrackBatch
{
    public abstract class TestSetup : BaseTest<Functions.CertificatePrintingTrackBatch>
    {
        protected IMapper Mapper;
        protected ILogger<ICertificatePrintingService> Logger;
        protected TimerSchedule TimerSchedule;
        protected ICommonService CommonService;
        protected ICertificatePrintingService CertificatePrintingService;
        protected Functions.CertificatePrintingTrackBatch CertificatePrintingTrackBatchFunction;

        public override void Setup()
        {
            TimerSchedule = Substitute.For<TimerSchedule>();
            CommonService = Substitute.For<ICommonService>();
            Logger = Substitute.For<ILogger<ICertificatePrintingService>>();
            CertificatePrintingService = Substitute.For<ICertificatePrintingService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Startup).Assembly));
            Mapper = new AutoMapper.Mapper(mapperConfig);
            CertificatePrintingTrackBatchFunction = new Functions.CertificatePrintingTrackBatch(CommonService, CertificatePrintingService);
        }

        public async override Task When()
        {
            await CertificatePrintingTrackBatchFunction.FetchCertificatePrintingTrackBatchAsync(new TimerInfo(TimerSchedule, new ScheduleStatus()), new ExecutionContext(), new NullLogger<Functions.CertificatePrintingRequest>());
        }
    }
}
