using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Api.Client.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Functions.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.CertificatePrintingService
{
    public abstract class TestSetup : BaseTest<Functions.Services.CertificatePrintingService>
    {
        protected IMapper Mapper;
        protected ILogger<ICertificatePrintingService> Logger;
        protected IPrintingService PrintingService;
        protected IPrintingApiClient PrintingApiClient;
        protected ICertificateService CertificateService;
        protected Functions.Services.CertificatePrintingService Service;
        protected CertificatePrintingResponse ActualResult;
        protected ILoggerFactory LoggerFactory;

        public override void Setup()
        {
            Logger = Substitute.For<ILogger<ICertificatePrintingService>>();
            PrintingService = Substitute.For<IPrintingService>();
            PrintingApiClient = Substitute.For<IPrintingApiClient>();
            LoggerFactory = Substitute.For<ILoggerFactory>();
            CertificateService = Substitute.For<ICertificateService>();

            var mapperConfig = new MapperConfiguration(c => c.AddMaps(typeof(Program).Assembly), LoggerFactory);
            Mapper = new AutoMapper.Mapper(mapperConfig);

            Service = new Functions.Services.CertificatePrintingService(Mapper, Logger, PrintingApiClient, PrintingService, CertificateService);
        }
    }
}